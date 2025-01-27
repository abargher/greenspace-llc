using Godot;
using System;

public partial class EmailEntry : Panel
{
	[Signal]
	public delegate void EmailSelectedEventHandler(Email emailContents);

	[Signal]
	public delegate void EmailAppCloseEventHandler();

	// Called when the node enters the scene tree for the first time.
    public int IndexInQueue { get; set; }

	public EmailApp emailApp;
	public Email emailContents;

	public override void _Ready()
	{
	}

    public void OnEmailPreviewPressed()
    {
		EmitSignal(SignalName.EmailSelected, emailContents);
    }

    public void OnEmailEntryCloseButtonPressed()
	{
		// Emit the signal
		EmitSignal(SignalName.EmailAppClose);
	}
}
