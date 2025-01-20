using Godot;
using System;

public partial class House : TextureRect
{
	/* 
	Slope of motion is (2600 - 1970) / (60 + 650) = 0.88733f
	*/
	[Export]
	public Godot.Collections.Array<Texture2D> houseImages;

	[Export]
	public Texture2D solarPanelImage;

	[Export]
	public Vector2 startPos = new(1440, -120);
	[Export]
	public Vector2 endPos = new(2600, 60);

	float roadLength;
	float speed;
	float slope;
	float scaleSpeed = (1.0f - 0.05f )/ 5f;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		slope = calcSlope(endPos, startPos);
		roadLength = distance(startPos, endPos);
		speed = roadLength / 5f;
		GD.Print("House ready");
		this.Scale = new Vector2(0.05f, 0.05f);
		this.Position = startPos;
	}

	static private float calcSlope(Vector2 a, Vector2 b)
	{
		return (b.Y - a.Y) / (b.X - a.X);
	}
	static private float distance(Vector2 a, Vector2 b)
	{
		return (float)Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		this.Position += new Vector2(1, slope) * speed * (float)delta;
		this.Scale += new Vector2(scaleSpeed, scaleSpeed) * (float)delta;
	}
}
