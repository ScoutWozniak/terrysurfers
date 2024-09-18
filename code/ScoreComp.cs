using Sandbox;

public sealed class ScoreComp : Component
{
	public int Score;

	public float Distance;

	[Property] Curve SpeedDistScale { get; set; }

	[Property] float MaxSpeedDistance { get; set; } = 10000000.0f;

	bool Dead = true;

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (!Dead )
		{
			Distance += 2 * GetSpeedMult() * Time.Delta;
		}
	}

	public void StopScore()
	{
		Dead = true;
	}

	public void StartScore()
	{
		Dead = false;
	}

	public float GetSpeedMult()
	{
		return SpeedDistScale.Evaluate(Distance / MaxSpeedDistance);
	}
}
