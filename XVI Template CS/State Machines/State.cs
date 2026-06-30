using Godot;

namespace DragonXVI.StateMachines;

#pragma warning disable IDE1006

/// <summary>
/// A template state node, meant to be a child of a state machine node.
/// </summary>
[GlobalClass, Tool, Icon( "uid://co7j8ckrp6x72" )]
public abstract partial class State : Node {
    /// <summary>
    /// used to tell the parent state machine to change states.
    /// </summary>
    /// <param name="stateName">Name of the state to change to.</param>
    [Signal]
    public delegate void StateChangeRequestEventHandler( StringName stateName );
    /// <summary>
    /// used to tell the parent state machine to change states.
    /// </summary>
    /// <param name="stateName">Name of the state to change to.</param>
    protected void RequestStateChange( StringName stateName ) {
        EmitSignal( SignalName.StateChangeRequest, stateName );
    }
    
    public override void _Ready() {
        if ( Engine.IsEditorHint() ) {
            XVIFuncs.SetNodeProcesses( this, false );
        }
    }
    
    /// <summary>
    /// Called by the state machine when this state is entered.
    /// </summary>
    public virtual void _EnterState() {}
    
    /// <summary>
    /// Called by the state machine when leaving this state.
    /// </summary>
    public virtual void _LeaveState() {}
    /// <summary>
    /// Tester for if this can swap to a new state.
    /// By default, this stops states from transitioning into themselves.
    /// </summary>
    /// <param name="state">The state this is moving into.</param>
    public virtual bool CanSwitchState(State state) => Name != state.Name;
    /// <summary>
    /// States are enabled by the state machine when in use.
    /// </summary>
    public virtual void _Enable() {}
    /// <summary>
    /// States are disabled by the state machine when not in use.
    /// </summary>
    public virtual void _Disable() {}
}