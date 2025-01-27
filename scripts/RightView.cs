using Godot;
using Godot.Collections;
using System;

public partial class RightView : Control
{
	[Signal]
	public delegate void StaplerDownEventHandler();
	[Signal]
	public delegate void StaplerUpEventHandler();

	[Signal]
	public delegate void PaperClickEventHandler();

	[Export]
	public AudioStreamWav staplerSound;

	[Export]
	public AudioStreamWav stampSound;

	[Export]
	public AudioStreamWav paperSound;

	[Export]
	public AudioStreamPlayer player;

	SceneManager sceneManager;
	Gameplay gameplay;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sceneManager = GetNode<SceneManager>("/root/SceneManager");
		gameplay = GetNode<Gameplay>("/root/Gameplay");
	}

	public void MoveLeft()
	{
		sceneManager.SwapScenes("res://scenes/office_pc_view.tscn", GetNode<Gameplay>("/root/Gameplay"), this, "fade_to_black");
	}

	public void OnStaplerDown()
	{
		// pivot the stapler top down
		GetNode<Button>("StaplerTop").RotationDegrees = 5.5f;
		player.Stream = staplerSound;
		player.Play();

		if (gameplay.hudManager.isHoldingDocument && !gameplay.hudManager.isDocumentStapled) {
			gameplay.hudManager.isDocumentStapled = true;
			gameplay.hudManager.documentFollower.Texture = gameplay.hudManager.stapledDocumentTexture;
			gameplay.hudManager.metricsHud.OnChangeOptimization(17);
		}
	}
	public void OnStaplerUp()
	{
		// pivot the stapler top up
		GetNode<Button>("StaplerTop").RotationDegrees = 0f;
	}

	public void OnPaperClick()
	{
		if (gameplay.dailyGreenliningPapersRemaining <= 0) {
			return;
		}
		sceneManager.SwapScenes("res://scenes/document_view.tscn", GetNode<Gameplay>("/root/Gameplay"), this, "fade_to_black");
	}
}
