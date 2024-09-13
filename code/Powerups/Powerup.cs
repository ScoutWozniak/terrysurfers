using Sandbox;
using System;

public sealed class Powerup : Component, Component.ITriggerListener
{
	[Property] Action OnCollect { get; set; }

	[Property] string Name { get; set; }

	[Property] bool HasDuration { get; set; } = true;
	[Property, ShowIf("HasDuration", true)] float Duration { get; set; } = 2.0f;

	private TimeUntil UntilExpired { get; set; }

	private bool IsActive { get; set; }

	protected override void OnFixedUpdate()
	{
		base.OnFixedUpdate();
		if ( !HasDuration )
			return;
		if ( IsActive )
		{
			if ( UntilExpired <= 0 )
			{
				IsActive = false;
				foreach ( var listener in Components.GetAll<IPowerupListener>() )
				{
					listener.RemoveEffects();
				}
			}
		}
	}


	void ITriggerListener.OnTriggerEnter(Sandbox.Collider other)
	{
		if (other.Tags.Has("player"))
		{
			OnCollect?.Invoke();
			foreach ( var listener in Components.GetAll<IPowerupListener>() )
			{
				listener.OnPowerupCollected( this );
			}
			
			IsActive = true;

			UntilExpired = Duration;
		}
	}

	public float GetProgress()
	{
		return ((UntilExpired.Passed - Duration)  / Duration) * -1;
	}
}
