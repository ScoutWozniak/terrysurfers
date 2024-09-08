using Sandbox;

public sealed class CameraFollowComponent : Component, Component.ExecuteInEditor
{
	[Property] public GameObject Target { get; set; }
	[Property] public Vector3 Dir { get; set; } = new Vector3(0, 1, 1 );
	[Property] public float Distance { get; set; } = 500.0f;
	protected override void OnUpdate()
	{
		if (Target != null )
		{
			Transform.Position = Target.Transform.Position + Dir * Distance;

			Transform.Rotation = Rotation.LookAt( (Target.Transform.Position - Transform.Position).Normal );
		}
	}
}
