using Godot;
using Godot.Collections;
using System;

public partial class Paper : TextureButton
{
	Array<ColorRect> paperSquares = new Array<ColorRect>();
	Gameplay gameplay;

	Random random = new Random();
	// Called when the node enters the scene tree for the first time.

	public bool IsInteractable = true;

	public override void _Ready()
	{
		gameplay = GetNode<Gameplay>("/root/Gameplay");
		// collect all squares
		for (int i = 1; i <= 49; i++) {
			paperSquares.Add(GetNode<ColorRect>($"ColorRect/VBoxContainer/HBoxContainer/GridContainer/ButtonBorder{i}"));
		}

		// disable some squares
		int numDisabledSquares = random.Next(1, 16);
		for (int i = 0; i < numDisabledSquares; i++) {
			int randomIndex = random.Next(0, paperSquares.Count);
			SetSquareBlack(paperSquares[randomIndex]);
		}

		foreach (ColorRect square in paperSquares) {
			Button squareButton = square.GetNode<Button>("TextureRect/Button");
			squareButton.Pressed += OnSquarePressed;

		}
	}

    public override void _ExitTree()
    {
		foreach (ColorRect square in paperSquares) {
			Button squareButton = square.GetNode<Button>("TextureRect/Button");
			squareButton.Pressed -= OnSquarePressed;

		}
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

	public void SetSquareBlack(ColorRect square)
	{
		square.GetNode<TextureRect>("TextureRect").Modulate = new Color(0, 0, 0);
		square.GetNode<Button>("TextureRect/Button").Disabled = true;
	}

	private void SetIsInteractable(bool value)
	{
		IsInteractable = value;
		foreach (ColorRect square in paperSquares) {
			square.GetNode<Button>("TextureRect/Button").Disabled = value;
		}
	}

	public void OnSquarePressed() {
		// TODO: change metrics on doc button press

	}
}