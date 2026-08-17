using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace DragonXVI.Timers;

/// <summary>
/// Base class for custom timers.
/// </summary>
[Tool, GlobalClass]
public abstract partial class XVITimer : Timer
{
		private static readonly List<string> DisabledProperties = [
			Timer.PropertyName.WaitTime,
			Timer.PropertyName.Autostart,
		];

    /// <summary>
    /// Same as autostart in the timer class, but i have more control over this one.
    /// If true, the timer starts itself when in the tree.
    /// </summary>
    [Export]
    public bool StartAutomatically = false;

    /// <summary>
    /// Godot rng object used to control the random state of this.
    /// If not provided then itll use the global random functions.
    /// </summary>
    public RandomNumberGenerator Rng = null;

    public override void _Ready()
    {
			Timeout += OnSelfTimeout;
    }
    public override void _ValidateProperty(Dictionary property)
    {
			if ( DisabledProperties.Contains( (string)property[ Property.Name ] ) ) {
            property[Property.Usage] = (long)PropertyUsageFlags.None;
        }
    }
    public override void _EnterTree()
    {
        if ( Engine.IsEditorHint() ){
            return;
        }

			if ( StartAutomatically ) {
            CallDeferred(MethodName.StartExt);
        }
    }

		/// <summary>
		/// Function called to start the special behaviour of this timer.
		/// </summary>
    public abstract void StartExt();

    protected void OnSelfTimeout()
    {
        if (!OneShot)
        {
            StartExt();
        }
    }
}
