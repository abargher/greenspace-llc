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
	RichTextLabel questionLabel;
	[Export]
	Array<RichTextLabel> answerButtons;
	[Export]
	VBoxContainer answerContainer;

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
		// gameplay = GetNode<Gameplay>("/root/Gameplay");
		// currentDayIndex = Math.Max(gameplay.currentDay - 1, NUM_CONVERSATION_DAYS);
		currentDayIndex = 0;

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
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

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

}
