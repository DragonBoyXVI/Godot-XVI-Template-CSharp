using Godot;

namespace DragonXVI.GodotTemplate;

/// <summary>
/// Helper class for property dictionaries.
/// Like those used for _ValidateProperty.
/// </summary>
public static class Property {
    /// <summary>
    /// Name of the property.
    /// </summary>
    public static readonly StringName Name = "name";
    
    /// <summary>
    /// String name of the BUILT IN class.
    /// Only used if the property type is TYPE_OBJECT.
    /// </summary>
    public static readonly StringName ClassName = "class_name";
    /// <summary>
    /// The Variant.Type of this property.
    /// </summary>
    public static readonly StringName Type = "type";
    /// <summary>
    /// Determines how the editor displays and edits this property.
    /// </summary>
    public static readonly StringName Hint = "hint";
    /// <summary>
    /// Used to provide hint information, depending on the hint.
    /// </summary>
    public static readonly StringName HintString = "hint_string";
    /// <summary>
    /// How the property is used (e.g. using this as a group, rather than a property).
    /// </summary>
    public static readonly StringName Usage = "usage";
}