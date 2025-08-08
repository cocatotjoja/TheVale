using Godot;
using System;

public partial class Script_GameManager : Node3D
{
	private Node levelInstance;
	private PackedScene levelScene = GD.Load<PackedScene>("res://TheVale/Levels/Level_TheVale.tscn");


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		levelInstance = (Script_TheVale)levelScene.Instantiate();
		AddChild(levelInstance);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
