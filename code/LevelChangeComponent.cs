using Sandbox;

public sealed class LevelChangeComponent : Component, Component.ITriggerListener
{
	public void OnTriggerEnter( Collider other )
	{
		Log.Info( "yeah" );
		if (other.Tags.Has("player"))
		{
			var player = Scene.Components.Get<PlayerControllerComponent>(FindMode.EverythingInDescendants);
			player.DeathColl.Enabled = false;
			player.Transform.Position = player.Transform.Position.WithX( 0 );
			player.DeathColl.Enabled = true;

			Scene.Components.Get<LevelComponent>( FindMode.EverythingInDescendants ).MoveSections();

		}
	}

	public void OnTriggerExit( Collider other )
	{

	}
}
