using Sandbox;
using Sandbox.Citizen;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Sandbox.Citizen.CitizenAnimationHelper;

public sealed class TestControllerComponent : Component
{
	Vector3 NextPos { get; set; }

	Vector3 Velocity { get; set; } = new Vector3(0.0f,0.0f,0.0f);

	bool IsGrounded;

	[Property] CitizenAnimationHelper AnimHelper { get; set; }

	CitizenAnimationHelper.SpecialMoveStyle MoveStyle { get; set; }

	bool Moving { get; set; }

	[Property] float SideMoveTime { get; set; }
	float SideMoveTargetVel { get; set; }

	float SideMoveVel { get; set; }

	int Lane = 0;

	bool Rolling = false;

	TimeSince RollGroundTime { get; set; } = 2;

	[Property] BoxCollider DeathColl { get; set; }

	protected override void OnStart()
	{
		base.OnStart();
		NextPos = Transform.Position;

		SideMoveTargetVel = 128.0f;
		Log.Info( 128.0f / SideMoveTime );

	}

	protected override void OnUpdate()
	{

		AnimHelper.WithVelocity( Vector3.Forward * 500.0f );
		AnimHelper.IsGrounded = IsGrounded;
		AnimHelper.SpecialMove = MoveStyle;
	}

	protected override void OnFixedUpdate()
	{
		base.OnFixedUpdate();

		if ( Movements.Count > 0 && !Moving )
		{
			_ = MoveToSide( Movements.First());
		}

		//Velocity = Velocity.WithY( SideMoveVel );

		if ( Input.Pressed( "Jump" ) && IsGrounded )
			Velocity = Velocity.WithZ( 12.5f );

		if ( Input.Pressed( "Left" ) )
			QueueMovement( 1 );

		if ( Input.Pressed( "Right" ) )
			QueueMovement( -1 );


		var startPos = Transform.Position + Transform.Rotation.Up * 25.0f;
		var endPos = Transform.Position - Transform.Rotation.Up * 10.0f;
		var tr = Scene.Trace.Ray( startPos, endPos ).WithoutTags("player").Run();
		Log.Info( tr.Hit );
		if ( tr.Hit )
			GroundMove( tr.HitPosition );
		else
			AirMove();

		if ( Input.Down( "Backward" ) && IsGrounded )
		{
			_ = Duck();
		}

		if ( Rolling )
		{
			DeathColl.Scale = new Vector3(16, 16, 23 );
		}
		else
		{
			DeathColl.Scale = new Vector3( 16, 16, 74 );
		}
	}

	void GroundMove(Vector3 hitPos)
	{
		Transform.Position = hitPos;
		Velocity = Velocity.WithZ( 0 );
		IsGrounded = true;
	}

	void AirMove()
	{
		Transform.Position = Transform.Position + Velocity;
		var gravMult = 1.0f;
		if ( Input.Down( "Backward" ) )
			gravMult = 4.0f;
		Velocity += Vector3.Down * gravMult;
		IsGrounded = false;
	}

	List<float> Movements = new List<float>();
	async Task MoveToSide( float to )
	{
		Moving = true;
		TimeSince timeSince = 0;

		Log.Info( to );
		Lane += (int)to;

		while ( timeSince < SideMoveTime )
		{
			Transform.Position = Transform.Position.WithY(Lane * SideMoveTargetVel);
			await Task.FixedUpdate(); // wait one frame
		}
		Moving = false;
		SideMoveVel = 0.0f;
		Movements.RemoveAt( 0 );
	}

	void QueueMovement( float dir )
	{
		Movements.Add( dir );
	}

	async Task Duck()
	{
		Rolling = true;
		if ( RollGroundTime < 1 )
			MoveStyle = CitizenAnimationHelper.SpecialMoveStyle.Roll;
		else
			MoveStyle = CitizenAnimationHelper.SpecialMoveStyle.Slide;
		await Task.DelaySeconds( 0.8f );
		Rolling = false;
		MoveStyle = CitizenAnimationHelper.SpecialMoveStyle.None;
	}
}
