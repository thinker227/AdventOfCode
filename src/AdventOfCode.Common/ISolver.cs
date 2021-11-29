using System;

namespace AdventOfCode.Common;

/// <summary>
/// Represents a solver for a puzzle.
/// </summary>
/// <remarks>
/// Types implementing <see cref="ISolver"/>
/// should also be attributed with <see cref="SolverAttribute"/>.
/// </remarks>
public interface ISolver {

	/// <summary>
	/// Solves the puzzle with a given input.
	/// </summary>
	/// <param name="input">The puzzle input.</param>
	/// <returns>A <see cref="Solution"/> instance
	/// containing the solution of the puzzle.</returns>
	Solution Solve(string? input);

}
