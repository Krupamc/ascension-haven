using Godot;
using System;

public partial class TotalTimeR2 : Label
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

	public override void _Process(double delta)
	{
		if (_totalTime >= 30)
		{
			GetTree().ChangeSceneToFile("res://Overworld.tscn");
		}
	}
}
