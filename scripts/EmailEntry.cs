using Godot;
using System;
using System.Data;

public partial class EmailEntry: Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnEmailEntryCloseButtonPressed()
	{
		// Emit the signal
		GD.Print("Email Entry clicked");
		EmailApp.Instance.EmitSignal(EmailApp.SignalName.EmailAppClose);
	}
}
