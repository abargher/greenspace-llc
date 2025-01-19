using Godot;
using System;

public partial class TitleScreen : Control
{
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

	public void OnButtonUp() {
		GD.Print("Title screen exiting, starting game");
		sceneManager.SwapScenes("res://scenes/gameplay.tscn", GetTree().Root, this, "fade_to_black");
	}
}
