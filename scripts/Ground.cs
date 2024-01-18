using Godot;
using System;

public partial class Ground : CharacterBody2D
{
    [Export] Bird bird;

    float yPos;

    public override void _Ready()
    {
        yPos = Position.Y;
    }

    public override void _Process(double delta)
    {
        Vector2 pos = bird.Position;

        pos.Y = yPos;
        Position = pos;

        MoveAndSlide();

        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            KinematicCollision2D collision = GetSlideCollision(i);

            if (collision.GetCollider().HasMethod("StopBird"))
            {
                collision.GetCollider().Call("StopBird");
            }
        }
    }
}