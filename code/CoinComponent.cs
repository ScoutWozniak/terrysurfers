using Sandbox;
using System;

public sealed class CoinComponent : Component, Component.ITriggerListener
{
	[Property] float RotationSpeed { get; set; }
	[Property] GameObject CollectParticle { get; set; }

	[Property] GameObject Body { get; set; }


	public void OnCollect()
	{
		CollectParticle.Clone( Transform.Position, Transform.Rotation ); // Clone the OnCollect GameObject at the same position and rotation as the coin
		GameObject.Destroy();
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		Body.Transform.Rotation = Body.Transform.Rotation * new Angles( 0, RotationSpeed, 0 ).ToRotation();
	}

	void ITriggerListener.OnTriggerEnter(Sandbox.Collider other )
	{
		if ( other.Tags.Has( "player" ) )
		{
			OnCollect();
			Scene.Components.Get<ScoreComp>(FindMode.InDescendants).Score += 1;
		}
	}
}
