using Godot;
using System;

public partial class PlayerScriptV2 : CharacterBody2D
{
	public const float Speed = 175.0f;
	public const float JumpVelocity = -250.0f;

	private AnimatedSprite2D aniSprite;

	private enum Animationstate { Idle, Run, Jump };
	private Animationstate _currentAnimation = Animationstate.Idle;

	public override void _Ready()
	{
		aniSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}
	private void UpdateAnimation(float direction)
	{
		if (direction != 0)
		{
			aniSprite.FlipH = (direction < 0);
		}
		if (!IsOnFloor())
		{
			PlayAnimation(Animationstate.Jump);
			return;
		}
		if (Mathf.Abs(Velocity.X) > 0.1f)
		{
			PlayAnimation(Animationstate.Run);
		}
		else
		{
			PlayAnimation(Animationstate.Idle);
		}
	}
	private void PlayAnimation(Animationstate newState)
	{
		{
			if (_currentAnimation == newState)
				return;
		}

		_currentAnimation = newState;

		aniSprite.Play(newState.ToString().ToLower());
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();

		UpdateAnimation(direction.X);
	}
}
