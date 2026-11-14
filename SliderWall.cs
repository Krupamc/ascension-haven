using Godot;

public partial class SliderWall : Area2D
{
    [Signal]
    public delegate void PlayerHitEventHandler(Node2D player);
    private Rect2 _firstBlock;
    private Rect2 _secondBlock;
    private Vector2 _velocity;
    private Vector2 _playfieldSize;
    private Vector2 _wallSize;
    private CharacterBody2D _player;
    private bool _configured;
    private bool _hitReported;
    private const float CleanupMargin = 180.0f;
    private readonly Color _fill = new Color("dc744b");

    public void Configure(
        Vector2 playfieldSize,
        bool horizontal,
        float gapStart,
        float gapSize,
        float thickness,
        Vector2 startPosition,
        Vector2 velocity,
        CharacterBody2D player)
    {
        _playfieldSize = playfieldSize;
        _velocity = velocity;
        _player = player;
        Position = startPosition;
        ZIndex = 1;

        CollisionLayer = 0;
        CollisionMask = player.CollisionLayer;
        Monitoring = true;
        Monitorable = false;

        float gapEnd = gapStart + gapSize;
        if (horizontal)
        {
            _wallSize = new Vector2(playfieldSize.X, thickness);
            _firstBlock = new Rect2(0, 0, gapStart, thickness);
            _secondBlock = new Rect2(
                gapEnd, 0, playfieldSize.X - gapEnd, thickness);
        }
        else
        {
            _wallSize = new Vector2(thickness, playfieldSize.Y);
            _firstBlock = new Rect2(0, 0, thickness, gapStart);
            _secondBlock = new Rect2(
                0, gapEnd, thickness, playfieldSize.Y - gapEnd);
        }

        AddBlockShape("FirstBlock", _firstBlock);
        AddBlockShape("SecondBlock", _secondBlock);
        _configured = true;
    }

    public override void _Ready()
    {
        if (!_configured)
        {
            GD.PushError("Create SliderWall through SliderSpawner.");
            QueueFree();
            return;
        }

        BodyEntered += OnBodyEntered;
        QueueRedraw();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!_configured)
            return;

        Position += _velocity * (float)delta;

        if (Position.X > _playfieldSize.X + CleanupMargin ||
            Position.Y > _playfieldSize.Y + CleanupMargin ||
            Position.X + _wallSize.X < -CleanupMargin ||
            Position.Y + _wallSize.Y < -CleanupMargin)
        {
            QueueFree();
        }
    }

    public override void _Draw()
    {
        if (!_configured)
            return;

        DrawRect(_firstBlock, _fill);
        DrawRect(_secondBlock, _fill);
        DrawRect(_firstBlock, Colors.White, false, 2.0f);
        DrawRect(_secondBlock, Colors.White, false, 2.0f);
    }

    private void AddBlockShape(string nodeName, Rect2 block)
    {
        var collision = new CollisionShape2D
        {
            Name = nodeName,
            Position = block.Position + block.Size * 0.5f,
            Shape = new RectangleShape2D { Size = block.Size }
        };
        AddChild(collision);
    }

    private void OnBodyEntered(Node2D body)
    {
        if (_hitReported || !GodotObject.IsInstanceValid(_player) ||
            body != _player)
        {
            return;
        }

        _hitReported = true;
        EmitSignal(SignalName.PlayerHit, body);
    }
}
