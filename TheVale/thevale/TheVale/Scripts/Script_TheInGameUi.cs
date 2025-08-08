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

	[Export]
	ColorRect inv1;
	[Export]
	ColorRect inv2;
	[Export]
	ColorRect inv3;
	[Export]
	ColorRect inv4;

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

	public void ChangeInvSlot(int slot, Color item)
	{
		if (slot == 0)
		{
			inv1.Color = item;
		}
		if (slot == 1)
		{
			inv2.Color = item;
		}
		if (slot == 2)
		{
			inv3.Color = item;
		}
		if (slot == 3)
		{
			inv4.Color = item;
		}
	}
	
}
