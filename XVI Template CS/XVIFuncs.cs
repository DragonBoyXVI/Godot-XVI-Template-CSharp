using System.Threading.Tasks;
using Godot;
using Godot.Collections;

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
    public static void SetNodeProcesses( Node node, bool enabled = false ) {
        node.SetProcess( enabled );
        node.SetPhysicsProcess( enabled );
        node.SetProcessInput( enabled );
        node.SetProcessShortcutInput( enabled );
        node.SetProcessUnhandledInput( enabled );
        node.SetProcessUnhandledKeyInput( enabled );
    }
    
    /// <summary>
    /// A neat wrapper for the functions in [ResourceLoader].
    /// A standardized way to load a resource on a thread using await.
    /// 
    /// All of the arguments are the same as the resource loader thread ones, including the progress array.
    /// More of a test than anything, not gdscript friendly qwqqqq
    /// </summary>
    public static async Task<Resource> LoadResourceCoroutine( string resourcePath, string typeHint = "", Array progress = [] )
    {
        Error err = ResourceLoader.LoadThreadedRequest( resourcePath, typeHint );
        if ( err != Error.Ok ) {
            GD.PushError( $"Could not load resource {resourcePath} on thread: {err}" );
            return null;
        }
        
        while (true) {
            var loadStatus = ResourceLoader.LoadThreadedGetStatus( resourcePath, progress );
            if ( loadStatus == ResourceLoader.ThreadLoadStatus.Loaded ) {
                break;
            } else if ( loadStatus == ResourceLoader.ThreadLoadStatus.InProgress ) {
                var tree = (SceneTree)Engine.GetMainLoop();
                await tree.ToSignal( tree, SceneTree.SignalName.ProcessFrame );
            } else {
                return null;
            }
        }
        
        return ResourceLoader.LoadThreadedGet( resourcePath );
    }
}
