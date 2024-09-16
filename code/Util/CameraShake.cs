using Sandbox;
using Sandbox.Events;
using Sandbox.Utility;
using System.ComponentModel.Design;
using System.Threading;
using System.Threading.Tasks;

public sealed class CameraShake : Component, IGameEventHandler<OnStateChangedEvent>
{
	[RequireComponent] CameraFollowComponent CameraFollow { get; set; }

	float duration = 0.5f;
	float amplitude = 1.0f;


	public void OnGameEvent( OnStateChangedEvent eventArgs )
	{
		if(eventArgs.toState == GameStates.GameOver )
		{
			_ = ShakeCamera();
		}
	}

	public async Task ShakeCamera()
	{
		TimeSince elapsed = 0;
		Log.Info( "test" );
		while (elapsed < duration)
		{
/*			float x = (Noise.Perlin( elapsed * 1000.0f ) - 0.5f);
			float y = (Noise.Perlin((elapsed + 100) * 1000.0f) - 0.5f);
			CameraFollow.Offset =CameraFollow.Offset.LerpTo( new Vector3( 0, x, y ) * 100.0f, Time.Delta * 100.0f);
			Log.Info( x);*/
			

			float intensity = amplitude * (1 - (duration - elapsed) / duration);

			var prevOffset = CameraFollow.Offset;

			Vector3 newVals = new Vector3( Game.Random.Float( -1, 1 ), Game.Random.Float( -1, 1 ), Game.Random.Float( -1, 1 ) );
			Vector3 newOffset = new();
			newOffset.x = intensity * (prevOffset.x + (Time.Delta * (newVals.x - prevOffset.x)));
			newOffset.y = intensity * (prevOffset.y + (Time.Delta * (newVals.y - prevOffset.y)));
			newOffset.z = intensity * (prevOffset.z + (Time.Delta * (newVals.z - prevOffset.z)));
			Log.Info( newOffset );
			await Task.Frame();
		}
		CameraFollow.Offset = Vector3.Zero;
	}
}
