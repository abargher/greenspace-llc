using Godot;
using System;

public partial class PcDesktop : Node
{
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
		GetNode<PresentationApp>("PresentationApp").Visible = true;
		GetNode<EmailApp>("EmailApp").Visible = false;
	}
	public void OnEmailButtonPressed()
	{
		GetNode<EmailApp>("EmailApp").Visible = true;
		GetNode<PresentationApp>("PresentationApp").Visible = false;
	}

}
