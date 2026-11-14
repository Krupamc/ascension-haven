using Godot;

public partial class EnemySpawner : CollisionShape2D
{
    [Export] public PackedScene EnemyScene { get; set; }
    [Export] public Timer SpawnTimer { get; set; }

    public override void _Ready()
    {
        // Connect the Timer's Timeout signal to your spawn method
        SpawnTimer.Timeout += OnSpawnTimerTimeout;
        OnSpawnTimerTimeout();
    }

    private void OnSpawnTimerTimeout()
    {
        Node2D enemy = EnemyScene.Instantiate<Node2D>();
        GetTree().CurrentScene.AddChild(enemy);
        
        // Spawn at a random X coordinate between 100 and 500
        float randomX = (float)GD.RandRange(100.0, 500.0);
        enemy.GlobalPosition = new Vector2(randomX, 0);
    }
}
