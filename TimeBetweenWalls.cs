using Godot;
using System;

public partial class TimeBetweenWalls : Label
{
	[Export] private Timer _timer;
	private static float _timeBetweenWalls; 
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_timer.Timeout += OnTimerTimeout;
		Text = "Time Between Walls: " + _timer.WaitTime.ToString();
		_timeBetweenWalls = (float)_timer.WaitTime;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void OnTimerTimeout()
	{
		Text = "Time Between Walls: " + _timer.WaitTime.ToString();
		_timeBetweenWalls = (float)_timer.WaitTime;
	}

	public static float GetTimeBetweenWalls()
	{
		return _timeBetweenWalls;
	}
}
