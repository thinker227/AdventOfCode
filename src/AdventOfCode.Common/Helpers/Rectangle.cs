using System;

namespace AdventOfCode.Common;

/// <summary>
/// Represents a rectangle of integer points.
/// </summary>
public readonly struct Rectangle : IEquatable<Rectangle> {

    /// <summary>
    /// The minimum position of the rectangle.
    /// </summary>
    public Point Min { get; }
    /// <summary>
    /// The maximum position of the rectangle.
    /// </summary>
    public Point Max { get; }



    /// <summary>
    /// Initializes a new <see cref="Rectangle"/> instance.
    /// </summary>
    /// <param name="min">The minimum position of the rectangle.</param>
    /// <param name="max">The maximum position of the rectangle.</param>
    public Rectangle(Point min, Point max) {
        Min = min;
        Max = max;
    }
    /// <summary>
    /// Initializes a new <see cref="Rectangle"/> instance.
    /// </summary>
    /// <param name="xMin">The minimum x position of the rectangle.</param>
    /// <param name="yMin">The minimum y position of the rectangle.</param>
    /// <param name="xMax">The maximum x position of the rectangle.</param>
    /// <param name="yMax">The maximum y position of the rectangle.</param>
    public Rectangle(int xMin, int yMin, int xMax, int yMax) {
        Min = new(xMin, yMin);
        Max = new(xMax, yMax);
    }



    public bool Equals(Rectangle other) =>
        Min.Equals(other.Min) &&
        Max.Equals(other.Max);
    public override bool Equals(object? obj) =>
        obj is Rectangle rectangle &&
        Equals(rectangle);
    public override int GetHashCode() =>
        HashCode.Combine(Min, Max);
    public override string ToString() =>
        $"{Min} to {Max}";



    public static bool operator ==(Rectangle a, Rectangle b) =>
        a.Equals(b);
    public static bool operator !=(Rectangle a, Rectangle b) =>
        !a.Equals(b);

}
