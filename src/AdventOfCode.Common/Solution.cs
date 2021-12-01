namespace AdventOfCode.Common;

/// <summary>
/// Represents a solution to a puzzle.
/// </summary>
public readonly struct Solution {

	/// <summary>
	/// The solution to part 1 of the puzzle.
	/// </summary>
	public string? Part1 { get; }
	/// <summary>
	/// The solution to part 2 of the puzzle.
	/// </summary>
	public string? Part2 { get; }



	/// <summary>
	/// Initializes a new <see cref="Solution"/> instance.
	/// </summary>
	/// <param name="part1">The solution to part 1 of the puzzle.</param>
	/// <param name="part2">The solution to part 2 of the puzzle.</param>
	public Solution(string? part1, string? part2) {
		Part1 = part1;
		Part2 = part2;
	}
	/// <summary>
	/// Initializes a new <see cref="Solution"/> instance.
	/// </summary>
	/// <param name="part1">The solution to part 1 of the puzzle.</param>
	public Solution(string? part1) {
		Part1 = part1;
		Part2 = null;
	}
	/// <summary>
	/// Initializes a new <see cref="Solution"/> instance.
	/// </summary>
	/// <param name="part1">The solution to part 1 of the puzzle.</param>
	/// <param name="part2">The solution to part 2 of the puzzle.</param>
	public Solution(int? part1, int? part2) {
		Part1 = part1?.ToString();
		Part2 = part2?.ToString();
	}
	/// <summary>
	/// Initializes a new <see cref="Solution"/> instance.
	/// </summary>
	/// <param name="part1">The solution to part 1 of the puzzle.</param>
	public Solution(int? part1) {
		Part1 = part1?.ToString();
		Part2 = null;
	}

}
