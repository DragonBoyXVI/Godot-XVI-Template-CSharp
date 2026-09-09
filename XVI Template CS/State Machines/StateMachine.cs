using System.Collections.Generic;
using Godot;

namespace DragonXVI.GodotTemplate.StateMachines;

[GlobalClass, Tool, Icon( "uid://d1tih0h8h2lhj" )]
public partial class StateMachine : Node {
    /// <summary>
    /// Emitted after the state is entered.
    /// </summary>
    /// <param name="state">The entered state.</param>
    [Signal]
    public delegate void StateEnteredEventHandler( State state );
    /// <summary>
    /// Emitted after the state is left.
    /// </summary>
    /// <param name="state">The left state.</param>
    [Signal]
    public delegate void StateLeftEventHandler( State state );

    /// <summary>
    /// The state the state machine swiches to when readied.
    /// </summary>
    [Export]
    private State InitialState {
        set {
            initialState = value;
            UpdateConfigurationWarnings();
        }
        get => initialState;
    }
    private State initialState;
    /// <summary>
    /// The current state og the machine, not meant to be set directley.
    /// Use the change function instead.
    /// </summary>
    public State CurrentState{
        private set;
        get;
    }
    public State GetCurrentState() => CurrentState;
    /// <summary>
    /// A cache for the states this machine owns.
    /// The machine only collects states on ready, so adding states after does nothing.
    /// </summary>
    private readonly Dictionary< StringName, State > StateCache = [];

    public override void _Ready()
    {
        if ( Engine.IsEditorHint() ){
            XVIFuncs.SetNodeProcesses( this, false );
            return;
        }

        foreach (var child in GetChildren() )
        {
            if ( child is State state ) {
                RegisterState( state );
            }
        }

        if ( InitialState != null ) {
            ChangeState( InitialState.Name );
        }
    }
    public override string[] _GetConfigurationWarnings() {
        List<string> warnings = [];

        if ( InitialState == null ) {
            warnings.Add( "No initial state set! State machine wont act unless you use the change method." );
        }

        return [.. warnings];
    }

    /// <summary>
    /// Used to add a state to this machine.
    /// </summary>
    /// <param name="state">The state to add.</param>
    private void RegisterState( State state ) {
        StringName stateName = state.Name;

        if ( StateCache.ContainsKey( stateName ) ) {
            GD.PushError( "Attempting to add dupe state: ", stateName );
            return;
        }

        if ( !state.IsInsideTree() ) {
            AddChild( state );
        } else if ( state.GetParent() != this ) {
            state.Reparent( this );
        }

        StateCache[ stateName ] = state;
        state._Disable();
        state.StateChangeRequest += OnStateChangeRequested;
    }

    /// <summary>
    /// Changes the state of this machine.
    /// </summary>
    /// <param name="stateName">The name of the state to change to.</param>
    public void ChangeState( StringName stateName ) {
        if (!StateCache.TryGetValue(stateName, out State state))
        {
            GD.PushError("Trying to get an invalid state: ", stateName);
            return;
        }

        if ( CurrentState != null ) {
            if ( !CurrentState.CanSwitchState( state ) ) {
                return;
            }

            CurrentState._LeaveState();
            CurrentState._Disable();
            EmitSignal( SignalName.StateLeft, CurrentState );
        }

        CurrentState = state;
        CurrentState._Enable();
        CurrentState._EnterState();
        EmitSignal( SignalName.StateEntered, state );
    }

    private void OnStateChangeRequested( StringName stateName ) {
        ChangeState( stateName );
    }
}