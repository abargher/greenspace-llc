using Godot;
using System;

public partial class LeftView : Node
{
	Gameplay gameplay;
	AudioStreamPlayer effectPlayer;

	[Signal]
	public delegate void MailboxClickEventHandler();

	SceneManager sceneManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sceneManager = GetNode<SceneManager>("/root/SceneManager");
		gameplay = GetNode<Gameplay>("/root/Gameplay");
		effectPlayer = GetNode<AudioStreamPlayer>("EffectPlayer");
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

		// document is submitted, remove it from the 'queue'
		gameplay.numDocumentsStamped--;
		// TODO: make this do something score-related
	}
}
