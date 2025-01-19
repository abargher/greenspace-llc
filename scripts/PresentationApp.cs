using Godot;
using System;

public partial class PresentationApp : Control
{
    public static PresentationApp Instance { get; private set; }
    public bool IsStickerInHand { get; set; }
    // total number of slides made in the presentation
    public int SlideCount { get; set; }
    // property for the currently active slide.
    public TextureButton Slide { get; set; }
    public StickerInHand CurrHeldSticker { get; set; }
    public CenterContainer SlideContainer { get; set; }
    
    [Signal]
    public delegate void PickupStickerEventHandler(string iconFilepath);

    [Signal]
    public delegate void SavePresentationEventHandler();


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
        IsStickerInHand = false;

        SlideCount = 0;

        SlideContainer = (CenterContainer)this.GetNode("SlideContainer");
        Slide = (TextureButton)this.GetNode("SlideContainer/Slide");

        // current idea is to have the panel listen for clicks,
        // then emit a signal to the presentation app saying
        // "HEY were gonna place a sticker dawg [Mouse position:]"

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void RefreshSlide()
    {
        GD.Print("refreshing slide in papp");
        var children = Slide.GetChildren();
        foreach (Node child in children) {
            if (child is not ColorRect cr) {
                GD.Print(child);
                child.QueueFree(); 
            }
        }
        //GD.Load<PackedScene>("res://scenes/name").Instantiate<Slide>();

        SlideCount += 1;
        GD.Print("Slide Count = ", SlideCount);
    }

    // this is received
    private void OnPickupSticker(string iconPath)
    {
        GD.Print("PickupSticker recv" + iconPath);
        // now create thing to follow mouse
        // instance of a StickerInHand
        StickerInHand nextPickup = GD.Load<PackedScene>("res://scenes/sticker_in_hand.tscn").Instantiate<StickerInHand>();
        Slide.AddChild(nextPickup);
        GD.Print(nextPickup);

        CurrHeldSticker = nextPickup;
        IsStickerInHand = true;
    }

    // called when the TextureButton that is the slide is clicked.
    private void OnSlidePressed()
    {
        if (IsStickerInHand) {
            // Position = GetChild.GetLocalMousePosition() - halfvec;
        GD.Print("Clicked sticker in slide! heard in presentationApp");
            // stop updating the position of the sticker
            CurrHeldSticker.hasBeenPlaced = true;
            CurrHeldSticker = null;
        } else {
            GD.Print("clicked on panel without Sticker in hand");
        }

    }

    private void OnSavePresentation()
    {
        GD.Print("SavePresentation handled in PresentationApp Scene");
        QueueFree();
    }

}
