using Godot;
using System;

public partial class Script_TheInGameUi : Control
{
	[Export]
	Script_ThePlayer player;
	[Export]
	ProgressBar health;
	[Export]
	ProgressBar stamina;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		health.Value = player.health;
		stamina.Value = player.stamina;
	}
}
