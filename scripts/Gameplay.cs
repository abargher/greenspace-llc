using Godot;
using System;
using System.Collections.Generic;

public partial class Gameplay : Node
{
	[Signal]
	public delegate void DayEndEventHandler();

	[Export]
	public AudioStreamPlayer backgroundPlayer;
	AudioStreamWav currentBackgroundTrack;

    public static Gameplay Instance { get; private set; }

	public int currentDay = 1;
	public const int maxDay = 11;
	HudManager hudManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// hudManager = GetNode<HudManager>("HUDManager");
		currentBackgroundTrack = GD.Load<AudioStreamWav>($"res://assets/audio/music/office-sounds/day1.wav");
        if (Instance != null) 
        {
            GD.PushWarning("attempting to recreate instance of Gameplay");
            return;
        }
        Instance = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnDayEnd(string nextAudioPath)
	{
		if (currentDay < maxDay)
		{
			backgroundPlayer.Stop();
			currentDay++;
			backgroundPlayer.Stream = GD.Load<AudioStreamWav>(nextAudioPath);
			backgroundPlayer.Play();
		}
	}
}
