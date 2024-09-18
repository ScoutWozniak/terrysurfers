using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[GameResource( "Outfit", "outfit", "Player outfit!" )]
public class Outfit : GameResource
{
	public string Name { get; set; }

	public int CoinsNeeded { get; set; }

	public List<Clothing> Clothing { get; set; }

	public ClothingContainer CreateContainer()
	{
		var container = new ClothingContainer();

		foreach ( var clothe in Clothing )
		{
			container.Toggle( clothe );
		}
		return container;
	}
}

