using Godot;

namespace DragonXVI;

/// <summary>
/// Helper class for godot dictionaries returned by ray cast quiries.
/// </summary>
public static class RayDict
{
    /// <summary>
    /// The colliding object, usually a Node2D (TileMapLayer or CollisionObject2D).
    /// </summary>
    public static readonly StringName Collider = "collider";
    /// <summary>
    /// The colliding objects id.
    /// Unsure what this means.,.,.
    /// </summary>
    public static readonly StringName ColliderID = "collider_id";
    /// <summary>
    /// Normal vector of a collision.
    /// Can be Vector2.Zero if the collision happens inside an object.
    /// </summary>
    public static readonly StringName Normal = "normal";
    /// <summary>
    /// Global position of a collision.
    /// </summary>
    public static readonly StringName Position = "position";
    /// <summary>
    /// Rid of the hit object.
    /// </summary>
    public static readonly StringName Rid = "rid";
    /// <summary>
    /// Shape index of the hit collider.
    /// </summary>
    public static readonly StringName Shape = "shape";
}
