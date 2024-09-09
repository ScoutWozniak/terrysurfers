using Sandbox;

public sealed class PlayerTriggerComponent : Component, Component.ITriggerListener
{
	[Property] GameObject RagdollPrefab { get; set; }
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
		var player = GameObject.Parent.Components.Get<PlayerControllerComponent>();
		player.Destroy();
		Score.StopScore();
		DeathUi.GameObject.Enabled = true;
		Sound.Play( "death.yell", Transform.Position );
		Scene.Components.Get<GameStateManager>( FindMode.InDescendants ).GameState = GameStates.GameOver;
	}
}
