using Godot;
using System;

public partial class WallSpawnTime : Timer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		switch (Difficulty.getDiffCount())
		{
			case 1:
				WaitTime = 7.5f;
				break;
			case 2:
				WaitTime = 6f;
				break;
			case 3:
				WaitTime = 4.5f;
				break;
			case 4:
				WaitTime = 3.75f;
				break;
			case 5:
				WaitTime = 3f;
				break;
			default:
				WaitTime = Math.Pow(0.1, (Difficulty.getDiffCount() - 6.7));
				break;
		}
	}
}
