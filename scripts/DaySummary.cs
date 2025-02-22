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
	MetricsHud metricsHud;

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
		gameplay = GetNode<Gameplay>("/root/Gameplay");
		gameplay.HideHUD();
		gameplay.StopBackgroundMusic();

		// summary metrics HUD, align with main HUD
		metricsHud = GetNode<MetricsHud>("MetricsHUD");
		metricsHud.RiskManagement = gameplay.hudManager.metricsHud.RiskManagement;
		metricsHud.Innovation = gameplay.hudManager.metricsHud.Innovation;
		metricsHud.Synergy = gameplay.hudManager.metricsHud.Synergy;
		metricsHud.Efficiency = gameplay.hudManager.metricsHud.Efficiency;
		metricsHud.Optimization = gameplay.hudManager.metricsHud.Optimization;
		metricsHud.AlignProgressBars();

		if (gameplay.currentDay > 6) {
			metricsHud.ShowRiskInnovationBars();
		} else {
			metricsHud.HideRiskInnovationBars();
		}

		// Get good mid bad score
		string tempText = "";
		if (gameplay.dailyTotalScore == 2) {
			score = random.Next(37, 48);

		} else if (gameplay.dailyTotalScore == 1) {
			score = random.Next(48, 63);

		} else { // score is bad == 2
			score = random.Next(63, 69);
		}
		scoreLabel.Text = $"[font_size=80][center]{score}/100[/center][/font_size]";

		Array<int> metrics = new()
        {
            gameplay.hudManager.metricsHud.Synergy,
            gameplay.hudManager.metricsHud.Efficiency,
            gameplay.hudManager.metricsHud.Optimization
        };

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

	public void OnNextDayButtonPressed()
	{
		gameplay.AdvanceToNextDay();
		gameplay.hudManager.metricsHud.ResetAllMetrics();
		sceneManager.SwapScenes("res://scenes/apartment.tscn", gameplay, this, "fade_to_black");
	}
}
