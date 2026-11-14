using Godot;
using System;

public partial class Background : Parallax2D
{   

	// Called when the node enters the scene tree for the first time.
	[Export]
    private float scrollSpeedFrac {get; set;} = 200.0f;

    [Export]
    private float playfieldWidth { get; set; } = 720.0f;

    [Export(PropertyHint.Range, "0.1,10.0,0.1")]
    private float difficultyCheckInterval { get; set; } = 0.5f;

    private int _lastDifficulty = int.MinValue;

    public override void _Ready()
    {
        float scrollSpeed = scrollSpeedFrac * _lastDifficulty;
        GD.Print($"Background scroll speed: {scrollSpeed}");

        Sprite2D backgroundA = GetNodeOrNull<Sprite2D>("BackgroundA");
        Sprite2D backgroundB = GetNodeOrNull<Sprite2D>("BackgroundB");

        Scale = Vector2.One;

        float heightA = ConfigureSprite(backgroundA);
        float heightB = ConfigureSprite(backgroundB);

        // Stack the second image directly below the first.
        backgroundA.Position = Vector2.Zero;
        backgroundB.Position = new Vector2(0.0f, heightA);

        RepeatSize = new Vector2(0.0f, heightA + heightB);

        IgnoreCameraScroll = true;
        ZIndex = -10;

        // Get New Speed.
        UpdateBackgroundSpeed();

        var difficultyTimer = new Timer
        {
            Name = "BackgroundDifficultyTimer",
            WaitTime = Mathf.Max(0.1f, difficultyCheckInterval),
            OneShot = false
        };

        difficultyTimer.Timeout += UpdateBackgroundSpeed;

        AddChild(difficultyTimer);
        difficultyTimer.Start();

        Autoscroll = new Vector2(0.0f, scrollSpeed);
    }

     private void UpdateBackgroundSpeed()
    {
        int difficulty = Difficulty.getDiffCount();

        if (difficulty == _lastDifficulty)
        {
            return;
        }

        _lastDifficulty = difficulty;

        float scrollSpeed = scrollSpeedFrac * difficulty;
        Autoscroll = new Vector2(0.0f, scrollSpeed);

        GD.Print(
            $"Difficulty: {difficulty} | " +
            $"Background scroll speed: {scrollSpeed}"
        );
    }
    private float ConfigureSprite(Sprite2D sprite)
    {
        sprite.Centered = false;
        sprite.Offset = Vector2.Zero;
        sprite.RegionEnabled = false;

        // Preserve the image's proportions while fitting the width.
        float scaleFactor = playfieldWidth / sprite.Texture.GetWidth();

        sprite.Scale = new Vector2(scaleFactor, scaleFactor);

        return sprite.Texture.GetHeight() * scaleFactor;
    }
}
