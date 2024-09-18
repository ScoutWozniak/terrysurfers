using Sandbox;

public sealed class BoxObstacle : Component, Component.ICollisionListener
{
	void ICollisionListener.OnCollisionStart(Sandbox.Collision collision)
	{
		if (collision.Other.GameObject.Tags.Has("player"))
		{
			Log.Info( "test" );
		}
	}
}
