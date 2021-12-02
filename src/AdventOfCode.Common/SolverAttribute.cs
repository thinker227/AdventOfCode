using System;

namespace AdventOfCode.Common;

/// <summary>
/// Indicates that a type is a solver.
/// </summary>
/// <remarks>
/// Types attributed with <see cref="SolverAttribute"/>
/// should also implement <see cref="ISolver"/>.
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = true, AllowMultiple = false)]
public sealed class SolverAttribute : Attribute {

	/// <summary>
	/// The day of the puzzle the solver solves.
	/// </summary>
	public int Day { get; }
	/// <summary>
	/// The file path to the input file of the solver.
	/// </summary>
	public string InputPath { get; }



	/// <summary>
	/// Initializes a new <see cref="SolverAttribute"/> instance.
	/// </summary>
	/// <param name="day">The day of the puzzle the solver solves.</param>
	/// <param name="inputPath">The file path to the input file of the solver.</param>
	public SolverAttribute(int day, string inputPath) {
		Day = day;
		InputPath = inputPath;
	}

}
