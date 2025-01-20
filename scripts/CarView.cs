using Godot;
using System;

public partial class CarView : Control
{
	double timeElapsed = 0;
	const double speed = 1.0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		timeElapsed += delta;
		Math.Sin(timeElapsed);
	}
}

/* 
- Can await the Finished signal from the AudioStreamPlayer

*/