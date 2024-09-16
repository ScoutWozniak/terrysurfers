using Sandbox;
using System.Reflection.Metadata;
using System.Threading.Tasks;

public sealed class PlayerTriggerComponent : Component, Component.ITriggerListener
{
	[Property] ScoreComp Score { get; set; }

	[Property] DeathUiComponent DeathUi { get; set; }

	[Property] ModelPhysics Ragdoll { get; set; }

	[Property] Curve DeathTimeScale { get; set; }

	SoundHandle deathSound { get; set; }

	public void OnTriggerEnter( Collider other )
	{
		if ( other.Tags.Has( "death" ) ) 
			OnDeath();
	}

	void OnDeath()
	{
		Ragdoll.Enabled = true;
		Score.StopScore();
		DeathUi.GameObject.Enabled = true;
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
		}
	}

}
