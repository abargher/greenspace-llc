using Godot;
using System;

public partial class EmailEntry : Panel
{
	// Called when the node enters the scene tree for the first time.
    public int IndexInQueue { get; set; }
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void OnEmailPreviewPressed()
    {
        GD.Print("Email ", IndexInQueue);
        EmailApp.Instance.EmitSignal(EmailApp.SignalName.EmailSelected, IndexInQueue);
    }
    public void OnEmailEntryCloseButtonPressed()
	{
		// Emit the signal
		GD.Print("Email Entry clicked");
		EmailApp.Instance.EmitSignal(EmailApp.SignalName.EmailAppClose);
	}
}
