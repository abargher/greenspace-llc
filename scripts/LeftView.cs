using Godot;
using Godot.Collections;
using System;

public partial class LeftView : Node
{
	Gameplay gameplay;
	AudioStreamPlayer effectPlayer;

	[Export]
	Array<TextureRect> coworkers;

	[Export]
	Array<Texture2D> coworkerTextures;

	[Signal]
	public delegate void MailboxClickEventHandler();

	const int NUM_WORKER_DAYS = 4;
	int currentDayIndex;

	SceneManager sceneManager;
	Random random = new();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sceneManager = GetNode<SceneManager>("/root/SceneManager");
		gameplay = GetNode<Gameplay>("/root/Gameplay");
		effectPlayer = GetNode<AudioStreamPlayer>("EffectPlayer");

		currentDayIndex = Math.Min(gameplay.currentDay - 1, NUM_WORKER_DAYS);

		for (int i = 2; i < coworkers.Count; i++) {
			coworkers[i].Texture = coworkerTextures[random.Next(0, coworkerTextures.Count)];
			coworkers[i].FlipH = random.Next(0, 2) == 1;
		}

		for (int i = 0; i < currentDayIndex; i++) {
			coworkers[i].Visible = false;
		}

	}

	public void MoveRight()
	{
		sceneManager.SwapScenes("res://scenes/office_pc_view.tscn", GetNode<Gameplay>("/root/Gameplay"), this, "fade_to_black");
	}

	public void OnMailboxClick()
	{
		if (!gameplay.hudManager.isHoldingDocument) {
			return;
		}
		gameplay.hudManager.isHoldingDocument = false;
		gameplay.hudManager.documentFollower.Visible = false;
		effectPlayer.Play();
		if (gameplay.hudManager.metricsHud.Optimization > gameplay.hudManager.metricsHud.Efficiency) {
			gameplay.hudManager.metricsHud.OnChangeEfficiency(7);
		} else {
			gameplay.hudManager.metricsHud.OnChangeOptimization(7);
		}

		gameplay.numDocumentsInMailbox++;
	}
}
