using Godot;
using System;
using System.Threading.Tasks;

public partial class Apartment : Control
{
	SceneManager sceneManager;
	AudioStreamPlayer alarmPlayer;
	AudioStreamPlayer bedPlayer;
	ColorRect curtain;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sceneManager = GetNode<SceneManager>("/root/SceneManager");
		alarmPlayer = GetNode<AudioStreamPlayer>("AlarmPlayer");
		bedPlayer = GetNode<AudioStreamPlayer>("BedPlayer");
		curtain = GetNode<ColorRect>("Curtain");
		alarmPlayer.Play();
	}

	public async void OnAlarmFinish()
	{
		curtain.Visible = false;
		alarmPlayer.Stop();
		bedPlayer.Play();
		await ToSignal(bedPlayer, "finished");
		bedPlayer.Stop();
		sceneManager.SwapScenes("res://scenes/car_transition.tscn", GetNode<Gameplay>("/root/Gameplay"), this, "fade_to_black");
	}
}
