using Sandbox.UI;
using System;

public partial class Powerupbar : Panel
{
	float Progress { get; set; } = 1.0f;

	public Powerup TrackingPowerup { get; set; }

	public override void Tick()
	{
		base.Tick();
		Progress = TrackingPowerup.GetProgress();
	}

	protected override int BuildHash()
	{
		HashCode c = new();
		c.Add( Progress );
		return c.ToHashCode();
	}
}
