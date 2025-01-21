using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class Gameplay : Node
{
	SceneManager sceneManager;
	[Signal]
	public delegate void DayEndEventHandler();


	[Signal]
	public delegate void StopBackgroundMusicEventHandler();

	[Export]
	public Array<AudioStreamWav> officeSounds;
	[Export]
	public AudioStreamWav waterCoolerMusic;

	[Export]
	public AudioStreamPlayer backgroundPlayer;

    public static Gameplay Instance { get; private set; }

	public int currentDay = 9;
	public const int maxDay = 11;
    public int dailyPowerpointsRemaining { get; set; }
    public int dailyGreenliningPapersRemaining { get; set; }
    public int dailyFluffEmailsRemaining { get; set; }
    public bool hasDoneWaterCooler {get; set; }

	public int numDocumentsStamped { get; set; } // Need some stamped to submit to mailbox
	public int numDocumentsInMailbox { get; set; }
	public int numPowerpointsCompleted { get; set; }

	public HudManager hudManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sceneManager = GetNode<SceneManager>("/root/SceneManager");
		sceneManager.SceneAdded += OnSceneAdd;
		hudManager = GetNode<HudManager>("HUDManager");
		
		backgroundPlayer.Stream = officeSounds[Math.Min(currentDay - 1, officeSounds.Count)];
		backgroundPlayer.Play();

        if (Instance != null) 
        {
            GD.PushWarning("attempting to recreate instance of Gameplay");
            return;
        }
        Instance = this;
	}

    public bool DoneWithTasks()
    {
        GD.Print("---------");
        GD.Print(dailyPowerpointsRemaining,dailyGreenliningPapersRemaining,dailyFluffEmailsRemaining);
        if (dailyPowerpointsRemaining <= 0 && dailyFluffEmailsRemaining <= 0 && dailyGreenliningPapersRemaining <= 0)
        {
            return true;
        }
        return false;
    }
    public void IfDoneWithTasksSwapScene()
    {
        GD.Print("checkin if done in Gameplay swapscene");
        // if user has completed all tasks and has not done the watercooler, immediately move them to the water cooler
        if (DoneWithTasks())
        {
            if (!hasDoneWaterCooler)
            {
                sceneManager.SwapScenes("res://scenes/water_cooler.tscn", GetNode<Gameplay>("/root/Gameplay"), GetNode("OfficePCView"), "fade_to_black");
                GD.Print("Swapping Scenes to WATER COOLER");
            } else {
                // go home
                sceneManager.SwapScenes("res://scenes/day_summary.tscn", GetNode<Gameplay>("/root/Gameplay"), GetNode("OfficePCView"), "fade_to_black");
                GD.Print("Swapping Scenes to EOD");
            }
        }

        // if user has completed al tasks and watercooler, immediately move to end of day. 
    }
	public void OnDayEnd()
	{
		if (currentDay < maxDay)
		{
			backgroundPlayer.Stop();
			currentDay++;
			backgroundPlayer.Stream = officeSounds[Math.Min(currentDay - 1, officeSounds.Count)];
			// TODO: test day end event and if background music changes and continues playing
			// backgroundPlayer.Play();
		}
	}

	// keeps HUD in front when scene changes
	public void OnSceneAdd(Node incomingScene, LoadingScreen loadingScreen) {
		this.MoveChild(hudManager, GetChildCount() - 1);
	}
}
