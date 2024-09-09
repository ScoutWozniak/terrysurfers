using Sandbox;
using Sandbox.Citizen;
using Sandbox.Events;
using Sandbox.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public sealed class PlayerControllerComponent : Component, IGameEventHandler<OnStateChangedEvent>
{
	[RequireComponent] CharacterController CharController { get; set; } 

	[Property] ParticleConeEmitter RunPartcles { get; set; }

	[Property] CitizenAnimationHelper AnimHelper { get; set; }


	[Property] public BoxCollider DeathColl { get; set; }

	[Property] ScoreComp Score { get; set; }

	CitizenAnimationHelper.SpecialMoveStyle MoveStyle { get; set; }


	[Property, Category( "Jumping" )] public float JumpHeight { get; set; } = 256;

	[Property, Category( "Jumping" )] float JumpTimeToPeak { get; set; }

	[Property, Category( "Jumping" )] float JumpTimeToDescent { get; set; }

	float MovementMult = 0.0f;

	bool Rolling = false;
	TimeSince RollGroundTime { get; set; } = 2;

	Vector3 Velocity { get; set; }

	float JumpVelocity { get; set; }
	float JumpGravity { get; set; }
	float FallGravity { get; set; }

	float Gravity { get { return CharController.Velocity.z >= 0.0f ? JumpGravity : FallGravity; } }

	protected override void OnEnabled()
	{
		CalculateJumpHeight();
	}

	protected override void OnUpdate()
	{
		if ( MovementMult == 0.0f )
			return;
		AnimHelper.WithVelocity( CharController.Velocity / 2 );
		AnimHelper.WithWishVelocity( Velocity );
		AnimHelper.IsGrounded = CharController.IsOnGround;
		AnimHelper.SpecialMove = MoveStyle;
	}

	protected override void OnFixedUpdate()
	{
		base.OnFixedUpdate();

		if ( MovementMult == 0.0f )
			return;

		if ( Rolling )
		{
			CharController.Height = 22;
			DeathColl.Scale = new Vector3( DeathColl.Scale.x, DeathColl.Scale.y, 23 );
		}
		else
		{
			CharController.Height = 72;
			DeathColl.Scale = new Vector3( DeathColl.Scale.x, DeathColl.Scale.y, 74 );
		}


		// Side to side movement
		int dir = 0;
		if ( Input.Down( "Left" ) )
			dir = 1;
		if ( Input.Down( "Right" ) )
			dir = -1;

		Vector3 DirVec = (Vector3.Forward + Vector3.Left).Normal;

		Velocity = Vector3.Forward * DirVec.x * 1000.0f * Score.GetSpeedMult() * GameGlobals.SpeedMultiplier;
		Velocity += Vector3.Left * DirVec.y * dir * 500.0f;

		CharController.Accelerate( Velocity );
		CharController.ApplyFriction( 4.0f );


		if ( CharController.IsOnGround && (Input.Pressed( "Forward" ) || Input.Pressed( "Jump" )))
		{
			float flGroundFactor = 1.0f;
			float flMul = JumpVelocity * GameGlobals.JumpMultiplier;

			CharController.Punch( Vector3.Up * flMul * flGroundFactor );
		}

		if ( !CharController.IsOnGround )
		{
			var gravMult = 1.0f;

			if ( Input.Down( "Backward" ) )
				gravMult = 4.0f;

			CharController.Velocity -= Vector3.Down * Gravity * gravMult * Time.Delta * 0.5f;
			RollGroundTime = 0;
		}
		else
		{
			CharController.Velocity = CharController.Velocity.WithZ( 0 );
		}

		if (Input.Down("Backward") && CharController.IsOnGround && !Rolling)
		{
			_ = Duck();
		}

		CharController.Move();

		RunPartcles.Enabled = CharController.IsOnGround;
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

	
	public void StartMovement()
	{
		MovementMult = 1;
		Score.StartScore();
	}

	void CalculateJumpHeight()
	{
		JumpVelocity = (2.0f * JumpHeight) / JumpTimeToPeak;
		JumpGravity = (-2.0f * JumpHeight) / (JumpTimeToPeak * JumpTimeToPeak);
		FallGravity = (-2.0f * JumpHeight) / (JumpTimeToDescent * JumpTimeToDescent);
	}

	public void OnGameEvent( OnStateChangedEvent eventArgs )
	{
		Log.Info( eventArgs.toState );
		if ( eventArgs.toState == GameStates.GameOver )
		{
			MovementMult = 0;
			RunPartcles.Enabled = false;
		}
	}
}
