using Godot;
using Godot.Collections;
using System;

public partial class House : TextureRect
{
	[Export]
	public Array<Texture2D> houseImages;

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
	Random random = new Random();


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		int currentDay = 2;  // FOR TESTING ONLY - delete once below is satisfied
		// TODO: uncomment this when we have everything connected and this won't run without the Gameplay scene
		// Gameplay gameplay = GetNode<Gameplay>("/root/Gameplay");
		// currentDay = gameplay.currentDay;

		if (currentDay == 10)
		{
			// half of the houses are solar panels
			int choice = random.Next(0, 2);
			if (choice == 1) {
				this.Texture = solarPanelImage;
			} else {
				this.Texture = houseImages[random.Next(0, houseImages.Count)];
			}
		} else if (currentDay == 11) {
			this.Texture = solarPanelImage;
		} else {
			this.Texture = houseImages[random.Next(0, houseImages.Count)];
		}
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
