using Godot;
using System;
using System.Threading.Tasks;

public partial class LoadingScreen : CanvasLayer
{
	[Signal]
	public delegate void TransitionInCompleteEventHandler();  // When transition has completed

	ProgressBar progressBar;
	AnimationPlayer animationPlayer;
	Timer timer;
	String startingAnimationName;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Grab references to child nodes we need later
		progressBar = GetNode<ProgressBar>("ProgressBar");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		timer = GetNode<Timer>("Timer");
		progressBar.Visible = false;
	}

	public void StartTransition(String animationName)
	{
		if (!animationPlayer.HasAnimation(animationName)) {
			GD.PushWarning(animationName + " is not a valid animation name.");
			animationName = "fade_to_black";
		}
		startingAnimationName = animationName;
		animationPlayer.Play(animationName);
	
	 	// if timer expires before finishing loading, show the progress bar.
		timer.Start();
	}

	public async Task FinishTransition()
	{
		if (timer != null) {
			timer.Stop();
		}
		String endingAnimationName = startingAnimationName.Replace("to", "from");

		if (!animationPlayer.HasAnimation(endingAnimationName)) {
			GD.PushWarning(endingAnimationName + " is not a valid animation name.");
			endingAnimationName = "fade_from_black";
		}
		animationPlayer.Play(endingAnimationName);

        _ = await ToSignal(animationPlayer, AnimationMixer.SignalName.AnimationFinished);
		QueueFree();
	}

	public void ReportMidpoint()
	{
		EmitSignal(SignalName.TransitionInComplete);
	}

	public void OnTimerTimeout()
	{
		progressBar.Visible = true;
	}

	public void UpdateBar(float val)
	{
		progressBar.Value = val;
	}

}
