using Godot;
using System;

public partial class LeftView : Node
{
	[Signal]
	public delegate void MailboxClickEventHandler();

	SceneManager sceneManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sceneManager = GetNode<SceneManager>("/root/SceneManager");
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
		// TODO: handle submission for documents
		// idea: on pick up document from table, attach to HUD gameplay layer. Then, it persists across Gameplay scenes
		// until freed when clicked on mailbox, i.e., this method calls QueueFree() on the document.
	}
}
