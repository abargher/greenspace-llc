using Godot;
using System;

public partial class OfficePcView : Control
{
    public static MetricsHud metricsHud { get; private set; }
	public override void _Ready()
	{
        metricsHud = GetNode<MetricsHud>("/root/Gameplay/HUDManager/MetricsHUD"); 
        metricsHud.OnChangeSEO(10,5,2);
        
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
