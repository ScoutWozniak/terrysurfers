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
		{
			OnDeath();
		}
		else if ( other.Tags.Has( "coin" ) ) {  
			CollectCoin( other.GameObject );
		}
	}


	void OnDeath()
	{
		//var ragdoll = RagdollPrefab.Clone( Transform.Position );
		Ragdoll.Enabled = true;
		var player = GameObject.Parent.Components.Get<PlayerControllerComponent>();
		player.Destroy();
		//ragdoll.Enabled = true;
		//ragdoll.Components.Get<ModelPhysics>().Renderer = GameObject.Parent.Components.Get<SkinnedModelRenderer>(FindMode.EverythingInSelfAndChildren);
		Score.StopScore();
		DeathUi.GameObject.Enabled = true;
		Sound.Play( "death.yell", Transform.Position );
	}

	void CollectCoin(GameObject coin)
	{
		coin.Components.Get<CoinComponent>().OnCollect();
		Score.Score += 1;
	}
}
