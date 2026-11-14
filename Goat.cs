using Godot;
using System;

public partial class Goat : CharacterBody2D
{
	private float speed = 900.0f;
	private float focusMultiplier = 0.3f;

	private AnimatedSprite2D _animatedSprite;
	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		float actualSpeed = speed;
		if (Input.IsKeyPressed(Key.Shift))
		{
			actualSpeed *= focusMultiplier;
		}
		Velocity = direction * actualSpeed;
		MoveAndSlide();

		_animatedSprite.Play("up");
	}

	public void Die()
	{
		GetTree().ChangeSceneToFile("res://over.tscn");
	}
}
