using Godot;
using Godot.Collections;
using System;
using System.Globalization;

public partial class WaterCooler : Node
{
	SceneManager sceneManager;
	Gameplay gameplay;

	const int NUM_CONVERSATION_DAYS = 6;

	[Export]
	Array<string> questions;
	[Export]
	Array<Array<string>> answers;
	[Export]
	Array<string> retorts;

	Timer questionTimer;
	Timer answersTimer;
	Button returnButton;
	

	[Export]
	Array<Texture2D> speakerTextures;

	TextureRect speakingCoworker;

	[Export]
	RichTextLabel questionLabel;
	[Export]
	Array<RichTextLabel> answerButtons;
	[Export]
	VBoxContainer answerContainer;
	[Export]
	Array<Texture2D> fillerTextures;

	[Export]
	Array<TextureRect> coworkers;


	AudioStreamPlayer dialoguePlayer;

	bool allowAnswers = false;
	bool retortPlayed = false;
	int currentDayIndex;
	Random random = new();
	Timer emptyDayTimer;


	private string FormatText(string text)
	{
		return $"[center]{text}[/center]";
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sceneManager = GetNode<SceneManager>("/root/SceneManager");
		gameplay = GetNode<Gameplay>("/root/Gameplay");

		currentDayIndex = Math.Min(gameplay.currentDay - 1, NUM_CONVERSATION_DAYS - 1);

		dialoguePlayer = GetNode<AudioStreamPlayer>("DialoguePlayer");
		questionTimer = GetNode<Timer>("QuestionTimer");
		answersTimer = GetNode<Timer>("AnswersTimer");
		returnButton = GetNode<Button>("ReturnButton");
		speakingCoworker = GetNode<TextureRect>("SpeakingCoworker");
		emptyDayTimer = GetNode<Timer>("EmptyDayTimer");

		if (gameplay.currentDay > NUM_CONVERSATION_DAYS) {
			emptyDayTimer.Start();
			questionTimer.Stop();
			speakingCoworker.Visible = false;
			return;
		}

		questionLabel.Text = FormatText(questions[currentDayIndex]);
		GD.Print(questionLabel.Text);
		for (int i = 0; i < answerButtons.Count; i++)
		{
			answerButtons[i].Text = FormatText(answers[currentDayIndex][i]);
		}
		questionLabel.Visible = true;
		dialoguePlayer.Seek((float)(random.NextDouble() * dialoguePlayer.Stream.GetLength()));
		dialoguePlayer.Play();

		// get and randomize coworkers
		foreach (TextureRect coworker in coworkers)
		{
			coworker.Texture = fillerTextures[random.Next(0, fillerTextures.Count)];
			coworker.FlipH = random.Next(0, 2) == 1;
		}

		// show one less coworker each day
		for (int i = 0; i < NUM_CONVERSATION_DAYS - currentDayIndex; i++) {
			coworkers[i].Visible = true;
		}

		// set speaker texture to match lore script
		speakingCoworker.Texture = speakerTextures[currentDayIndex];
	}

	public void OnQuestionTimerTimeout()
	{
		dialoguePlayer.Stop();
		if (retortPlayed) {
			returnButton.Visible = true;
			return;
		}
		answerContainer.Visible = true;
		for (int i = 0; i < answerButtons.Count; i++)
		{
			answerButtons[i].Visible = true;
		}
		answersTimer.Start();
	}

	public void OnAnswerTimerTimeout()
	{
		allowAnswers = true;
	}

	public void OnReturnButtonClick()
	{
		questionLabel.Visible = false;
		gameplay.hasDoneWaterCooler = true;
		gameplay.SetTimeOfDay(320); // 1:20 PM is 320 minutes after 8:00 AM
		sceneManager.SwapScenes("res://scenes/office_pc_view.tscn", gameplay, this, "fade_to_black");
	}

	public void OnAnswerButtonClick()
	{
		if (!allowAnswers) {
			return;
		}

		answerContainer.Visible = false;
		if (!retortPlayed) {
			retortPlayed = true;
			questionLabel.Text = FormatText(retorts[currentDayIndex]);
			questionTimer.Start();
			dialoguePlayer.Seek((float)(random.NextDouble() * dialoguePlayer.Stream.GetLength()));
			dialoguePlayer.Play();
		}
	}

	public void OnEmptyDayTimerTimeout()
	{
		returnButton.Visible = true;
	}

	public void InitScene()
	{
		GD.Print("Initializing Water Cooler scene");
		gameplay = GetNode<Gameplay>("/root/Gameplay");

		if (gameplay.currentDay > NUM_CONVERSATION_DAYS){
			return;
		}
		
		if (!gameplay.IsPlayingWaterCoolerMusic()) {
			gameplay.StartWaterCoolerMusic();
		}
	}

}
