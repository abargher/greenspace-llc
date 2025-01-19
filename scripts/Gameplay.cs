using Godot;
using System;

public partial class Gameplay : Node
{
	HudManager hudManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		hudManager = GetNode<HudManager>("HUDManager");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnGameplaySceneAdded()
	{

	}
}
