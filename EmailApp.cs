using Godot;
using System;

public partial class EmailApp : Control
{
	public static EmailApp Instance { get; private set; }
	[Signal]
	public delegate void EmailAppCloseEventHandler();

	[Signal]
	public delegate void EmailAppReplyEventHandler();

    public override void _EnterTree()
    {
		if (Instance != null) {
			GD.PushWarning("Attempted to re-create another instance of EmailApp. Ignoring.");
			return;
		}
		Instance = this;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnEmailAppClose()
	{
		GD.Print("EmailAppClose");
	}

	public void OnEmailAppReply()
	{
		GD.Print("EmailAppReply");
	}
}
