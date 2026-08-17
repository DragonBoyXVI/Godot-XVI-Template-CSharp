using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace DragonXVI.Stripped
{
    /// <summary>
    /// A static body with some properties disabled, so they can be enabled in code instead.
    /// 
    /// Note that all disabled properties are set to 0.
    /// </summary>
    [GlobalClass, Tool]
    public abstract partial class StrippedStaticBody2D : StaticBody2D
    {
        private static readonly List<string> DisabledProperties = [
            CollisionObject2D.PropertyName.CollisionLayer,
            CollisionObject2D.PropertyName.CollisionMask,
            CollisionObject2D.PropertyName.InputPickable,
            CanvasItem.PropertyName.ZIndex,
        ];

        public StrippedStaticBody2D()
        {
            CollisionLayer = 0;
            CollisionMask = 0;
            InputPickable = false;
        }

        public override void _Ready()
        {
            if (Engine.IsEditorHint())
            {
                XVIFuncs.SetNodeProcesses(this, false);
                return;
            }
        }
        public override void _ValidateProperty(Dictionary property)
        {
            if (DisabledProperties.Contains((string)property[Property.Name]))
            {
                property[Property.Usage] = (long)PropertyUsageFlags.None;
            }
        }
    }
}
