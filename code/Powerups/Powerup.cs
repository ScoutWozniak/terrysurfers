using Sandbox;
using System;

public sealed class Powerup : Component, Component.ITriggerListener
{
	[Property] Action OnCollect { get; set; }

	[Property] string Name { get; set; }


	void ITriggerListener.OnTriggerEnter(Sandbox.Collider other)
	{
		if (other.Tags.Has("player"))
		{
			OnCollect?.Invoke();
			foreach ( var listener in Components.GetAll<IPowerupListener>() )
			{
				listener.OnPowerupCollected( this );
			}
		}
	}
}
