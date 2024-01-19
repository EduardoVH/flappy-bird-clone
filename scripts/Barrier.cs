using Godot;
using System;

public partial class Barrier : Node2D
{

	[Export] private NinePatchRect bottomBarrierSprite;
	[Export] private float groundHeight;

	private bool hasSetSprite = false;
	
	
	
	public override void _Process(double delta)
	{

		if (hasSetSprite) return;

		if (bottomBarrierSprite != null)
		{
			hasSetSprite = true;
			Vector2 newSize = bottomBarrierSprite.Size;
			newSize.Y = groundHeight - bottomBarrierSprite.GlobalPosition.Y;
			bottomBarrierSprite.Size = newSize;
		}

	}
}
