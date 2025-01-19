using System.Numerics;
using Godot;

public partial class StickerInHand : TextureRect
{
    public string iconPath { get; private set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        iconPath = "res://image_assets/happy-star.png";
        this.Texture = GD.Load<Texture2D>(iconPath);
        this.Texture.Set(PropertyName.Size, new Godot.Vector2(220, 220));
        GD.Print(this.Texture);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        int width = Texture.GetWidth();
        int height = Texture.GetHeight();
        Godot.Vector2 halfvec = new(width/2.0f,height/2.0f);
        Position = GetParent<Panel>().GetLocalMousePosition() - halfvec;
        this.Texture.Set(PropertyName.CustomMinimumSize, new Godot.Vector2(220, 220));
        this.Texture.Set(PropertyName.Size, new Godot.Vector2(220, 220));
	}

}
