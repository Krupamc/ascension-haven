using Godot;
using System;

public partial class Difficulty : Label
{
	// Called when the node enters the scene tree for the first time.
	[Export] private Timer _timer;
	private static int _diffCount = 1;

	public static int getDiffCount()
	{
		return _diffCount;
	}
	public override void _Ready()
	{
		_timer.Timeout += OnTimerTimeout;
		Text = "Difficulty: " + _diffCount.ToString();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void OnTimerTimeout()
	{
		_diffCount++;
		Text = "Difficulty: " + _diffCount.ToString();
	}
}
