using Godot;
using System;

public partial class TotalTime : Label
{
	// Called when the node enters the scene tree for the first time.
	[Export] private Timer _timer;
	private int _totalTime = 0;
	public override void _Ready()
	{
		_timer.Timeout += OnTimerTimeout;
		Text = "Seconds: " + _totalTime.ToString();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void OnTimerTimeout()
	{
		_totalTime++;
		Text = "Seconds: " + _totalTime.ToString();
	}
	
}
