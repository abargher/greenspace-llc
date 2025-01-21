using Godot;
using Godot.Collections;
using System;

public partial class DaySummary : Control
{
	SceneManager sceneManager;
	Gameplay gameplay;
	[Export]
	RichTextLabel scoreLabel;
	[Export]
	RichTextLabel feedbackLabel;

	[Export]
	Array<string> scoreFeedbacks; // good, mid, bad
	[Export]
	Array<string> metricBads; // S E O
	[Export]
	Array<string> metricGoods; // S E O
	int score;
	// [37-48)
	// [48-63)
	// [63-68]

	Random random = new();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sceneManager = GetNode<SceneManager>("/root/SceneManager");
		// Get good mid bad score
		string tempText = "";
		if (gameplay.dailyTotalScore == 0) {
			score = random.Next(37, 48);

		} else if (gameplay.dailyTotalScore == 1) {
			score = random.Next(48, 63);

		} else { // score is bad == 2
			score = random.Next(63, 69);
		}
		scoreLabel.Text = $"[font_size=80][center]{score}/100[/center][/font_size]";

		Array<int> metrics = new Array<int>();
		metrics.Add(gameplay.hudManager.metricsHud.Synergy);
		metrics.Add(gameplay.hudManager.metricsHud.Efficiency);
		metrics.Add(gameplay.hudManager.metricsHud.Optimization);

		int minIndex = 0;
		int maxIndex = 0;
		foreach (int metric in metrics) {
			if (metric < metrics[minIndex]) {
				minIndex = metrics.IndexOf(metric);
			}
			if (metric > metrics[maxIndex]) {
				maxIndex = metrics.IndexOf(metric);
			}
		}


		tempText += FormatText(scoreFeedbacks[gameplay.dailyTotalScore]);
		tempText += FormatText(metricBads[minIndex]);
		tempText += FormatText(metricGoods[maxIndex]);

		feedbackLabel.Text = $"[font_size=32]{tempText}[/font_size]";
	}

	private string FormatText(string text)
	{
		return $"[center]{text}[/center]\n";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnNextDayButtonPressed()
	{
		gameplay.currentDay++;
		sceneManager.SwapScenes("res://scenes/apartment.tscn", gameplay, this, "fade_to_black");
	}
}
