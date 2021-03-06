namespace AdventOfCode.Common;

/// <summary>
/// Represents a solver for a puzzle calculating both its solutions simultaniously.
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
	/// <returns>A <see cref="CombinedSolution"/> instance
	/// containing the combined solution of the puzzle.</returns>
	CombinedSolution Solve(string input);

}

/// <summary>
/// Represents a solver for a puzzle calculating its solutions separately.
/// </summary>
/// <remarks>
/// Types implementing <see cref="ISplitSolver"/>
/// should also be attributed with <see cref="SolverAttribute"/>.
/// </remarks>
public interface ISplitSolver {

	/// <summary>
	/// Solves part 1 of the puzzle with a given input.
	/// </summary>
	/// <param name="input">The puzzle input.</param>
	/// <returns>The solution of part 1 of the puzzle.</returns>
	Part SolvePart1(string input);
	/// <summary>
	/// Solves part 2 of the puzzle with a given input.
	/// </summary>
	/// <param name="input">The puzzle input.</param>
	/// <returns>The solution of part 2 of the puzzle.</returns>
	Part SolvePart2(string input);

}
