using Godot;
using System;

public partial class Clock : RichTextLabel
{
	Gameplay gameplay;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameplay = GetNode<Gameplay>("/root/Gameplay");
		gameplay.TimeChanged += OnTimeChanged;
		Text = FormatTime();
	}

	public override void _ExitTree()
	{
		gameplay.TimeChanged -= OnTimeChanged;
	}


	public string FormatTime()
	{
		int hours = gameplay.numMinutesInCurrentDay / 60 + 8;
		if (hours > 12) {
			hours -= 12;
		}
		int minutes = gameplay.numMinutesInCurrentDay % 60;

		return $"[font_size=130][center]{hours:00}:{minutes:00}[/center][/font_size]";
	}

	public void OnTimeChanged()
	{
		Text = FormatTime();
	}
}
