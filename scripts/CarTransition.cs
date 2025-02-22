using Godot;
using System;

public partial class CarTransition : Control
{
	Gameplay gameplay;
	SceneManager sceneManager;
	AudioStreamPlayer doorPlayer;
	AudioStreamPlayer enginePlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sceneManager = GetNode<SceneManager>("/root/SceneManager");
		gameplay = GetNode<Gameplay>("/root/Gameplay");

		doorPlayer = GetNode<AudioStreamPlayer>("DoorPlayer");
		enginePlayer = GetNode<AudioStreamPlayer>("EnginePlayer");
		doorPlayer.Play();
	}

	public void InitScene()
	{
		gameplay.StopBackgroundMusic();
		gameplay.HideHUD();
	}

	public async void OnDoorFinish()
	{
		enginePlayer.Play();
		await ToSignal(enginePlayer, "finished");
		sceneManager.SwapScenes("res://scenes/car_view.tscn", GetNode<Gameplay>("/root/Gameplay"), this, "fade_to_black");
	}
}
