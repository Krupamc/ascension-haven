    using Godot;

public partial class Walls : Node2D
{
    [Export] public PackedScene SceneToSpawn { get; set; }
    
    public float WallInterval { get; private set; }
    public override void _Ready()
    {

        WallInterval =
            TimeBetweenWalls.GetTimeBetweenWalls();
        GD.Print($"Time between walls: {WallInterval} seconds");
    }

    public void SpawnObject(Vector2 spawnPosition)
    {
        if (SceneToSpawn == null) return;

        Node2D instance = SceneToSpawn.Instantiate<Node2D>();
        AddChild(instance);

        instance.GlobalPosition = spawnPosition;
        
    }
}
