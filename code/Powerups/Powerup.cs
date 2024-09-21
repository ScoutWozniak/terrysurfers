using Sandbox;
using Sandbox.Events;
using System;

public sealed class Powerup : Component, Component.ITriggerListener, IGameEventHandler<OnStateChangedEvent>
{
	[Property] Action OnCollect { get; set; }

	[Property] string Name { get; set; }

	[Property] bool HasDuration { get; set; } = true;
	[Property, ShowIf("HasDuration", true)] float Duration { get; set; } = 2.0f;

	private TimeUntil UntilExpired { get; set; }

	private bool IsActive { get; set; }

	[Property] GameObject Model { get; set; }

	[Property] SphereCollider Collider { get; set; }

	protected override void OnFixedUpdate()
	{
		base.OnFixedUpdate();
		if ( !HasDuration )
			return;
		if ( IsActive )
		{
			if ( UntilExpired <= 0 )
			{
				RemoveEffects();
			}
		}
	}


	void ITriggerListener.OnTriggerEnter(Sandbox.Collider other)
	{
		if (other.Tags.Has("player"))
		{
			Collect();
		}
	}

	public float GetProgress()
	{
		return ((UntilExpired.Passed - Duration)  / Duration) * -1;
	}

	public void OnGameEvent( OnStateChangedEvent eventArgs )
	{
		if ( eventArgs.toState == GameStates.GameOver )
		{
			RemoveEffects();
		}
	}

	void Collect()
	{
		Log.Info( $"Power up {GameObject.Name} activated" );
		OnCollect?.Invoke();
		foreach ( var listener in Components.GetAll<IPowerupListener>() )
		{
			listener.OnPowerupCollected( this );
		}

		IsActive = true;
		UntilExpired = Duration;

		GameObject.SetParent( Scene.Root );

		Model.Enabled = false;
		Collider.Enabled = false;
	}

	void RemoveEffects()
	{
		IsActive = false;
		foreach ( var listener in Components.GetAll<IPowerupListener>() )
		{
			listener.RemoveEffects();
		}
		GameObject.Destroy();
	}
}
