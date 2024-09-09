using Sandbox;

public sealed class RunBoots : Component, IPowerupListener
{
	[Property] float SpeedIncrease { get; set; } = 0.5f;
	void IPowerupListener.OnPowerupCollected( Powerup powerup )
	{
		Scene.GetSystem<GlobalSystem>().Get<PlayerGlobals>().RunMultiplier += SpeedIncrease;
	}

	void IPowerupListener.RemoveEffects()
	{
		Scene.GetSystem<GlobalSystem>().Get<PlayerGlobals>().RunMultiplier -= SpeedIncrease;
	}
}
