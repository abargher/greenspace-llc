using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class CarView : Control
{

	[Export]
	public Array<AudioStreamWav> radioSongs;

	[Export]
	public AudioStreamPlayer backgroundPlayer;
	[Export]
	public AudioStreamPlayer songPlayer;
	[Export]
	public AudioStreamPlayer dialoguePlayer;
	[Export]
	public TextureRect backgroundImage;
	[Export]
	public TextureRect wheel;

	const float minWheelRotation = -30;
	const float maxWheelRotation = 30;

	AudioStreamWav radioSong;
	double timeElapsed = 0;
	const float speed = 1.0f;
	const float rotationSpeed = 0.3f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Random random = new Random();
		int trackNum = random.Next(0, 5);
		GD.Print(trackNum);
		GD.Print(radioSongs[trackNum]);
		
		// disable global background music
		EmitSignal(Gameplay.SignalName.UpdateBackgroundTrack, null);

		// play the radio song
		GD.Print("Playing radio song");
		songPlayer.Stream = radioSongs[trackNum];
		songPlayer.Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("ui_left"))
		{
			wheel.RotationDegrees = Mathf.Clamp(wheel.RotationDegrees - (rotationSpeed * 2f), minWheelRotation, maxWheelRotation);
			backgroundImage.Position += new Vector2(speed, 0);
		}
		else if (Input.IsActionPressed("ui_right"))
		{
			wheel.RotationDegrees = Mathf.Clamp(wheel.RotationDegrees + (rotationSpeed * 2f), minWheelRotation, maxWheelRotation);
			backgroundImage.Position -= new Vector2(speed, 0);
		} else {
			timeElapsed += delta;
			backgroundImage.Position += new Vector2((float)Math.Cos(timeElapsed) * speed, 0);
			wheel.RotationDegrees = Mathf.Clamp(wheel.RotationDegrees - (float)Math.Cos(timeElapsed + 1.0) * rotationSpeed, minWheelRotation, maxWheelRotation);
		}

	}

	public void ReceiveData(Dictionary data) {

	}


}

/* 
- Can await the Finished signal from the AudioStreamPlayer

*/