using Godot;
using System;

public class Startup : Node2D
{

	public override void _Ready()
	{
		var player = GetNode<PlayerNode>("Player");
		var overlay = GetNode<GUI>("Overlay");

		overlay.SetPlayer(player);
	}
	
}
