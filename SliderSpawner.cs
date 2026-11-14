using Godot;

public partial class SliderSpawner : Node2D
{
    [Signal]
    public delegate void PlayerHitEventHandler(Node2D player);
    [Export] public Timer SpawnTimer { get; set; }
    [Export] public CharacterBody2D Player { get; set; }
    [Export] public float WallSpeed { get; set; } = 180.0f;
    [Export] public float GapSize { get; set; } = 200.0f;
    [Export] public float Thickness { get; set; } = 64.0f;
    [Export] public int MaxActiveWalls { get; set; } = 4;
    [Export] public bool SpawnOnStart { get; set; } = true;

    private readonly RandomNumberGenerator _random = new();
    private Node2D _activeWalls;
    private bool _connected;
    private const float MinimumBlockLength = 40.0f;
    private const float SpawnPadding = 60.0f;

    public override void _Ready()
    {
        if (SpawnTimer == null || Player == null)
        {
            GD.PushError("SliderSpawner: assign Spawn Timer and Player.");
            return;
        }
        if (WallSpeed <= 0 || GapSize <= 0 || Thickness <= 0 ||
            MaxActiveWalls < 1)
        {
            GD.PushError("SliderSpawner: speed, gap, thickness and count must be positive.");
            return;
        }
        if (Player.GetViewport() != GetViewport())
        {
            GD.PushError("SliderSpawner and Player must be in the same SubViewport.");
            return;
        }
        Vector2 size = GetViewport().GetVisibleRect().Size;
        if (Mathf.Min(size.X, size.Y) <= MinimumBlockLength * 2 + 1)
        {
            GD.PushError("SliderSpawner: gameplay viewport is too small.");
            return;
        }

        _random.Randomize();
        _activeWalls = new Node2D { Name = "ActiveSliderWalls" };
        AddChild(_activeWalls);
    }

    private void StartSpawning()
    {
        if (!IsInsideTree() || !GodotObject.IsInstanceValid(SpawnTimer))
            return;

        SpawnTimer.OneShot = false;
        SpawnTimer.Start();
        if (SpawnOnStart)
            SpawnWall();
    }

    private void OnSpawnTimeout()
    {
        // Also safe if the timer is later set to physics processing.
        Callable.From(SpawnWall).CallDeferred();
    }

    public void SpawnWall()
    {
        GD.Print("3: SpawnWall was reached.");

        if (!IsInsideTree())
        {
            GD.PushError("Spawner is not inside the running scene.");
            return;
        }

        if (!GodotObject.IsInstanceValid(Player))
        {
            GD.PushError("Player is missing. Assign Frog to the spawner's Player field.");
            return;
        }

        if (!GodotObject.IsInstanceValid(_activeWalls))
        {
            GD.PushError("ActiveSliderWalls was not created in _Ready().");
            return;
        }

        if (_activeWalls.GetChildCount() >= MaxActiveWalls)
        {
            GD.Print(
                $"Spawn blocked: {_activeWalls.GetChildCount()} active walls, " +
                $"limit is {MaxActiveWalls}."
            );
            return;
        }
        
        if (!IsInsideTree() || !GodotObject.IsInstanceValid(Player) ||
            _activeWalls.GetChildCount() >= MaxActiveWalls)
        {
            return;
        }

        Vector2 size = GetViewport().GetVisibleRect().Size;
        int edge = _random.RandiRange(0, 3);
        bool horizontal = edge < 2;
        float span = horizontal ? size.X : size.Y;
        float actualGap = Mathf.Min(GapSize, span - MinimumBlockLength * 2);
        if (actualGap <= 0)
            return;

        float gapStart = _random.RandfRange(
            MinimumBlockLength, span - MinimumBlockLength - actualGap);

        Vector2 start;
        Vector2 direction;
        string edgeName;
        switch (edge)
        {
            case 0:
                start = new Vector2(0, -Thickness - SpawnPadding);
                direction = Vector2.Down;
                edgeName = "top";
                break;
            case 1:
                start = new Vector2(0, size.Y + SpawnPadding);
                direction = Vector2.Up;
                edgeName = "bottom";
                break;
            case 2:
                start = new Vector2(-Thickness - SpawnPadding, 0);
                direction = Vector2.Right;
                edgeName = "left";
                break;
            default:
                start = new Vector2(size.X + SpawnPadding, 0);
                direction = Vector2.Left;
                edgeName = "right";
                break;
        }

        var wall = new SliderWall();
        wall.Configure(size, horizontal, gapStart, actualGap, Thickness,
            start, direction * WallSpeed, Player);
        wall.PlayerHit += OnPlayerHit;
        _activeWalls.AddChild(wall);
        GD.Print($"Slider from {edgeName}; gap {gapStart:0}-{gapStart + actualGap:0}");
    }

    private void OnPlayerHit(Node2D player)
    {
        GD.Print($"{player.Name} was hit by a slider.");
        EmitSignal(SignalName.PlayerHit, player);
    }

    public override void _ExitTree()
    {
        if (_connected && GodotObject.IsInstanceValid(SpawnTimer))
            SpawnTimer.Timeout -= OnSpawnTimeout;
    }
}
