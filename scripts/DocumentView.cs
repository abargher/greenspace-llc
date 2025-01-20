using Godot;
using Godot.Collections;
using System;

public partial class DocumentView : Control
{
	SceneManager sceneManager;
	Gameplay gameplay;
	[Export]
	public Button stampButton;
	[Export]
	Array<AudioStreamWav> sounds;
	AudioStreamWav stampSound;
	AudioStreamPlayer effectPlayer;

	TextureRect followerStamp;
	TextureRect inkSeal;

	bool isHoldingStamp = false;
	bool hasInk = false;

	Timer stampCloseTimer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sceneManager = GetNode<SceneManager>("/root/SceneManager");
		gameplay = GetNode<Gameplay>("/root/Gameplay");
		followerStamp = GetNode<TextureRect>("FollowerStamp");
		inkSeal = GetNode<TextureRect>("InkSeal");
		effectPlayer = GetNode<AudioStreamPlayer>("EffectPlayer");
		stampCloseTimer = GetNode<Timer>("StampCloseTimer");
		stampSound = sounds[0];
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!isHoldingStamp) {
			return;
		}

		Vector2 centerOffset = new Vector2(followerStamp.Size.X / 2, followerStamp.Size.Y / 2);
		followerStamp.Position = GetGlobalMousePosition() - centerOffset;
	}

	public void OnInkpadClick()
	{
		GD.Print("Inkpad clicked");
		if (!isHoldingStamp || hasInk) {
			GD.Print("Inkpad clicked without holding stamp or already has ink.");
			return;
		}
		hasInk = true;
		effectPlayer.Stream = stampSound;
		effectPlayer.Play();
	}

	public void OnStampClick()
	{
		if (isHoldingStamp) {
			GD.Print("Stamp clicked while holding stamp!!");
			return;
		}
		GD.Print("Stamp click registered");
		isHoldingStamp = true;
		stampButton.Visible = false;
		followerStamp.Visible = true;
	}

	public void OnDocumentClick()
	{
		if (!(isHoldingStamp && hasInk)) {
			GD.Print("Document clicked without stamp.");
			return;
		}
		GD.Print("Document click with stamp!");
		isHoldingStamp = false;
		stampButton.Visible = true;
		followerStamp.Visible = false;
		inkSeal.Position = GetGlobalMousePosition();
		inkSeal.Visible = true;
		effectPlayer.Stream = stampSound;
		effectPlayer.Play();
		stampCloseTimer.Start();
	}

	public void OnStampCloseTimerTimeout()
	{
		gameplay.numDocumentsStamped++;
		sceneManager.SwapScenes("res://scenes/right_view.tscn", gameplay, this, "fade_to_black");
	}
}
