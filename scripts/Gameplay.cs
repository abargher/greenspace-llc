using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class Gameplay : Node
{
	[Signal]
	public delegate void DayEndEventHandler();

	[Signal]
	public delegate void StopBackgroundMusicEventHandler();

	[Export]
	public Array<AudioStreamWav> officeSounds;

	[Export]
	public AudioStreamPlayer backgroundPlayer;

    public static Gameplay Instance { get; private set; }

	public int currentDay = 1;
	public const int maxDay = 11;
    public int dailyPowerpointsRemaining { get; set; }
    public int dailyGreenliningPapersRemaining { get; set; }
    public bool hasDoneWaterCooler {get; set; }
	HudManager hudManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
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
			// backgroundPlayer.Play();
		}
	}
}
