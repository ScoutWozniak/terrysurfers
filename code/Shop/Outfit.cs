using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[GameResource("Outfit", "outfit", "Player outfit!")]
public class Outfit : GameResource
{
	public string Name { get; set; }

	public List<Clothing> Clothing { get; set; }
	
}

