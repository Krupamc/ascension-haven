using Godot;
using System;

public partial class Background : Parallax2D
{
	// Called when the node enters the scene tree for the first time.
	[Export]
    public float ScrollSpeed {get; private set;} = 35.0f;

    public override void _Ready()
    {
        Sprite2D backgroundSprite = GetNodeOrNull<Sprite2D>("Sprite2D");

        backgroundSprite.Centered = false;
        backgroundSprite.Position = Vector2.Zero;

        IgnoreCameraScroll = true;
        ZIndex = -10;

        float backgroundHeight = backgroundSprite.Texture.GetHeight() * backgroundSprite.Scale.Y;

        RepeatSize = new Vector2(0.0f, backgroundHeight);
        Autoscroll = new Vector2(0.0f, ScrollSpeed);
    }
}
