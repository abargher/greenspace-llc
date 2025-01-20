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

	public int currentDay = 1;
	public const int maxDay = 11;
    public int dailyPowerpointsRemaining { get; set; }
    public int dailyGreenliningPapersRemaining { get; set; }
    public bool hasDoneWaterCooler {get; set; }

	public int numDocumentsStamped { get; set; } // Need some stamped to submit to mailbox
	public bool isHoldingDocument { get; set; }  // holding paper for submission to mailbox

	HudManager hudManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sceneManager = GetNode<SceneManager>("/root/SceneManager");
		sceneManager.SceneAdded += OnSceneAdd;
		hudManager = GetNode<HudManager>("HUDManager");
		
        dailyPowerpointsRemaining = 0;
        dailyGreenliningPapersRemaining = 0;
		backgroundPlayer.Stream = officeSounds[Math.Min(currentDay - 1, officeSounds.Count)];
		backgroundPlayer.Play();

        if (Instance != null) 
        {
            GD.PushWarning("attempting to recreate instance of Gameplay");
            return;
        }
        Instance = this;
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
