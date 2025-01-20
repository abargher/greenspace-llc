using Godot;
using System;

/*
Allows the label 
*/
public partial class TypingLabel : RichTextLabel
{
	[Export]
	public Timer typingTimer;

	public override void _Ready()
	{
		this.VisibleRatio = 0.0f;
		this.Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// this.VisibleRatio = Mathf.Clamp((float)(timeElapsed / durationSeconds), 0, 1.0f);
		this.VisibleRatio = (float)((typingTimer.WaitTime - typingTimer.TimeLeft) / typingTimer.WaitTime);
	}
}
