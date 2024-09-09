using System;
using Sandbox.Events;
public record OnStateChangedEvent( GameStates toState, GameStates fromState) : IGameEvent;

public record OnLevelShift(float ammount) : IGameEvent;
