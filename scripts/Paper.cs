using Godot;
using Godot.Collections;
using System;

public partial class Paper : TextureButton
{
	Array<ColorRect> paperSquares = new Array<ColorRect>();
	Gameplay gameplay;

	[Export]
	TextureRect completeStamp;

	[Export]
	bool isPaperHome = false;

	Random random = new Random();
	// Called when the node enters the scene tree for the first time.

	public bool IsInteractable = true;

	public override void _Ready()
	{
		gameplay = GetNode<Gameplay>("/root/Gameplay");
		if (gameplay.currentDay == 11 || isPaperHome) {
			return;
		}

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

		if (gameplay.currentDay > 6) {
			int numGreenedSquares = random.Next(1, 16);
			for (int i = 0; i < numDisabledSquares; i++) {
				int randomIndex = random.Next(0, paperSquares.Count);
				SetSquareGreen(paperSquares[randomIndex]);
			}
			completeStamp.Visible = true;

			foreach (ColorRect square in paperSquares) {
				Button squareButton = square.GetNode<Button>("TextureRect/Button");
				squareButton.Disabled = true;
			}

		} else {
			completeStamp.Visible = false;
			foreach (ColorRect square in paperSquares) {
				Button squareButton = square.GetNode<Button>("TextureRect/Button");
				squareButton.Pressed += OnSquarePressed;
			}
		}
	}

    public override void _ExitTree()
    {
		if (gameplay.currentDay > 6) {
			return;
		}

		foreach (ColorRect square in paperSquares) {
			Button squareButton = square.GetNode<Button>("TextureRect/Button");
			squareButton.Pressed -= OnSquarePressed;
		}
    }


	public void SetSquareBlack(ColorRect square)
	{
		square.GetNode<TextureRect>("TextureRect").Modulate = new Color(0, 0, 0);
		square.GetNode<Button>("TextureRect/Button").Disabled = true;
	}
	
	public void SetSquareGreen(ColorRect square)
	{
		square.GetNode<TextureRect>("TextureRect").Modulate = new Color("#00aa48dc");
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
		gameplay.IncrementTimeOfDay(5);
		if (gameplay.hudManager.metricsHud.Synergy > gameplay.hudManager.metricsHud.Efficiency) {
			gameplay.hudManager.metricsHud.OnChangeEfficiency(3);
		} else {
			gameplay.hudManager.metricsHud.OnChangeSynergy(3);
		}

	}
}
