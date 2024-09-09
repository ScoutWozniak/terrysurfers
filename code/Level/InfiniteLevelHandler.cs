using Sandbox;
using Sandbox.Events;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public sealed class InfiniteLevelHandler : Component
{
	[Property] List<GameObject> Segments { get; set; }

	PlayerControllerComponent Player { get; set; }

	[Property] List<GameObject> SegmentPrefabs { get; set; }

	[Property] List<LevelTheme> Themes { get; set; }

	[Property] LevelTheme CurrentTheme { get; set; }

	int Movements = 0;
	protected override void OnStart()
	{
		base.OnStart();
		Player = Scene.Components.Get<PlayerControllerComponent>( FindMode.InDescendants );
		CurrentTheme = Themes.First();

		AddSegment();
		AddSegment();
		AddSegment();
		AddSegment();
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (!Player.IsValid() )
			return;

		if ( Player.Transform.Position.x >= GameGlobals.SegmentLength )
		{
			Scene.GetSystem<WorldShiftSystem>().QueueShift = true;
		}
	}

	public void MoveLevel()
	{
		Movements++;
		var posXDiff = Player.Transform.Position.x - GameGlobals.SegmentLength;
		Player.Transform.Position = Player.Transform.Position.WithX( posXDiff );
		Player.Transform.ClearInterpolation();

		if ( Movements > 1 )
		{
			Segments.First().Destroy();
			Segments.Remove( Segments.First() );
			AddSegment();
		}

		Scene.Dispatch( new OnLevelShift( GameGlobals.SegmentLength ) );
		foreach ( var obj in Segments )
		{
			foreach( var segment in obj.Components.GetAll<LevelSegment>( FindMode.EnabledInSelfAndDescendants ))
			{
				segment.MoveSegment();
			}
		}
	}

	public void AddSegment()
	{
		var go = new GameObject(true, "Segment");
		go.Transform.Position = Vector3.Forward * GameGlobals.SegmentLength * Segments.Count;

		var newObstacle = SegmentPrefabs.ElementAt( Game.Random.Int( SegmentPrefabs.Count - 1 ) ).Clone();
		newObstacle.SetParent( go );
		newObstacle.Transform.World = go.Transform.World;

		var newScenery = CurrentTheme.Prefabs.ElementAt( Game.Random.Int( CurrentTheme.Prefabs.Count - 1 ) ).Clone();
		newScenery.SetParent( go );
		newScenery.Transform.World = go.Transform.World;

		Segments = Segments.Append( go ).ToList();

	}
}
