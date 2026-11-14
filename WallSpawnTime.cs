using Godot;
using System;

public partial class WallSpawnTime : Timer
{
	private PackedScene _wallScene = GD.Load<PackedScene>("res://wall.tscn");
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	public void OnTimeout()
	{
		SpawnWall();
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
				WaitTime = Math.Pow(0.9, (Difficulty.getDiffCount() - 3.75));
				break;
		}
	}

	private void SpawnWall()
    {
        // 2. Instantiate the packed scene into a usable node
        Node2D newWall = (Node2D)_wallScene.Instantiate();

        // 3. Set its position (optional)
        newWall.Position = new Vector2(400, 0);

        // 4. Add it as a child to the current scene tree so it appears in the game
        AddChild(newWall);																						
    }
}
