using Sandbox;

public sealed class RunBoots : Component, IPowerupListener
{
	void IPowerupListener.OnPowerupCollected( Powerup powerup )
	{
		GameGlobals.SpeedMultiplier += 4.0f;
	}

	void IPowerupListener.RemoveEffects()
	{
		GameGlobals.SpeedMultiplier -= 4.0f;
	}
}
