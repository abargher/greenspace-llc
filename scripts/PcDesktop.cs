using Godot;
using System;

public partial class PcDesktop : Node
{
	[Export]
	PresentationApp presentationApp;
	[Export]
	EmailApp emailApp;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnPresButtonPressed()
	{
		emailApp.Visible = false;
		presentationApp.Visible = true;
	}
	public void OnEmailButtonPressed()
	{
		emailApp.Visible = true;
		presentationApp.Visible = false;
	}

}
