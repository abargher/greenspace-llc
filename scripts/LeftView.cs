using Godot;
using Godot.Collections;
using System;

public partial class LeftView : Node
{
	Gameplay gameplay;
	AudioStreamPlayer effectPlayer;

	[Export]
	Array<TextureRect> coworkers;

	[Signal]
	public delegate void MailboxClickEventHandler();

	const int NUM_WORKER_DAYS = 4;
	int currentDayIndex;

	SceneManager sceneManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sceneManager = GetNode<SceneManager>("/root/SceneManager");
		gameplay = GetNode<Gameplay>("/root/Gameplay");
		effectPlayer = GetNode<AudioStreamPlayer>("EffectPlayer");

		currentDayIndex = Math.Min(gameplay.currentDay - 1, NUM_WORKER_DAYS);

		for (int i = 0; i < currentDayIndex; i++) {
			coworkers[i].Visible = false;
		}

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void MoveRight()
	{
		sceneManager.SwapScenes("res://scenes/office_pc_view.tscn", GetNode<Gameplay>("/root/Gameplay"), this, "fade_to_black");
	}

	public void OnMailboxClick()
	{
		if (!gameplay.hudManager.isHoldingDocument) {
			return;
		}
		gameplay.hudManager.isHoldingDocument = false;
		gameplay.hudManager.documentFollower.Visible = false;
		effectPlayer.Play();

		// TODO: make this do something score-related
		gameplay.numDocumentsInMailbox++;
	}
}
