using System;

namespace AdventOfCode.Common;

/// <summary>
/// Represents a point of three integers.
/// </summary>
public readonly struct Point3 {

	/// <summary>
	/// The X position of this point.
	/// </summary>
	public int X { get; }
	/// <summary>
	/// The Y position of this point.
	/// </summary>
	public int Y { get; }

	/// <summary>
	/// The Y position of this point.
	/// </summary>
	public int Z { get; }



	/// <summary>
	/// Initializes a new <see cref="Point3"/> instance.
	/// </summary>
	/// <param name="x">The X position of the point.</param>
	/// <param name="y">The Y position of the point.</param>
	public Point3(int x, int y, int z) {
		X = x;
		Y = y;
		Z = z;
	}



	/// <summary>
	/// Rotates the point according to a rotation matrix.
	/// </summary>
	/// <param name="matrix">The rotation matrix to apply.</param>
	/// <returns>A new rotated <see cref="Point3"/>.</returns>
	public Point3 Rotate(int[][] matrix) {
		int x = matrix[1][1] * X + matrix[2][1] * Y + matrix[3][1] * Z;
		int y = matrix[1][2] * X + matrix[2][2] * Y + matrix[3][2] * Z;
		int z = matrix[1][3] * X + matrix[2][3] * Y + matrix[3][3] * Z;
		return new(x, y, z);
	}

	public bool Equals(Point3 other) =>
		X == other.X && Y == other.Y && Z == other.Z;
	public override bool Equals(object? obj) =>
		obj is Point3 p && Equals(p);
	public override int GetHashCode() =>
		HashCode.Combine(X, Y);
	public override string ToString() =>
		$"{X},{Y},{Z}";



	public static bool operator ==(Point3 a, Point3 b) =>
		a.Equals(b);
	public static bool operator !=(Point3 a, Point3 b) =>
		!a.Equals(b);

}
