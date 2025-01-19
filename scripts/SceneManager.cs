using Godot;
using System;

public partial class SceneManager : Control
{
	[Signal]
	public delegate void LoadStartEventHandler();  // An asset begins loading
	[Signal]
	public delegate void SceneAddedEventHandler();  // After asset is added to SceneTree, but before animation over
	[Signal]
	public delegate void LoadCompleteEventHandler();  // When loading has completed


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// connect signals
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
