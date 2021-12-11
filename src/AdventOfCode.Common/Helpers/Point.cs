using System;
using System.Collections.Generic;

namespace AdventOfCode.Common;

/// <summary>
/// Represents a point of two integers.
/// </summary>
public readonly struct Point : IEquatable<Point> {

	/// <summary>
	/// The X position of this point.
	/// </summary>
	public int X { get; }
	/// <summary>
	/// The Y position of this point.
	/// </summary>
	public int Y { get; }



	/// <summary>
	/// Initializes a new <see cref="Point"/> instance.
	/// </summary>
	/// <param name="x">The X position of the point.</param>
	/// <param name="y">The Y position of the point.</param>
	public Point(int x, int y) {
		X = x;
		Y = y;
	}



	public bool Equals(Point other) =>
		X == other.X && Y == other.Y;
	public override bool Equals(object? obj) =>
		obj is Point p && Equals(p);
	public override int GetHashCode() =>
		HashCode.Combine(X, Y);
	public override string ToString() =>
		$"{X},{Y}";

	/// <summary>
	/// Generates a sequence of points in a grid.
	/// </summary>
	/// <param name="width">The width of the grid.</param>
	/// <param name="height">The height of the grid.</param>
	/// <returns>An <see cref="IEnumerable{T}"/> of points.</returns>
	public static IEnumerable<Point> GetPointGrid(int width, int height) {
		for (int x = 0; x < width; x++)
			for (int y = 0; y < height; y++)
				yield return new(x, y);
	}



	public static bool operator ==(Point a, Point b) =>
		a.Equals(b);
	public static bool operator !=(Point a, Point b) =>
		!a.Equals(b);

}
