using Sandbox;
using Sandbox.UI;
using System;
using System.Linq;


public partial class Shop : Panel
{
	Outfit LoadedOutfit { get; set; }
	int Index = 1;

	public int CurrentCoins { get; set; }

	protected override void OnAfterTreeRender( bool firstTime )
	{
		base.OnAfterTreeRender( firstTime );
		if ( firstTime )
		{
			ChangeOutfit();
			EquipClothing();
		}
	}

	void ChangeOutfit(int dif = 0)
	{
		var outfits = ResourceLibrary.GetAll<Outfit>().OrderBy(x => x.CoinsNeeded);

		if ( Index + dif >= outfits.Count() )
			Index = -1 * ((Index + dif) - outfits.Count());
		else if ( Index + dif < 0 )
			Index = outfits.Count() + (Index + dif);
		else
			Index += dif;

		LoadedOutfit = outfits.ElementAt( Index );
	}

	protected override int BuildHash()
	{
		HashCode a = new();
		a.Add( LoadedOutfit );
		a.Add( CurrentCoins );
		Log.Info( CurrentCoins );
		return a.ToHashCode();
	}

	void EquipClothing()
	{
		if ( LoadedOutfit == null )
			return;
		var player = Scene.Directory.FindByName( "PBody" ).First().Components.Get<SkinnedModelRenderer>();
		LoadedOutfit.CreateContainer().Apply( player );
	}
}
