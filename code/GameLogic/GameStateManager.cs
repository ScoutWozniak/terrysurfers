using Sandbox;
using Sandbox.Events;

public enum GameStates
{
	Menu,
	Playing,
	Paused,
	GameOver
}

public sealed class GameStateManager : Component
{
	private GameStates gameState = GameStates.Menu;

	[Property] public GameStates GameState { get => gameState; set { Scene.Dispatch( new OnStateChangedEvent( value, gameState ) ); gameState = value; } }
}
