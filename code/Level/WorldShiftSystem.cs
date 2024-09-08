using Sandbox;
using System;


public class WorldShiftSystem : GameObjectSystem
{
	PlayerControllerComponent player { get; set; }

	public bool QueueShift { get; set; } = false;
	public WorldShiftSystem(Scene scene) : base( scene )
	{
		Listen( Stage.PhysicsStep, 0, Tick, "Shift world" );
		Listen( Stage.SceneLoaded,0, OnStart, "Start" );
	}

	private void OnStart()
	{
		player = Scene.Components.Get<PlayerControllerComponent>(FindMode.InDescendants);
	}

	private void Tick()
	{
		if (QueueShift)
			ShiftWorld();
	}

	private void ShiftWorld()
	{
		QueueShift = false;
		var levelHandler = Scene.Components.Get<InfiniteLevelHandler>( FindMode.InDescendants );
		levelHandler.MoveLevel();
	}
}
