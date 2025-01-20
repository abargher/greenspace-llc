using Godot;
using Godot.Collections;
using System;

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
		gameplay.currentDay++;
		sceneManager.SwapScenes("res://scenes/office_pc_view.tscn", GetNode<Gameplay>("/root/Gameplay"), this, "fade_to_black");
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

	public void InitScene()
	{
		GD.Print("Initializing Water Cooler scene");
		gameplay = GetNode<Gameplay>("/root/Gameplay");

		gameplay.backgroundPlayer.Stop();
		gameplay.backgroundPlayer.Stream = gameplay.waterCoolerMusic;
		gameplay.backgroundPlayer.Play();
	}

}
