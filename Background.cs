using Godot;
using System;

public partial class Background : Parallax2D
{
	// Called when the node enters the scene tree for the first time.
	[Export]
    public float ScrollSpeed {get; private set;} = 150.0f;

    [Export]
    public float PlayfieldWidth { get; set; } = 720.0f;

    public override void _Ready()
    {
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

        Autoscroll = new Vector2(0.0f, ScrollSpeed);
    }

    private float ConfigureSprite(Sprite2D sprite)
    {
        sprite.Centered = false;
        sprite.Offset = Vector2.Zero;
        sprite.RegionEnabled = false;

        // Preserve the image's proportions while fitting the width.
        float scaleFactor =
                PlayfieldWidth / sprite.Texture.GetWidth();

        sprite.Scale = new Vector2(scaleFactor, scaleFactor);

        return sprite.Texture.GetHeight() * scaleFactor;
    }
}
