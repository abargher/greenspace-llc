using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class Gameplay : Node
{
	public enum MusicType
	{
		NONE,
		OFFICE,
		WATERCOOLER
	}

	SceneManager sceneManager;
	[Signal]
	public delegate void DayEndEventHandler();
	[Signal]
	public delegate void TimeChangedEventHandler();

	[Export]
	public Array<AudioStreamWav> officeSounds;
	[Export]
	public AudioStreamWav waterCoolerMusic;

	[Export]
	public AudioStreamPlayer backgroundPlayer;
	public MusicType musicType = MusicType.OFFICE;


	public int currentDay = 8;
	public const int maxDay = 11;
    public int dailyPowerpointsRemaining { get; set; }
    public int dailyGreenliningPapersRemaining { get; set; }
    public int dailyFluffEmailsRemaining { get; set; }
	public Array<Email> currentDayEmails = new();
    public bool hasDoneWaterCooler {get; set; }

	public int numDocumentsStamped { get; set; } // Need some stamped to submit to mailbox
	public int numDocumentsInMailbox { get; set; }
	public int numPowerpointsCompleted { get; set; }

	public const int STARTING_TIME = 0; // 8:00 AM
	public const int MID_DAY_TIME = 300;  // 1:00 PM, water cooler time
	public const int ENDING_TIME = 660; // 7:00 PM

	public int numMinutesInCurrentDay { get; set; }

	// either 0, 1, or 2 == good, mid, bad
	public int dailyTotalScore;

	[Export]
	Texture2D arrow;
	[Export]
	Texture2D pointingHand;

	public HudManager hudManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        Input.SetCustomMouseCursor(arrow);
        Input.SetCustomMouseCursor(pointingHand, Input.CursorShape.PointingHand);

		sceneManager = GetNode<SceneManager>("/root/SceneManager");
		sceneManager.SceneAdded += OnSceneAdd;
		hudManager = GetNode<HudManager>("HUDManager");

		backgroundPlayer.Stream = officeSounds[Math.Min(currentDay - 1, officeSounds.Count - 1)];
		backgroundPlayer.Play();

		// register Day End handler
		DayEnd += OnDayEnd;

	}

    public override void _ExitTree()
    {
		sceneManager.SceneAdded -= OnSceneAdd;
		DayEnd -= OnDayEnd;
    }

    public bool DoneWithTasks()
    {
        GD.Print("---------");
        GD.Print(dailyPowerpointsRemaining,dailyGreenliningPapersRemaining,dailyFluffEmailsRemaining);
        return dailyPowerpointsRemaining <= 0 && dailyFluffEmailsRemaining <= 0 && dailyGreenliningPapersRemaining <= 0;
    }

    public void IfDoneWithTasksSwapScene()
    {
        GD.Print("checkin if done in Gameplay swapscene");
        // if user has completed all tasks and has not done the watercooler, immediately move them to the water cooler
        if (DoneWithTasks())
        {
            if (!hasDoneWaterCooler)
            {
                sceneManager.SwapScenes("res://scenes/water_cooler.tscn", GetNode<Gameplay>("/root/Gameplay"), GetNode<OfficePcView>("OfficePCView"), "fade_to_black");
                GD.Print("Swapping Scenes to WATER COOLER");
            } else {
				// if user has completed all tasks and watercooler, immediately move to end of day.
				if (currentDay < maxDay) {
					sceneManager.SwapScenes("res://scenes/day_summary.tscn", GetNode<Gameplay>("/root/Gameplay"), GetNode<OfficePcView>("OfficePCView"), "fade_to_black");
				} else {
					// game over, go back to title
					sceneManager.SwapScenes("res://scenes/title_screen.tscn", null, this, "fade_to_black");
				}
                GD.Print("Swapping Scenes to EOD");
            }
        }
    }

    public void OnDayEnd()
	{
		if (currentDay < maxDay)
		{
			backgroundPlayer.Stop();
			backgroundPlayer.Stream = officeSounds[Math.Min(currentDay - 1, officeSounds.Count)];

			sceneManager.SwapScenes("res://scenes/day_summary.tscn", this, this.GetChild(0), "fade_to_black");
		}
		// else {
		// 	sceneManager.SwapScenes("res://scenes/title_screen.tscn", null, this, "fade_to_black");
		// }
	}

	// keeps HUD in front when scene changes
	public void OnSceneAdd(Node incomingScene, LoadingScreen loadingScreen) {
		this.MoveChild(hudManager, GetChildCount() - 1);
		this.MoveChild(backgroundPlayer, GetChildCount() - 1);
	}

	public void SetTimeOfDay(int numMinutes)
	{
		this.numMinutesInCurrentDay = numMinutes;
		EmitSignal(SignalName.TimeChanged);
	}

	public void IncrementTimeOfDay(int numMinutes)
	{
		this.numMinutesInCurrentDay += numMinutes;
		EmitSignal(SignalName.TimeChanged);
		if (numMinutesInCurrentDay >= ENDING_TIME)
		{
			GD.Print("Day End happening");
			EmitSignal(SignalName.DayEnd);
		} else if (numMinutesInCurrentDay >= MID_DAY_TIME && !hasDoneWaterCooler)
		{
			sceneManager.SwapScenes("res://scenes/water_cooler.tscn", this, this.GetChild(0), "fade_to_black");
		}
	}

	public bool IsPlayingOfficeSounds()
	{
		return musicType == MusicType.OFFICE;
	}

	public bool IsPlayingWaterCoolerMusic()
	{
		return musicType == MusicType.WATERCOOLER;
	}

	public void StartOfficeSounds()
	{
		this.musicType = MusicType.OFFICE;

		this.backgroundPlayer.Stop();
		int currBackgroundIndex = Math.Min(this.currentDay - 1, this.officeSounds.Count - 1);
		this.backgroundPlayer.Stream = this.officeSounds[currBackgroundIndex];
		this.backgroundPlayer.Play();
	}

	public void StartWaterCoolerMusic()
	{
		this.musicType = MusicType.WATERCOOLER;

		this.backgroundPlayer.Stop();
		this.backgroundPlayer.Stream = this.waterCoolerMusic;
		this.backgroundPlayer.Play();
	}
	public void StopBackgroundMusic()
	{
		musicType = MusicType.NONE;
		if (backgroundPlayer.Playing) {
			backgroundPlayer.Stop();
		}
	}
	
	public void HideHUD()
	{
		if (hudManager.Visible) {
			hudManager.Visible = false;
		}
	}

	public void ShowHUD()
	{
		if (!hudManager.Visible) {
			hudManager.Visible = true;
		}
	}

	public void AdvanceToNextDay()
	{
		this.currentDay++;
		if(currentDay > 6) { // TODO: check if this is the day for promotion
			hudManager.metricsHud.ShowRiskInnovationBars();
		}

		this.numMinutesInCurrentDay = STARTING_TIME;

		// Clear all emails from previous day
		this.currentDayEmails.Clear();
		this.dailyPowerpointsRemaining = 0;
		this.dailyGreenliningPapersRemaining = 0;
		this.dailyFluffEmailsRemaining = 0;

		// reset daily progress trackers
		this.hasDoneWaterCooler = false;
		this.numDocumentsStamped = 0;
		this.numDocumentsInMailbox = 0;
		this.numPowerpointsCompleted = 0;
		this.hudManager.metricsHud.ResetAllMetrics();
	}
}
