using Godot;
using Godot.Collections;
using System;

public partial class WaterCooler : Node
{
	SceneManager sceneManager;
	Gameplay gameplay;

	const int NUM_CONVERSATION_DAYS = 6;

	[Export]
	Array<string> questions;
	[Export]
	Array<Array<string>> answers;
	[Export]
	string retort;

	[Export]
	RichTextLabel questionLabel;
	[Export]
	Array<RichTextLabel> answerButtons;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sceneManager = GetNode<SceneManager>("/root/SceneManager");
		int currentDayIndex;
		// gameplay = GetNode<Gameplay>("/root/Gameplay");
		// currentDayIndex = Math.Max(gameplay.currentDay - 1, NUM_CONVERSATION_DAYS);
		currentDayIndex = 0;

		


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}
