using Godot;
using System;

public partial class OfficePcView : Control
{
	SceneManager sceneManager;
    public static MetricsHud metricsHud { get; private set; }
	public override void _Ready()
	{
		sceneManager = GetNode<SceneManager>("/root/SceneManager");

        metricsHud = GetNode<MetricsHud>("/root/Gameplay/HUDManager/MetricsHUD"); 
		metricsHud.CallDeferred(nameof(MetricsHud.OnChangeSEO), 10, 5, 2);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void MoveLeft()
	{
		sceneManager.SwapScenes("res://scenes/left_view.tscn", GetNode<Gameplay>("/root/Gameplay"), this, "fade_to_black");
	}

	public void MoveRight()
	{
		sceneManager.SwapScenes("res://scenes/right_view.tscn", GetNode<Gameplay>("/root/Gameplay"), this, "fade_to_black");

	}
}
