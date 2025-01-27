using Godot;
using System;

public partial class BlackScreenDelay : Control
{
	SceneManager sceneManager;
	Timer delayTimer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		delayTimer = GetNode<Timer>("DelayTimer");
		sceneManager = GetNode<SceneManager>("/root/SceneManager");
	}

	public void OnDelayTimeout()
	{
		sceneManager.SwapScenes("res://scenes/title_screen.tscn", null, this, "fade_to_black");
	}

}
