using Godot;
using System;

public partial class Script_ThePlayer : CharacterBody3D
{
	[Export]
	Node3D pivot;
	[Export]
	Node3D collision;
	[Export]
	Node3D camera;

	public float speed = 10.0f;
	public float jumpVelocity = 30.5f;
	public float health = 75.0f;
	public float stamina = 90.0f;

	private Vector3 targetVelocity = Vector3.Zero;
	private int fallAcceleration = 75;

	float startTimer;

	public override void _Ready()
	{
		startTimer = 2.0f;
	}


	public override void _PhysicsProcess(double delta)
	{
		// Start of game anim
		if (startTimer > 0.0f)
		{
			startTimer -= (float)delta;
		}
		else
		{
			//TEMP Change health & stamina
			if (Input.IsActionJustPressed("h_down"))
			{
				health--;
			}
			if (Input.IsActionJustPressed("h_up"))
			{
				health++;
			}
			if (Input.IsActionJustPressed("s_down"))
			{
				stamina--;
			}
			if (Input.IsActionJustPressed("s_up"))
			{
				stamina++;
			}
			
			// Walking
			Vector3 direction = Vector3.Zero;
			if (Input.IsActionPressed("Forward"))
			{
				direction -= ((Script_TheCameraRig)camera).yaw.Basis.Z;
			}
			if (Input.IsActionPressed("Back"))
			{
				direction += ((Script_TheCameraRig)camera).yaw.Basis.Z;
			}
			if (Input.IsActionPressed("Left"))
			{
				direction -= ((Script_TheCameraRig)camera).yaw.Basis.X;
			}
			if (Input.IsActionPressed("Right"))
			{
				direction += ((Script_TheCameraRig)camera).yaw.Basis.X;
			}

			if (direction != Vector3.Zero)
			{
				direction = direction.Normalized();
			}


			float holderY = targetVelocity.Y;
			if (direction != Vector3.Zero)
			{
				targetVelocity = targetVelocity.Lerp(direction * speed, (float)delta * 2);
			}
			else
			{
				targetVelocity = targetVelocity.Lerp(direction * speed, (float)delta * 8);
			}
			targetVelocity.Y = holderY;

			//Set pivot rotation to match targetVelocity
			Vector3 looking = targetVelocity.Normalized();
			looking.Y = 0.0f;
			if (looking != Vector3.Up && looking != Vector3.Down && looking != Vector3.Zero)
			{
				pivot.Basis = Basis.LookingAt(looking);
			}


			// Add the gravity
			if (!IsOnFloor())
			{
				targetVelocity.Y -= fallAcceleration * (float)delta;
			}
			else
			{
				targetVelocity.Y = 0.0f;
			}

			// Jumping
			if (Input.IsActionJustPressed("Jump") && IsOnFloor())
			{
				//GD.Print("Jump");
				targetVelocity.Y = jumpVelocity;
			}

			this.Velocity = targetVelocity;
			MoveAndSlide();
		}
	}
}
