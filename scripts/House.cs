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

	public Vector2 startPos;
	public Vector2 endPos;
	public bool isLeft = false;

	float roadLength;
	float speed;
	float slope;
	float scaleSpeed = (1.0f - 0.05f )/ 5f;
	bool ready = false;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	public void SetupHouse()
	{
		slope = calcSlope(startPos, endPos);
		roadLength = distance(startPos, endPos);
		speed = roadLength / 5f;
		GD.Print("House ready");
		this.Scale = new Vector2(0.05f, 0.05f);
		this.FlipH = isLeft;
		GD.Print(startPos);
		GD.Print(endPos);
		GD.Print(slope);
		GD.Print(roadLength);
		ready = true;
	}

	private float calcSlope(Vector2 a, Vector2 b)
	{
		return (b.Y - a.Y) / (b.X - a.X);
	}
	private float distance(Vector2 a, Vector2 b)
	{
		return (float)Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!ready) {
			return;
		}

		Vector2 baseVec;
		if (isLeft) {
			baseVec = new Vector2(-1, -slope);
		} else {
			// Right side houses, THIS IS CORRECT
			baseVec = new Vector2(1, slope);
		}
		this.Position += baseVec * speed * (float)delta;
		this.Scale += new Vector2(scaleSpeed, scaleSpeed) * (float)delta;
	}

	public void OnExitScreen()
	{
		QueueFree();
	}
}
