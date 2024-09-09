using Sandbox;
using Sandbox.Events;

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

	Vector3 TargetPosition { get; set; }

	public void OnGameEvent( OnStateChangedEvent eventArgs )
	{
		Log.Info( "test" );
		if (eventArgs.toState == GameStates.Playing)
		{
			CameraMode = CameraModes.FollowPlayer;
		}
		else if ( eventArgs.toState == GameStates.GameOver )
		{
			CameraMode = CameraModes.TrackObject;
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

		Log.Info( TargetPosition );
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
			TargetPosition = Target.Transform.Position + Dir * Distance;

			Transform.Rotation = Rotation.LookAt( (Target.Transform.Position - Transform.Position).Normal );
		}
	}

	void TrackObject()
	{
		Transform.Rotation = Rotation.LookAt( (PlayerRagdoll.Transform.Position - Transform.Position).Normal );
	}

	
}
