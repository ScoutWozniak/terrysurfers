using Sandbox;
using Sandbox.Citizen;
using Sandbox.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public sealed class PlayerControllerComponent : Component
{
	[Property] public float JumpHeight { get; set; } = 256;

	[Property] float JumpTimeToPeak { get; set; }

	[Property] float JumpTimeToDescent { get; set; }
	[Property] CitizenAnimationHelper AnimHelper { get; set; }

	[Property] LevelComponent Level { get; set; }

	[Property] public BoxCollider DeathColl { get; set; }

	[Property] ScoreComp Score { get; set; }

	bool Rolling = false;
	bool Moving = false;

	float DuckValue = 0.0f;

	float JumpVelocity { get; set; }
	float JumpGravity { get; set; }
	float FallGravity { get; set; }

	[Property] float SideMoveTime { get; set; }
	float SideMoveTargetVel { get; set; }

	float SideMoveVel { get; set; }


	CitizenAnimationHelper.SpecialMoveStyle MoveStyle { get; set; }

	TimeSince RollGroundTime { get; set; } = 2;

	[Property] ParticleConeEmitter RunPartcles { get; set; }

	float MovementMult = 0.0f;

	protected override void OnEnabled()
	{
		JumpVelocity = (2.0f * JumpHeight) / JumpTimeToPeak;
		JumpGravity = (-2.0f * JumpHeight) / (JumpTimeToPeak * JumpTimeToPeak);
		FallGravity = (-2.0f * JumpHeight) / (JumpTimeToDescent * JumpTimeToDescent);

		SideMoveTargetVel = (122.0f) / SideMoveTime;
	}

	protected override void OnUpdate()
	{

		var cc = GameObject.Components.Get<CharacterController>();

		if ( MovementMult == 0.0f )
			return;
		AnimHelper.WithVelocity( Transform.Rotation.Forward * 300.0f + Transform.Rotation.Left * SideMoveVel );
		AnimHelper.IsGrounded = cc.IsOnGround;
		AnimHelper.SpecialMove = MoveStyle;
	}

	protected override void OnFixedUpdate()
	{
		base.OnFixedUpdate();

		if ( MovementMult == 0.0f )
			return;


		var cc = GameObject.Components.Get<CharacterController>();


		if ( Rolling )
		{
			cc.Height = 22;
			DeathColl.Scale = new Vector3( 48, 48, 23 );
		}
		else
		{
			cc.Height = 72;
			DeathColl.Scale = new Vector3( 48, 48, 74 );
		}


		// Side to side movement

		if (Input.Pressed("Left"))
		{
			QueueMovement( 1 );
		}
		if (Input.Pressed("Right"))
		{
			QueueMovement( -1 );
		}

		cc.Accelerate( Vector3.Forward * 500.0f * Score.GetSpeedMult() * GameGlobals.SpeedMultiplier );
		cc.Velocity = cc.Velocity.WithY( SideMoveVel );
		cc.ApplyFriction( 2.0f );


		if (Movements.Count > 0 && !Moving)
		{
			_ = MoveToSide(Movements.First());
		}


		if ( cc.IsOnGround && (Input.Down( "Forward" ) || Input.Down( "Jump" )))
		{
			float flGroundFactor = 1.0f;
			float flMul = JumpVelocity * GameGlobals.JumpMultiplier;
			//if ( Duck.IsActive )
			//	flMul *= 0.8f;

			cc.Punch( Vector3.Up * flMul * flGroundFactor );
			//	cc.IsOnGround = false;
		}

		cc.Move();

		if ( !cc.IsOnGround )
		{
			var gravMult = 1.0f;
			if ( Input.Down( "Backward" ) )
				gravMult = 4.0f;

			cc.Velocity -= Vector3.Down * GetGravity(cc) * gravMult * Time.Delta * 0.5f;
			RollGroundTime = 0;
		}
		else
		{
			cc.Velocity = cc.Velocity.WithZ( 0 );
		}

		if (Input.Down("Backward") && cc.IsOnGround)
		{
			_ = Duck();
		}

		/*		if ( Transform.Position.x > 576 )
				{
					WrapX();
				}*/

		RunPartcles.Enabled = cc.IsOnGround;

		if (!Moving)
		{
			Transform.Position = Transform.Position.WithY( MathF.Round( Transform.Position.y / 128.0f ) * 128.0f );
		}
	}


	async Task Duck()
	{
		Rolling = true;
		if (RollGroundTime < 1)
			MoveStyle = CitizenAnimationHelper.SpecialMoveStyle.Roll;
		else
			MoveStyle = CitizenAnimationHelper.SpecialMoveStyle.Slide;
		await Task.DelaySeconds( 0.8f );
		Rolling = false;
		MoveStyle = CitizenAnimationHelper.SpecialMoveStyle.None;
	}

	List<float> Movements = new List<float>();
	async Task MoveToSide(float to)
	{
		Moving = true;
		TimeSince timeSince = 0;

		while ( timeSince < SideMoveTime )
		{
			if ( !GameObject.IsValid() )
				return;
			SideMoveVel = to * SideMoveTargetVel;
			await Task.FixedUpdate(); // wait one frame
		}
		Moving = false;
		SideMoveVel = 0.0f;
		Movements.RemoveAt( 0 );
	}

	void QueueMovement(float dir)
	{
		Movements.Add( dir );
	}

	float GetGravity(CharacterController cc)
	{
		if ( cc.Velocity.y >= 0.0f )
		{
			return JumpGravity;
		}
		else
		{
			return FallGravity;
		}
	}

	public void StartMovement()
	{
		MovementMult = 1;
		Score.StartScore();
	}
}
