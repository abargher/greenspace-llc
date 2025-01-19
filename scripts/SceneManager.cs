using Godot;
using System;
using Godot.Collections;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

public partial class SceneManager : Control
{
	/* External Signals */
	[Signal]
	public delegate void LoadStartEventHandler(LoadingScreen loadingScreen);  // An asset begins loading
	[Signal]
	public delegate void SceneAddedEventHandler(Node loadedScene, LoadingScreen loadingScreen);  // After asset is added to SceneTree, but before animation over
	[Signal]
	public delegate void LoadCompleteEventHandler(Node loadedScene);  // When loading has completed


	/* Internal Signals */
	[Signal]
	public delegate void _ContentFinishedLoadingEventHandler(Node incomingScene);  // When all content has finished loading
	[Signal]
	public delegate void _ContentInvalidEventHandler(string path);  // When content is invalid/cannot be found
	[Signal]
	public delegate void _ContentFailedToLoadEventHandler(string path);  // When content fails to load after starting


	/* Internal variables */
	PackedScene _loadingScreenScene = GD.Load<PackedScene>("res://scenes/loading_screen.tscn");
	LoadingScreen _loadingScreen;  // reference to loading screen instance
    // no need for zelda transition
    string _contentPath;  // path of asset the manager is trying to load
	Timer _loadProgressTimer;
	Node _loadSceneInto;  // Which node we will load the scene into
	Node _sceneToUnload;  // Which node we will unload
	bool _loadingInProgress = false;  // Blocks manager from loading two things at the same time


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// connect internal signals
		_ContentFinishedLoading += _OnContentFinishedLoading;
		_ContentFailedToLoad += _OnContentFailedToLoad;
		_ContentInvalid += _OnContentInvalid;
	}

	private void _AddLoadingScreen(string transitionType = "fade_to_black")
	{
		string transition;
		if (transitionType == "no_transition") {
			transition = "no_to_transition";
		} else {
			transition = transitionType;
		}

		_loadingScreen = _loadingScreenScene.Instantiate<LoadingScreen>();
		GetTree().Root.AddChild(_loadingScreen);
		_loadingScreen.StartTransition(transition);
	}

	public void SwapScenes(string sceneToLoad, Node loadInto = null, Node sceneToUnload = null, string transitionType = "fade_to_black")
	{
		if (_loadingInProgress) {
			GD.PushWarning("SceneManager is already loading. Cannot load another scene until current load completes.");
			return;
		}

		_loadingInProgress = true;
		if (loadInto == null) {
			loadInto = GetTree().Root;
		}

		_loadSceneInto = loadInto;
		_sceneToUnload = sceneToUnload;
		_AddLoadingScreen(transitionType);
		_LoadContent(sceneToLoad);
	}

	private void _LoadContent(string contentPath)
	{
		EmitSignal(SignalName.LoadStart, _loadingScreen);

		_contentPath = contentPath;

		Error loader = ResourceLoader.LoadThreadedRequest(contentPath);
		if (! ResourceLoader.Exists(contentPath) || loader != Error.Ok) {
			EmitSignal(SignalName._ContentInvalid);
			return;
		}

		_loadProgressTimer = new Timer();
		_loadProgressTimer.WaitTime = 0.1f;
		_loadProgressTimer.Timeout += _MonitorLoadStatus;

		GetTree().Root.AddChild(_loadProgressTimer);
		_loadProgressTimer.Start();
	}

	private void _MonitorLoadStatus()
	{
		Godot.Collections.Array loadProgress = new();
		var loadStatus = ResourceLoader.LoadThreadedGetStatus(_contentPath, loadProgress);

		if (loadStatus == ResourceLoader.ThreadLoadStatus.InvalidResource) {
			EmitSignal(SignalName._ContentInvalid, _contentPath);
			_loadProgressTimer.Stop();
		} else if (loadStatus == ResourceLoader.ThreadLoadStatus.InProgress) {
			if (_loadingScreen != null) {
				_loadingScreen.UpdateBar((float)loadProgress[0] * 100.0f);
			}
		} else if (loadStatus == ResourceLoader.ThreadLoadStatus.Failed) {
			EmitSignal(SignalName._ContentFailedToLoad, _contentPath);
			_loadProgressTimer.Stop();
			return;
		} else if (loadStatus == ResourceLoader.ThreadLoadStatus.Loaded) {
			_loadProgressTimer.Stop();
			_loadProgressTimer.QueueFree();
			PackedScene resource = (PackedScene)ResourceLoader.LoadThreadedGet(_contentPath);
			EmitSignal(SignalName._ContentFinishedLoading, resource.Instantiate());
			return;
		}
	}

	/* Internal signal handlers */
	private void _OnContentInvalid(string path)
	{
		GD.PushError($"Error: Cannot load resource: {path}");
	}
	private void _OnContentFailedToLoad(string path)
	{
		GD.PushError($"Error: Failed to load resource: {path}");
	}
	private async void _OnContentFinishedLoading(Node incomingScene)
	{
		var outgoingScene =  _sceneToUnload;
		if (outgoingScene != null) {
			if (outgoingScene.HasMethod("GetData") && incomingScene.HasMethod("ReceiveData")) {
				incomingScene.Call("ReceiveData", outgoingScene.Call("GetData"));
			}
		}

		_loadSceneInto.AddChild(incomingScene);
		EmitSignal(SignalName.SceneAdded, incomingScene, _loadingScreen);


		if (_sceneToUnload != null && _sceneToUnload != GetTree().Root) {
			_sceneToUnload.QueueFree();
		}

		if (incomingScene.HasMethod("InitScene")) {
			incomingScene.Call("InitScene");
		}

		if (_loadingScreen != null) {
			#pragma warning disable CS4014
            _loadingScreen.FinishTransition();
			#pragma warning restore CS4014
        }

		await _loadingScreen.animationPlayer.ToSignal(_loadingScreen.animationPlayer, AnimationMixer.SignalName.AnimationFinished);

		if (incomingScene.HasMethod("StartScene")) {
			incomingScene.Call("StartScene");
		}

		_loadingInProgress = false;
		EmitSignal(SignalName.LoadComplete, incomingScene);

	}

}


/* 

There are some optional methods that scenes loaded by SceneManager can implement if they want to automatically perform
actions upon scene load. These methods are:

- GetData(): This method should return data that the scene wants to pass to the incoming scene.
- ReceiveData(data): This method should accept data from the outgoing scene and use it to initialize itself.
- InitScene(): Called after the scene has been added to the SceneTree; can initialize any values for the scene.
- StartScene(): Can e.g., reenable control for the user, allow things that were unsafe to do during init, etc.

*/