using System;
using System.Collections.Generic;

namespace AdventOfCode.Common;

/// <summary>
/// Represents a cuboid of integer points.
/// </summary>
public readonly struct Cuboid : IEquatable<Cuboid> {

	/// <summary>
	/// The minimum position of the cuboid.
	/// </summary>
	public Point3 Min { get; }
	/// <summary>
	/// The maximum position of the cuboid.
	/// </summary>
	public Point3 Max { get; }



	/// <summary>
	/// Initializes a new <see cref="Cuboid"/> instance.
	/// </summary>
	/// <param name="min">The minimum position of the cuboid.</param>
	/// <param name="max">The maximum position of the cuboid.</param>
	public Cuboid(Point3 min, Point3 max) {
		Min = min;
		Max = max;
	}
	/// <summary>
	/// Initializes a new <see cref="Cuboid"/> instance.
	/// </summary>
	/// <param name="xMin">The minimum x position of the cuboid.</param>
	/// <param name="yMin">The minimum y position of the cuboid.</param>
	/// <param name="zMin">The minimum y position of the cuboid.</param>
	/// <param name="xMax">The maximum x position of the cuboid.</param>
	/// <param name="yMax">The maximum y position of the cuboid.</param>
	/// <param name="zMax">The maximum y position of the cuboid.</param>
	public Cuboid(int xMin, int yMin, int zMin, int xMax, int yMax, int zMax) {
		Min = new(Math.Min(xMin, xMax), Math.Min(yMin, yMax), Math.Min(zMin, zMax));
		Max = new(Math.Max(xMin, xMax), Math.Max(yMin, yMax), Math.Max(zMin, zMax));
	}



	/// <summary>
	/// Determines whether a specified <see cref="Point3"/> is in this cuboid.
	/// </summary>
	/// <param name="point">The point to check.</param>
	/// <returns>Whether <paramref name="point"/> is within
	/// the current <see cref="Cuboid"/>.</returns>
	public bool PointInCuboid(Point3 point) =>
		point.X >= Min.X && point.X <= Max.X &&
		point.Y >= Min.Y && point.Y <= Max.Y &&
		point.Z >= Min.Z && point.Z <= Max.Z;
	/// <summary>
	/// Enumerates the points within the cuboid.
	/// </summary>
	/// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Point3"/> instances
	/// within the current <see cref="Cuboid"/>.</returns>
	public IEnumerable<Point3> EnumeratePoints() {
		for (int x = Min.X; x <= Max.X; x++)
			for (int y = Min.Y; y <= Max.Y; y++)
				for (int z = Min.Z; z <= Max.Z; z++)
					yield return new(x, y, z);
	}

	public bool Equals(Cuboid other) =>
		Min.Equals(other.Min) &&
		Max.Equals(other.Max);
	public override bool Equals(object? obj) =>
		obj is Cuboid Cube &&
		Equals(Cube);
	public override int GetHashCode() =>
		HashCode.Combine(Min, Max);
	public override string ToString() =>
		$"{Min} to {Max}";



	public static bool operator ==(Cuboid a, Cuboid b) =>
		a.Equals(b);
	public static bool operator !=(Cuboid a, Cuboid b) =>
		!a.Equals(b);

}
