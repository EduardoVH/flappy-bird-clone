using Godot;
using System;

public partial class Bird : RigidBody2D
{

	[Export] float speed = 200f;
	[Export] float jumpPower = 350f;
	[Export] float resetTimer = 1f;
	
	
	private Vector2 movementThisFrame = new Vector2();
	private Vector2 jumpImpulse = new Vector2();
	private bool hasCrashed = false;
	
	private Bird animatedSprite2D;
	
	public override void _Ready()
	{
		movementThisFrame.X = speed;
		jumpImpulse.Y = -jumpPower;
		
	}

	public override void _Process(double delta)
	{
	}
	
	
	public override void _PhysicsProcess(double delta)
	{
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("fly");
		if (hasCrashed)
		{
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("dead");
			return;
		}
		
		movementThisFrame.Y = LinearVelocity.Y;

		LinearVelocity = movementThisFrame;
	}

	public override void _Input(InputEvent @event)
	{
		if (hasCrashed) return;
		
		if (@event is InputEventScreenTouch { Pressed: true })
		{
			Jump();
		}
		else if (@event is InputEventKey { Pressed: true, Keycode: Key.Space })
		{
			Jump();
		}
	}

	private void Jump()
	{
		movementThisFrame.Y = 0f;
		LinearVelocity = movementThisFrame;
			
		ApplyImpulse(jumpImpulse);
	}

	public void StopBird()
	{
		hasCrashed = true;
		LinearVelocity = Vector2.Zero;
		GravityScale = 0f;
		LockRotation = false;
	}
}
