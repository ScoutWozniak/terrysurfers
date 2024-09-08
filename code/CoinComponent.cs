using Sandbox;
using System;

public sealed class CoinComponent : Component
{
	[Property] float RotationSpeed { get; set; }
	[Property] GameObject CollectParticle { get; set; }

	protected override void OnEnabled()
	{
		base.OnEnabled();

	}

	protected override void OnFixedUpdate()
	{
		Transform.Rotation = Transform.Rotation * new Angles(0,RotationSpeed,0).ToRotation();
	}

	public void OnCollect()
	{
		CollectParticle.Clone( Transform.Position, Transform.Rotation ); // Clone the OnCollect GameObject at the same position and rotation as the coin
		GameObject.Destroy();
	}
}
