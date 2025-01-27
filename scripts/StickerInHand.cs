using System.Numerics;
using Godot;

public partial class StickerInHand : TextureRect
{
    [Export]
    public string iconPath { get; set; }
    public bool hasBeenPlaced { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        // iconPath = "res://assets/images/sprites/powerpoint/bargraphup.png";
        PrepareTexture();
	}

    public void PrepareTexture()
    {
        if (iconPath == null) {
            GD.Print("iconPath is null");
            return;
        }
        this.Texture = GD.Load<Texture2D>(iconPath);
        this.Texture.Set(PropertyName.Size, new Godot.Vector2(220, 220));
        GD.Print("NEW STICKER IN HAND CREATED");

        // when a sticker is created, it starts in your hand.
        hasBeenPlaced = false;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (hasBeenPlaced) {
            return;
        } else {
            int width = this.Texture.GetWidth();
            int height = this.Texture.GetHeight();
            Godot.Vector2 halfvec = new(width/2.0f,height/2.0f);
            Position = GetParent<TextureButton>().GetLocalMousePosition() - halfvec;
            this.Texture.Set(PropertyName.CustomMinimumSize, new Godot.Vector2(220, 220));
            this.Texture.Set(PropertyName.Size, new Godot.Vector2(220, 220));
        }
	}

}
