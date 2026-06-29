using Godot;

namespace DragonXVI;

/// <summary>
/// A class that holds utility functions.
/// </summary>
[GlobalClass, Tool]
public abstract partial class XVIFuncs : GodotObject
{
    
    /// <summary>
    /// Sets ALL processes on a node to enabled or disabled, depending on the "enabled" argument.
    /// By default, this disables all node processes.
    /// </summary>
    static void SetNodeProcesses( Node node, bool enabled = false ) {
        node.SetProcess( enabled );
        node.SetPhysicsProcess( enabled );
        node.SetProcessInput( enabled );
        node.SetProcessShortcutInput( enabled );
        node.SetProcessUnhandledInput( enabled );
        node.SetProcessUnhandledKeyInput( enabled );
    }
}
