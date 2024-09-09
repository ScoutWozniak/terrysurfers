using Sandbox;
using System.Threading.Tasks;

public sealed class PhysicsReloadFix : Component
{
	[RequireComponent] Rigidbody Rb { get; set; }

	protected override void OnStart()
	{
		_ = EnableAfterFrame();
	}

	async Task EnableAfterFrame()
	{
		await Task.FixedUpdate();
		Rb.Enabled = true;
	}
}
