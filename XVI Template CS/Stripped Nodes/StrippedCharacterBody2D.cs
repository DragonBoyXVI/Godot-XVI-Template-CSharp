using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace DragonXVI.Stripped;

/// <summary>
/// A character body with some properties disabled, so they can be enabled in code instead.
/// 
/// Note that all disabled properties are set to 0, while others are default.
/// (e.g. MotionMode is Grounded).
/// </summary>
[GlobalClass, Tool]
public abstract partial class StrippedCharacterBody2D : CharacterBody2D
{
    private static readonly List<string> DisabledProperties = [
        PropertyName.MotionMode,
        PropertyName.CollisionLayer,
        PropertyName.CollisionMask,
        PropertyName.InputPickable,
        PropertyName.ZIndex,
    ];
    
    public StrippedCharacterBody2D() {
        CollisionLayer = 0;
        CollisionMask = 0;
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
}
