using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public class GameGlobals : GameObjectSystem
{
	public GameGlobals( Scene scene ) : base( scene )
	{
		JumpMultiplier = 1.0f;
		SpeedMultiplier = 1.0f;
	}

	public static float SegmentLength = 1728;

	
	public static float SpeedMultiplier = 1.0f;

	public static float JumpMultiplier = 1.0f;
}

