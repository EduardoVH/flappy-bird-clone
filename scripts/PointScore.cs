using Godot;
using System;

public partial class PointScore : Area2D
{

    public void OnBodyExit(Node2D body)
    {
        if (body.GetClass() != "RigidBody2D") return;

        Bird bird = (Bird)body;
        
        if (bird != null && !bird.HasCrashed)
        {
            GameManager.Instance.AddPoint();
        }
    }
    
}
