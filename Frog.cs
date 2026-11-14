using Godot;
using System;

public partial class Frog : CharacterBody2D
{
	private float speed = 300.0f;
	private float focusMultiplier = 0.39f;

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


		if (direction.Y < 0)
		{
			_animatedSprite.Play("up");
		}
		else if (direction.Y > 0)
		{
			_animatedSprite.Play("down");
		}
		else if (direction.X > 0)
		{
			_animatedSprite.Play("right");
		}
		else if (direction.X < 0)
		{
			_animatedSprite.Play("left");
		}
		else if (direction.Y < 0 && direction.X < 0)
		{
			_animatedSprite.Play("up_left");
		}
		else if (direction.Y < 0 && direction.X > 0)
		{
			_animatedSprite.Play("up_right");
		}
		else if (direction.Y > 0 && direction.X < 0)
		{
			_animatedSprite.Play("down_left");
		}
		else if (direction.Y > 0 && direction.X > 0)
		{
			_animatedSprite.Play("down_right");
		}
		else
		{
			_animatedSprite.Play("idle");
		}
	}
}
