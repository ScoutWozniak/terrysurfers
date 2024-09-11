using Sandbox.UI;
using System;
using Sandbox;
public partial class Title : Panel
{
	float rot = 0;
	float scale = 1;
	public override void Tick()
	{
		base.Tick();
		rot = (float)Math.Sin(Time.Now * 1 ) * 5;
		scale = 1 + (float)(Math.Sin( Time.Now * 2 ) * 0.25);
	}
	protected override int BuildHash()
	{
		HashCode c = new();
		c.Add( rot );
		return c.ToHashCode();
	}
}
