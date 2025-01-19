using Godot;
using System;

public partial class Slide : TextureButton
{
    [Signal]
    public delegate void SlidePressedEventHandler();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
    // when the texture button on the slide is pressed,
    // emit signal that can generically be picked up by
    // the presentation app
    private void OnSlidePressed() {
        GD.Print("slide sees itself as clicked");
        EmitSignal(SignalName.SlidePressed);
    }
}
