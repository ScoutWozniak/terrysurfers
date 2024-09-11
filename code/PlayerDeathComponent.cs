using Sandbox;

public sealed class PlayerTriggerComponent : Component, Component.ITriggerListener
{
	[Property] ScoreComp Score { get; set; }

	[Property] DeathUiComponent DeathUi { get; set; }

	[Property] ModelPhysics Ragdoll { get; set; }

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
		Sound.Play( "death.yell", Transform.Position );
		//Sound.Play( "player.fart", Transform.Position );
		GameStateManager.Instance.GameState = GameStates.GameOver;
		Enabled = false;
	}
}
