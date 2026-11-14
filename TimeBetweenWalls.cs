using Godot;
using System;

public partial class TimeBetweenWalls : Label
{
	[Export] private Timer _timer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_timer.Timeout += OnTimerTimeout;
		Text = "Time Between Walls: " + _timer.WaitTime.ToString();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void OnTimerTimeout()
	{
		Text = "Time Between Walls: " + _timer.WaitTime.ToString();
	}

	public double GetTimeBetweenWalls()
	{
		return _timer.WaitTime;
	}
}
