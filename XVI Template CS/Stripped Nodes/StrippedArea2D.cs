using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace DragonXVI.Stripped;

/// <summary>
/// An Area2D with some properies disabled, so they can be enabled in code instead.
/// 
/// NOTE: Disabled properties are set to their OFF state. E.g. Monitoring/able is false, and collision layer/mask is 0.
/// </summary?
[GlobalClass, Tool]
public abstract partial class StrippedArea2D : Area2D {
    // arrays piss me off.,,.
    private static readonly List<string> DisabledProperties = [
        PropertyName.Monitoring,
        PropertyName.Monitorable,
        PropertyName.CollisionLayer,
        PropertyName.CollisionMask,
        PropertyName.InputPickable,
        PropertyName.ZIndex,
    ];
    
    public StrippedArea2D() {
        Monitoring = false;
        Monitorable = false;
        CollisionLayer = 0;
        CollisionMask = 0;
        InputPickable = false;
        
        if ( Engine.IsEditorHint() ){
            ChildEnteredTree += OnChildEnteredTree;
            return;
        }
    }
    
    public override void _Ready()
    {
        if ( Engine.IsEditorHint() ) {
            XVIFuncs.SetNodeProcesses( this, false );
            return;
        }
    }
    public override void _ValidateProperty( Dictionary property ) {
        if ( DisabledProperties.Contains( (string)property[ Property.Name ] ) ) {
            property[ Property.Usage ] = (long)PropertyUsageFlags.None;
        }
    }
    
    /// <summary>
    /// Runs in the editor only.
    /// </summary>
    private void OnChildEnteredTree( Node node ) {
        if ( node is CollisionShape2D shape ) {
            ShapeEnteredTree( shape );
        }
    }
    
    /// <summary>
    /// Shortcut for a collision shape child entering the tree.
    /// I like to use this to change its debug color.
    /// </summary>
    protected virtual void ShapeEnteredTree( CollisionShape2D shape ) {}
}