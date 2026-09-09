using System;

namespace DragonXVI.GodotTemplate;

/// <summary>
/// A simple helper for 2D directions. Both cardinal and diagnal.
/// More of a hold over from my earlier days..,.,.
/// </summary>
public static class Direction2D {
    public const double Right = 0.0;
    public const double DownRight = double.Pi * 0.25;
    public const double Down = double.Pi * 0.5;
    public const double DownLeft = double.Pi * 0.75;
    public const double Left = double.Pi;
    public const double UpperLeft = double.Pi * 1.25;
    public const double Up = double.Pi * 1.5;
    public const double UpperRight = double.Pi * 1.75;
}