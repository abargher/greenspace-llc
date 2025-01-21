using Godot;
using System;

public partial class PresentationSticker : MarginContainer
{
    [Export]
    public string iconPath { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        SetIcon();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void SetIcon()
    {
        GetChild<Button>(0).Icon = GD.Load<Texture2D>(iconPath);
    }
    private void OnStickerButtonPressed()
    {
        PresentationApp.Instance.EmitSignal(PresentationApp.SignalName.PickupSticker, iconPath);
        GD.Print("PickupSticker handled in PresentationSticker Scene");
    }
}
