using Sandbox;
using System.Collections.Generic;

public sealed class LevelComponent : Component
{
	[Property] public float LaneSize { get; set; } = 128.0f;

	[Property] GameObject DefaultConfig { get; set; }
	[Property] List<GameObject> LevelConfigs { get; set; } = new List<GameObject>();

	TimeSince SpawnDelay = 0;

	int SectionCount = 0;

	[Property] GameObject ActivePart { get; set; }

	protected override void OnEnabled()
	{
		base.OnEnabled();
		//AddConfig();
	}


	protected override void OnFixedUpdate()
	{
		//Transform.Position = Transform.Position + Vector3.Backward * 10.0f;

		if ( Transform.Position.x > 576.0f )
		{
			//Transform.Position = Transform.Position.WithX( Transform.Position.x - 576.0f );
			//MoveSections();
		}
	}

	void AddConfig()
	{
		

		Vector3 newPos = Transform.Position;
		GameObject go;
		Log.Info( Game.Random.Int( LevelConfigs.Count - 1 ) );
		go = SceneUtility.Instantiate( LevelConfigs[Game.Random.Int(LevelConfigs.Count - 1 )], newPos, Transform.Rotation);

		if (ActivePart != null )
		{
			ActivePart.Destroy();
		}

		ActivePart = go;
		go.Parent = GameObject;
	}

	public void MoveSections()
	{
		AddConfig();
	}
}
