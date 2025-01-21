using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class CarView : Control
{
	SceneManager sceneManager;
	Gameplay gameplay;

	[Export]
	public Array<string> radioBlurbs;

	
	[Export]
	public Array<AudioStreamWav> radioSongs;

	[Export]
	public AudioStreamPlayer backgroundPlayer;
	[Export]
	public AudioStreamPlayer songPlayer;
	[Export]
	public AudioStreamPlayer dialoguePlayer;
	[Export]
	public Timer typingTimer;
	[Export]
	public RichTextLabel dialogueLabel;
	[Export]
	public TextureRect backgroundImage;
	[Export]
	public TextureRect wheel;
	[Export]
	public Timer delayTimer;
	[Export]
	public PackedScene houseScene;

	const float minWheelRotation = -30;
	const float maxWheelRotation = 30;

	AudioStreamWav radioSong;
	double timeElapsed = 0;
	const float speed = 1.0f;
	const float rotationSpeed = 0.3f;
	Random random = new();


	// For house generation:
	Vector2 startPosRight = new Vector2(1440, -120);
	Vector2 endPosRight = new Vector2(2600, 60);
	Vector2 startPosLeft = new Vector2(1330, -120);
	Vector2 endPosLeft = new Vector2(140, 60);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sceneManager = GetNode<SceneManager>("/root/SceneManager");
		gameplay = GetNode<Gameplay>("/root/Gameplay");

		int currentDay;
		// currentDay = gameplay.currentDay;
		currentDay = 2;

		// set text for today
		dialogueLabel.Text = $"[font_size=32][color=black]{radioBlurbs[currentDay - 1]}\n\n[/color][/font_size]";

		int trackNum = random.Next(0, 5);
		GD.Print(trackNum);
		GD.Print(radioSongs[trackNum]);
		
		// play the radio song
		GD.Print("Playing radio song");
		songPlayer.Stream = radioSongs[trackNum];
		songPlayer.Play();
	}

	public void InitScene()
	{
		if (gameplay.backgroundPlayer.Playing) {
			gameplay.backgroundPlayer.Stop();
		}
		if (gameplay.hudManager.Visible) {
			gameplay.hudManager.Visible = false;
		}
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

	// Implement this method to receive data from the previous scene during the SceneManager handoff.
	public void ReceiveData() {

	}

	public void OnSongPlayerFinished()
	{
		// begin news announcement 
		dialoguePlayer.Play();
		typingTimer.Start();
		dialogueLabel.Visible = true;
	}

	public void OnDialoguePlayerFinished()
	{
		delayTimer.Start();
	}

	public void OnDelayTimerFinished()
	{
		backgroundPlayer.Stop();
		sceneManager.SwapScenes("res://scenes/office_pc_view.tscn", GetNode<Gameplay>("/root/Gameplay"), this, "fade_to_black");
	}

	public void GrowHouse()
	{
		int isRight = random.Next(0, 2);
		House newHouse = houseScene.Instantiate<House>();
		newHouse.PivotOffset = new Vector2(550, 550);
		if (isRight == 1) { // right
		 	newHouse.Position = startPosRight;
			newHouse.startPos = startPosRight;
			newHouse.endPos = endPosRight;
			newHouse.SetupHouse();
		} else { // left
		  	newHouse.isLeft = true;
		 	newHouse.Position = startPosLeft;
			newHouse.startPos = startPosLeft;
			newHouse.endPos = endPosLeft;
			newHouse.SetupHouse();
		}
		backgroundImage.AddChild(newHouse);
		backgroundImage.MoveChild(newHouse, 0);
	}


}
