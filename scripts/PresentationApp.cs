using Godot;
using System;

public partial class PresentationApp : Control
{
    public static PresentationApp Instance { get; private set; }
    public bool IsStickerInHand { get; set; }
    
    [Signal]
    public delegate void PickupStickerEventHandler(string iconFilepath);


    // makes this class a singleton
    // only ever going to have one PresentationApp
    public override void _EnterTree()
    {
        if (Instance != null) 
        {
            GD.PushWarning("attempting to recreate instance of PresentationApp");
            return;
        }
        Instance = this;
    }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        // create all the sticker buttons here 
        // (and disable those that aren't unlocked)
        // add argument to PresentationSticker constructor 
        // for sticker-image's filepath

        // rn default to true, will later start as false and be set to true 
        // upon StickerButton clicks
        IsStickerInHand = true;

        // current idea is to have the panel listen for clicks,
        // then emit a signal to the presentation app saying
        // "HEY were gonna place a sticker dawg [Mouse position:]"

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    // this is received
    private void OnPickupSticker(string iconPath)
    {
        GD.Print("PickupSticker recv" + iconPath);
        // now create thing to follow mouse
        // instance of a StickerInHand
    }

    // called when the TextureButton that is the slide is clicked.
    private void notOnPanelPressed()
    {
        GD.Print("clicked on panel!");
    }

}
