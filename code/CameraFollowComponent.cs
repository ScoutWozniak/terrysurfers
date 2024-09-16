using Sandbox;
using Sandbox.Events;
using System;

public sealed class CameraFollowComponent : Component, IGameEventHandler<OnStateChangedEvent>, IGameEventHandler<OnLevelShift>
{

	public enum CameraModes
	{
		FollowPlayer,
		GoToObject,
		TrackObject,
	}

	[Property] public GameObject Target { get; set; }
	[Property] public Vector3 Dir { get; set; } = new Vector3(0, 1, 1 );
	[Property] public float Distance { get; set; } = 500.0f;

	[Property] GameObject InitialCamPos { get; set; }

	[Property] GameObject PlayerRagdoll { get; set; }

	[Property] CameraModes CameraMode { get; set; } = CameraModes.GoToObject;

	public Vector3 Offset { get; set; }

	Vector3 TargetPosition { get; set; }

	float TargetFOV = 55.0f;

	[RequireComponent] CameraComponent Cam { get; set; }

	public void OnGameEvent( OnStateChangedEvent eventArgs )
	{
		if (eventArgs.toState == GameStates.Playing)
		{
			CameraMode = CameraModes.FollowPlayer;
		}
		else if ( eventArgs.toState == GameStates.GameOver )
		{
			CameraMode = CameraModes.TrackObject;
			TargetFOV = 40.0f;
		}
	}

	public void OnGameEvent( OnLevelShift eventArgs )
	{
		TargetPosition -= new Vector3( eventArgs.ammount, 0, 0 );
		Transform.Position -= new Vector3( eventArgs.ammount, 0, 0 );

		Transform.Rotation = Rotation.LookAt( (Target.Transform.Position - Transform.Position).Normal );
		Transform.ClearInterpolation();
	}

	protected override void OnUpdate()
	{
		if (CameraMode == CameraModes.FollowPlayer )
			FollowPlayer();
		else if ( CameraMode == CameraModes.GoToObject )
			GoToObject();
		else
			TrackObject();

		Transform.Position = Transform.Position.LerpTo( TargetPosition, Time.Delta * 10.0f );
		Cam.FieldOfView = Cam.FieldOfView.LerpTo( TargetFOV, Time.Delta * 10.0f );
	}

	void GoToObject()
	{
		TargetPosition = Transform.Position;
		Transform.Position = InitialCamPos.Transform.Position;
		Transform.Rotation = InitialCamPos.Transform.Rotation;
	}

	void FollowPlayer()
	{
		if ( Target != null )
		{
			var startPos = Target.Transform.Position;
			var endPos = startPos + Dir * Distance;

			var tr = Scene.Trace.Ray( startPos, endPos ).WithoutTags( "obstacle" ).Run();
			if ( tr.Hit )
				TargetPosition = tr.HitPosition + Offset;
			else
				TargetPosition = endPos + Offset;

			Transform.Rotation = Rotation.LookAt( (Target.Transform.Position - Transform.Position).Normal );
		}
	}

	void TrackObject()
	{
		Transform.Position = TargetPosition + Offset;
		Transform.Rotation = Rotation.LookAt( (PlayerRagdoll.Transform.Position - Transform.Position).Normal );
	}

	
}
