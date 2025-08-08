using Godot;
using System;

public partial class Script_TheCameraRig : Node3D
{
	[Export]
	public Node3D yaw;
	[Export]
	Node3D pitch;
	[Export]
	Node3D zoom;
	[Export]
	Node3D roll;

	[Export]
	Node3D player;

	float cameraYaw = 0.0f;
	float cameraPitch = 0.0f;
	float cameraZoom = 2.0f;
	float cameraRoll = 0.0f;

	float zoomDist = 2.0f;

	float startTimer;
	Vector3 rayFront;
	Vector3 rayBack;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		startTimer = 2.0f;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		// Start of game anim
		if (startTimer > 0.0f)
		{
			startTimer -= (float)delta;
			pitch.RotateX((float)delta * -0.2f);
			cameraPitch -= Mathf.RadToDeg((float)delta * 0.2f);
			cameraZoom = zoom.Position.Z + (float)delta * (10 * 0.2f);
			zoom.Position = new Vector3(0.0f, 0.0f, cameraZoom);
			zoomDist = cameraZoom;
		}
		else
		{
			// Camera LEFT/RIGHT
			this.GlobalPosition = player.GlobalPosition;
			if (Input.IsActionPressed("C_Left"))
			{
				yaw.RotateY((float)delta * -1);
			}
			if (Input.IsActionPressed("C_Right"))
			{
				yaw.RotateY((float)delta * 1);
			}

			// Camera UP/DOWN
			if (Input.IsActionPressed("C_Down"))
			{
				if (cameraPitch > (-85.0f))
				{
					CameraOut(delta, 1.0f);
				}
			}
			if (Input.IsActionPressed("C_Up"))
			{
				if (cameraPitch < -5.0f)
				{
					CameraIn(delta, 1.0f);
				}
			}
			// Check for obstacles
			Vector3 playerPos = new Vector3(player.GlobalPosition.X, 0.5f, player.GlobalPosition.Z);
			rayFront = RayCast(playerPos, zoom.GlobalPosition);

			// If there is an obstacle
			if (rayFront != Vector3.Inf)
			{
				float temp = playerPos.DistanceTo(rayFront);
				zoomDist = Mathf.Lerp(zoomDist, temp, (float)delta * 5);
				//zoom.Position = zoom.Position.Lerp(new Vector3(zoom.Position.X, zoom.Position.Y, zoomDist), (float)delta * 5);
			}
			// If there isn't an obstacle
			if (rayFront == Vector3.Inf)
			{
				Vector3 backTarget = zoom.GlobalPosition + (cameraZoom - zoom.Position.Z) * zoom.Basis.Z;
				rayBack = RayCast(zoom.GlobalPosition, backTarget);

				if (rayBack != Vector3.Inf)
				{
					float temp = playerPos.DistanceTo(rayBack);
					zoomDist = Mathf.Lerp(zoomDist, temp, (float)delta * 2);
					//zoom.Position = zoom.Position.Lerp(new Vector3(zoom.Position.X, zoom.Position.Y, zoomDist - 0.5f), (float)delta * 2);
				}
				else
				{
					zoomDist = Mathf.Lerp(zoomDist, cameraZoom, (float)delta * 5);
					//zoom.Position = zoom.Position.Lerp(new Vector3(zoom.Position.X, zoom.Position.Y, cameraZoom), (float)delta * 2);
				}
			}

			// Zoom camera
			zoom.Position = new Vector3(zoom.Position.X, zoom.Position.Y, zoomDist);
		}

	}

	public void CameraOut(double delta, float speed)
	{
		pitch.RotateX((float)delta * -speed);
		cameraPitch -= Mathf.RadToDeg((float)delta * speed);
		cameraZoom = cameraZoom + (float)delta * (10 * speed);
		//zoom.Position = new Vector3(0.0f, 0.0f, cameraZoom);
	}

	public void CameraIn(double delta, float speed)
	{
		pitch.RotateX((float)delta * speed);
		cameraPitch += Mathf.RadToDeg((float)delta * speed);
		cameraZoom = cameraZoom - (float)delta * (10 * speed);
		//zoom.Position = new Vector3(0.0f, 0.0f, cameraZoom);
	}

	public Vector3 RayCast(Vector3 origin, Vector3 target, uint bitmask = 2)
	{
		var spaceState = GetWorld3D().DirectSpaceState;
		var query = PhysicsRayQueryParameters3D.Create(origin, target, bitmask, new Godot.Collections.Array<Rid> { });
		var result = spaceState.IntersectRay(query);

		if (result.Count == 0)
		{
			return Vector3.Inf;
		}
		return (Vector3)result["position"];
	}
}
