using Sandbox;
using System.Reflection.Metadata;
using System.Threading.Tasks;

public sealed class PlayerTriggerComponent : Component, Component.ITriggerListener
{
	[Property] ScoreComp Score { get; set; }

	[Property] DeathUiComponent DeathUi { get; set; }

	[Property] ModelPhysics Ragdoll { get; set; }

	[Property] Curve DeathTimeScale { get; set; }

	[Property] CharacterController cc { get; set; }

	SoundHandle deathSound { get; set; }

	[Property] GameObject Gibs { get; set; }

	public void OnTriggerEnter( Collider other )
	{
		if ( other.Tags.Has( "death" ) ) 
			OnDeath(other.Tags.Has("gib"));
	}

	void OnDeath(bool gib = false)
	{
		if ( !gib )
		{
			Ragdoll.Enabled = true;
			Ragdoll.PhysicsGroup.AddVelocity( cc.Velocity * 0.1f );
		}
		else
		{
			var gibs = Gibs.Clone(Transform.Position, Transform.Rotation );
			Ragdoll.Renderer.GameObject.Enabled = false;
			foreach(var prop in gibs.Components.GetAll<Rigidbody>(FindMode.EverythingInSelfAndDescendants))
			{
				prop.ApplyForce( cc.Velocity * 1000.0f );
			}
		}


		Score.StopScore();
		//DeathUi.GameObject.Enabled = true;
		deathSound = Sound.Play( "death.yell", Transform.Position );
		deathSound.Pitch = 0.5f;
		//Sound.Play( "player.fart", Transform.Position );
		GameStateManager.Instance.GameState = GameStates.GameOver;
		Enabled = false;
		DeathSlowDown();
	}

	async Task DeathSlowDown()
	{
		TimeSince timeSince = 0;
		float length = 0.5f;

		while ( timeSince < length )
		{
			Scene.TimeScale = DeathTimeScale.Evaluate( timeSince / length );
			await Task.Frame();

			if (timeSince / length > 0.75f)
				DeathUi.GameObject.Enabled = true;
		}
	}

}
