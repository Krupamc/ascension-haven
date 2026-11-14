using Godot;
using System;

public partial class WallSpawnTime : Timer
{
    [Export]
    public SliderSpawner Spawner { get; set; }

    [Export]
    public double MinimumInterval { get; set; } = 0.75;

    private int _lastDifficulty = -1;

    public override void _Ready()
    {
        if (Spawner == null)
        {
            GD.PushError(
                "WallSpawnTime: Assign the SliderSpawner in the Inspector."
            );
            return;
        }

        Timeout += OnWallSpawnTimerTimeout;

        ApplyDifficulty();
        OneShot = false;
        Start();
    }

    private void OnWallSpawnTimerTimeout()
    {
        GD.Print("1: Wall timer finished.");

        if (Spawner == null)
        {
            GD.PushError("Wall timer has no Spawner assigned.");
            return;
        }

        GD.Print("2: Calling SpawnWall.");
        Callable.From(Spawner.SpawnWall).CallDeferred();
    }

    public override void _Process(double delta)
    {
        ApplyDifficulty();
    }

    private void ApplyDifficulty()
    {
        int difficulty = Math.Max(1, Difficulty.getDiffCount());

        if (difficulty == _lastDifficulty)
            return;

        _lastDifficulty = difficulty;

        double seconds = difficulty switch
        {
            1 => 7.5,
            2 => 6.0,
            3 => 4.5,
            4 => 3.75,
            5 => 3.0,
            _ => 3.0 * Math.Pow(0.9, difficulty - 5)
        };

        WaitTime = Math.Max(
            Math.Max(0.1, MinimumInterval),
            seconds
        );
    }
}