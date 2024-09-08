using Sandbox;

public sealed class LevelSegment : Component
{
	[Property] public float Width { get; set; } = 576;

	[Property] bool IsObstacles { get; set; } = false;
	public void MoveSegment()
	{
		Transform.Position = Transform.Position - Vector3.Forward * Width;
		Transform.ClearInterpolation();
	}

	protected override void DrawGizmos()
	{
		base.DrawGizmos();
		if ( !IsObstacles )
			return;

		var model = Model.Load( "models/dev_segment_outline.vmdl" );
		for ( int i = 0; i < (int)Width/576.0f; i++ )
		{
			Gizmo.Draw.Model( model, new Transform(Vector3.Forward * i * 576) );
		}
	}
}
