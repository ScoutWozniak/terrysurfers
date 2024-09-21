using Sandbox;
using System.Collections.Generic;
using System.Linq;

public sealed class PowerupSpawner : Component
{
	[Property] List<GameObject> Powerups { get; set; }

	float ChanceToSpawn { get; set; } = 0.1f;


	protected override void OnStart()
	{
		base.OnStart();

		if ( Game.Random.Float() > ChanceToSpawn )
			return;

		var go = Powerups.ElementAt( Game.Random.Int( 0, Powerups.Count - 1 ) ).Clone(Transform.Position, Transform.Rotation);
		go.SetParent( GameObject );
	}
}
