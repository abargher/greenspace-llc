using Godot;
using System;
using System.Collections.Generic;

public partial class Gameplay : Node
{
	[Signal]
	public delegate void DayEndEventHandler();

	[Export]
	public AudioStreamPlayer officePlayer;

	public int currentDay = 1;
	public const int maxDay = 11;
	List<AudioStreamWav> officeSounds = new();
	HudManager hudManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// hudManager = GetNode<HudManager>("HUDManager");

		// Load all office sound clips
		for (int i = 1; i <= maxDay; i++)
		{
			officeSounds.Add(GD.Load<AudioStreamWav>($"res://assets/audio/music/office-sounds/day-{i}.wav"));
		}

		officePlayer.Stream = officeSounds[currentDay - 1];
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnDayEnd()
	{
		if (currentDay < maxDay)
		{
			officePlayer.Stop();
			currentDay++;
			officePlayer.Stream = officeSounds[currentDay - 1];
			officePlayer.Play();
		}
	}
}
