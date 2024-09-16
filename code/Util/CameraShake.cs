using Sandbox;
using Sandbox.Events;
using Sandbox.Utility;
using System.ComponentModel.Design;
using System.Threading.Tasks;

public sealed class CameraShake : Component, IGameEventHandler<OnStateChangedEvent>
{
	[RequireComponent] CameraFollowComponent CameraFollow { get; set; }

	public void OnGameEvent( OnStateChangedEvent eventArgs )
	{
		if(eventArgs.toState == GameStates.GameOver )
		{
			_ = ShakeCamera();
		}
	}

	public async Task ShakeCamera()
	{
		float length = 0.5f;
		TimeSince elapsed = 0;
		Log.Info( "test" );
		while (elapsed < length)
		{
			float x = (Noise.Perlin( elapsed ) - 0.5f);
			float y = (Noise.Perlin(elapsed) - 0.5f);

			CameraFollow.Offset = new Vector3( x, 0, y ) * 10.0f;
			await Task.Frame();
		}
		CameraFollow.Offset = Vector3.Zero;
	}
}
