using Godot;
using System;

public partial class PresentationApp : Control
{
    public bool IsStickerInHand { get; set; }
    // total number of slides made in the presentation
    public int SlideCount { get; set; }
    // property for the currently active slide.
    public TextureButton Slide { get; set; }
    public StickerInHand CurrHeldSticker { get; set; }
    public CenterContainer SlideContainer { get; set; }
    public RichTextLabel SlideCountText { get; set; }
    public string LoremIpsumStickerPath {get; set; }
    public string StarStickerPath {get; set; }
    public string SquareStickerPath {get; set; }
    public string BarGraphStickerPath {get; set; }
    public string LineGraphStickerPath {get; set; }
    
    [Signal]
    public delegate void PickupStickerEventHandler(string iconFilepath);

    [Signal]
    public delegate void SavePresentationEventHandler();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        IsStickerInHand = false;

        SlideCount = 1;

        SlideContainer = GetNode<CenterContainer>("SlideContainer");
        Slide = GetNode<TextureButton>("SlideContainer/Slide");

        SlideCountText = GetNode<RichTextLabel>("NewSlideButtonContainer/Slide Count");
        SlideCountText.Text = "Slide " + SlideCount;
	}

    public void RefreshSlide()
    {
       IsStickerInHand = false; 
        GD.Print("refreshing slide in papp");
        var children = Slide.GetChildren();
        foreach (Node child in children) {
            if (child is not ColorRect cr) {
                GD.Print(child);
                child.QueueFree(); 
            }
        }

        SlideCount += 1;
        GD.Print("Slide Count = ", SlideCount);
        RichTextLabel slideCount = GetNode<RichTextLabel>("NewSlideButtonContainer/Slide Count");
        slideCount.Text = "Slide " + SlideCount;
    }

    // this is received
    public void OnPickupSticker(string iconPath)
    {
        if (IsStickerInHand) {
            GD.Print("PickupSticker recv ALREADY" + iconPath);
            return;
        } else {
            GD.Print("PickupSticker recv" + iconPath);
            // now create thing to follow mouse
            // instance of a StickerInHand
            StickerInHand nextPickup = GD.Load<PackedScene>("res://scenes/sticker_in_hand.tscn").Instantiate<StickerInHand>();
            nextPickup.iconPath = iconPath;
            nextPickup.PrepareTexture();
            Slide.AddChild(nextPickup);
            GD.Print(nextPickup);

            CurrHeldSticker = nextPickup;
            IsStickerInHand = true;
        }
        MetricsHud metr = GetNode<MetricsHud>("/root/Gameplay/HUDManager/MetricsHUD");
        metr.OnChangeSEO(6,1,-3);
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
            IsStickerInHand = false;
            Gameplay gameplay = (Gameplay)GetNode("/root/Gameplay");
            gameplay.IncrementTimeOfDay(13);
        } else {
            GD.Print("clicked on panel without Sticker in hand");
        }
    }

    public void OnNewSlideClick() 
    {
        RefreshSlide();
        Gameplay gameplay = (Gameplay)GetNode("/root/Gameplay");
        gameplay.IncrementTimeOfDay(20);
        MetricsHud metr = GetNode<MetricsHud>("/root/Gameplay/HUDManager/MetricsHUD");
        metr.OnChangeSEO(1,7,-2);
    }
    private void OnSavePresentation()
    {
        GD.Print("SavePresentation handled in PresentationApp Scene");
        Gameplay gameplay = (Gameplay)GetNode("/root/Gameplay");
        gameplay.IncrementTimeOfDay(37);
        gameplay.hudManager.metricsHud.OnChangeOptimization(15);
        gameplay.numPowerpointsCompleted++;
        this.Visible = false;
        SlideCount = 1;
        RichTextLabel slideCount = GetNode<RichTextLabel>("NewSlideButtonContainer/Slide Count");
        slideCount.Text = "Slide " + SlideCount;
    }
}
