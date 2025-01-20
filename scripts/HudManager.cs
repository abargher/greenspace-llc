using Godot;
using System;

public partial class HudManager : Control
{
	public bool isHoldingDocument = false;
	public bool isDocumentStapled = false;

	public TextureRect documentFollower;

	[Export]
	public Texture2D blankDocumentTexture;
	[Export]
	public Texture2D stapledDocumentTexture;

	Vector2 documentFollowerOffset = new Vector2(65, 50);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		documentFollower = GetNode<TextureRect>("DocumentFollower");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (isHoldingDocument) {
			documentFollower.Position = GetGlobalMousePosition() - documentFollowerOffset;
		}
	}
}
