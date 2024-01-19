using Godot;
using System;

public partial class Bird : RigidBody2D
{

	[Export] float speed = 200f;
	[Export] float jumpPower = 350f;
	[Export] float resetTimer = 1f;
	[Export] AudioStreamWav jumpSfx;
	[Export] AudioStreamWav scoreSfx;
	[Export] AudioStreamWav dieSfx;
	
	public bool HasCrashed { get; private set; } = false;
	
	private Vector2 movementThisFrame = new Vector2();
	private Vector2 jumpImpulse = new Vector2();
	private float timeSinceDeath = -0.5f;
	private AudioStreamPlayer jumpPlayer;
	private AudioStreamPlayer otherPlayer;
	
	private Bird animatedSprite2D;
	
	public override void _Ready()
	{
		movementThisFrame.X = speed;
		jumpImpulse.Y = -jumpPower;

		jumpPlayer = GetNode<AudioStreamPlayer>("JumpSounds");
		otherPlayer = GetNode<AudioStreamPlayer>("OtherSounds");
	}

	public override void _Process(double delta)
	{
		if (!HasCrashed) return;

		timeSinceDeath += (float)delta;

		if (timeSinceDeath > resetTimer)
		{
			GameManager.Instance.Reload();
		}
	}
	
	
	public override void _PhysicsProcess(double delta)
	{
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("fly");
		if (HasCrashed)
		{
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("dead");
			return;
		}
		
		movementThisFrame.Y = LinearVelocity.Y;

		LinearVelocity = movementThisFrame;
	}

	public override void _Input(InputEvent @event)
	{
		if (HasCrashed) return;
		
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

		jumpPlayer.Stream = jumpSfx;
		jumpPlayer.Play();
		
		ApplyImpulse(jumpImpulse);
	}

	public void StopBird()
	{
		if (HasCrashed) return;
		
		HasCrashed = true;
		LinearVelocity = Vector2.Zero;
		GravityScale = 0f;
		LockRotation = false;
		
		otherPlayer.Stream = dieSfx;
		otherPlayer.Play();
	}

	public void OnBodyEntered(Node body)
	{
		if (HasCrashed) return;

		otherPlayer.Stream = dieSfx;
		otherPlayer.Play();
		
		HasCrashed = true;
	}

	public void PointScoredSfx()
	{
		otherPlayer.Stream = scoreSfx;
		otherPlayer.Play();
	}
}
