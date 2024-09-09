using System;
using Sandbox;

public class PlayerGlobals : GlobalComponent
{
	[Property] public float RunMultiplier { get; set; } = 1.0f;

	[Property] public float JumpMultiplier { get; set; } = 1.0f;
}
