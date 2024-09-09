using Sandbox;

public sealed class JumpBoots : Component, IPowerupListener
{
	[Property] float JumpIncrease { get; set; } = 0.5f;
	void IPowerupListener.OnPowerupCollected( Powerup powerup )
	{
		Scene.GetSystem<GlobalSystem>().Get<PlayerGlobals>().JumpMultiplier += JumpIncrease;
	}

	void IPowerupListener.RemoveEffects()
	{
		Scene.GetSystem<GlobalSystem>().Get<PlayerGlobals>().JumpMultiplier -= JumpIncrease;
	}

}
