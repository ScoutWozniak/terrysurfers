using Sandbox;

public sealed class BoxObstacle : Component, Component.ICollisionListener
{
	[RequireComponent] Prop prop { get; set; }

	void ICollisionListener.OnCollisionStart(Sandbox.Collision collision)
	{
		if (collision.Other.GameObject.Tags.Has("player"))
		{
			prop.Break();
		}
	}
}
