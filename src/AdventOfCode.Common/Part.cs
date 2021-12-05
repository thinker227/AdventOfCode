using System;

namespace AdventOfCode.Common;

/// <summary>
/// Represents the type of solution to a <see cref="Part"/>.
/// </summary>
public enum PartType {
	/// <summary>
	/// The <see cref="Part"/> does not contain a solution.
	/// </summary>
	None,
	/// <summary>
	/// <see cref="Part.CharSpan"/> is set.
	/// </summary>
	CharSpan,
	/// <summary>
	/// <see cref="Part.Integer"/> is set.
	/// </summary>
	Int
}

/// <summary>
/// Represents the solution to a single part of a puzzle.
/// </summary>
public readonly ref struct Part {

	/// <summary>
	/// The type of part this <see cref="Part"/> represents.
	/// </summary>
	public PartType Type { get; } = PartType.None;
	/// <summary>
	/// Whether the part has a solution.
	/// </summary>
	public bool HasSolution => Type != PartType.None;
	/// <summary>
	/// The char span representation of this part.
	/// </summary>
	public ReadOnlySpan<char> CharSpan { get; } = default;
	/// <summary>
	/// The integer representation of this part.
	/// </summary>
	public int Integer { get; } = default;



	/// <summary>
	/// Initializes a new <see cref="Part"/> instance.
	/// </summary>
	/// <param name="charSpan">The char span representation of the part.</param>
	public Part(ReadOnlySpan<char> charSpan) {
		Type = PartType.CharSpan;
		CharSpan = charSpan;
	}
	/// <summary>
	/// Initializes a new <see cref="Part"/> instance.
	/// </summary>
	/// <param name="integer">The integer representation of the part.</param>
	public Part(int integer) {
		Type = PartType.Int;
		Integer = integer;
	}



	/// <summary>
	/// Gets a string representation of this <see cref="Part"/>.
	/// </summary>
	/// <returns>A string constructed from either
	/// <see cref="CharSpan"/> or <see cref="Integer"/>
	/// depending on the value of <see cref="Type"/>.</returns>
	/// <exception cref="InvalidOperationException">
	/// <see cref="Type"/> is outside the range of <see cref="PartType"/>.
	/// </exception>
	public override string? ToString() =>
		Type switch {
			PartType.None => null,
			PartType.CharSpan => new string(CharSpan),
			PartType.Int => Integer.ToString(),
			_ => throw new InvalidOperationException()
		};



	/// <summary>
	/// Implicitly converts a <see cref="ReadOnlySpan{T}"/>
	/// of <see cref="char"/> to a <see cref="Part"/> instance.
	/// </summary>
	/// <param name="charSpan">The char span representation of the part.</param>
	public static implicit operator Part(ReadOnlySpan<char> charSpan) =>
		new(charSpan);
	/// <summary>
	/// Implicitly converts a <see cref="string"/> to a <see cref="Part"/> instance.
	/// </summary>
	/// <param name="string">The char span representation of the part.</param>
	public static implicit operator Part(string @string) =>
		new(@string.AsSpan());
	/// <summary>
	/// Implicitly converts an <see cref="int"/> to a <see cref="Part"/> instance.
	/// </summary>
	/// <param name="integer">The integer representation of the part.</param>
	public static implicit operator Part(int integer) =>
		new(integer);

}

/// <summary>
/// Represents the combined solution of two parts of a puzzle.
/// </summary>
public readonly ref struct CombinedSolution {

	/// <summary>
	/// The solution to part 1 of the puzzle.
	/// </summary>
	public Part Part1 { get; } = default;
	/// <summary>
	/// The solution to part 2 of the puzzle.
	/// </summary>
	public Part Part2 { get; } = default;
	/// <summary>
	/// Whether <see cref="Part1"/> is set.
	/// </summary>
	public bool HasPart1 { get; } = false;
	/// <summary>
	/// Whether <see cref="Part2"/> is set.
	/// </summary>
	public bool HasPart2 { get; } = false;



	/// <summary>
	/// Initializes a new <see cref="CombinedSolution"/> instance.
	/// </summary>
	/// <param name="part1">The solution to part 1.</param>
	public CombinedSolution(Part part1) {
		Part1 = part1;
		HasPart1 = true;
	}
	/// <summary>
	/// Initializes a new <see cref="CombinedSolution"/> instance.
	/// </summary>
	/// <param name="part1">The solution to part 1.</param>
	/// <param name="part2">The solution to part 2.</param>
	public CombinedSolution(Part part1, Part part2) {
		Part1 = part1;
		Part2 = part2;
		HasPart1 = true;
		HasPart2 = true;
	}
	/// <summary>
	/// Initializes a new <see cref="CombinedSolution"/> instance.
	/// </summary>
	/// <param name="part1">The char span representation of the solution to part 1.</param>
	public CombinedSolution(ReadOnlySpan<char> part1) {
		Part1 = new(part1);
		HasPart1 = true;
	}
	/// <summary>
	/// Initializes a new <see cref="CombinedSolution"/> instance.
	/// </summary>
	/// <param name="part1">The integer representation of the solution to part 1.</param>
	public CombinedSolution(int part1) {
		Part1 = new(part1);
		HasPart1 = true;
	}
	/// <summary>
	/// Initializes a new <see cref="CombinedSolution"/> instance.
	/// </summary>
	/// <param name="part1">The char span representation of the solution to part 1.</param>
	/// <param name="part2">The char span representation of the solution to part 2.</param>
	public CombinedSolution(ReadOnlySpan<char> part1, ReadOnlySpan<char> part2) {
		Part1 = new(part1);
		Part2 = new(part2);
		HasPart1 = true;
		HasPart2 = true;
	}
	/// <summary>
	/// Initializes a new <see cref="CombinedSolution"/> instance.
	/// </summary>
	/// <param name="part1">The char span representation of the solution to part 1.</param>
	/// <param name="part2">The integer representation of the solution to part 2.</param>
	public CombinedSolution(ReadOnlySpan<char> part1, int part2) {
		Part1 = new(part1);
		Part2 = new(part2);
		HasPart1 = true;
		HasPart2 = true;
	}
	/// <summary>
	/// Initializes a new <see cref="CombinedSolution"/> instance.
	/// </summary>
	/// <param name="part1">The integer representation of the solution to part 1.</param>
	/// <param name="part2">The char span representation of the solution to part 2.</param>
	public CombinedSolution(int part1, ReadOnlySpan<char> part2) {
		Part1 = new(part1);
		Part2 = new(part2);
		HasPart1 = true;
		HasPart2 = true;
	}
	/// <summary>
	/// Initializes a new <see cref="CombinedSolution"/> instance.
	/// </summary>
	/// <param name="part1">The integer representation of the solution to part 1.</param>
	/// <param name="part2">The integer representation of the solution to part 2.</param>
	public CombinedSolution(int part1, int part2) {
		Part1 = new(part1);
		Part2 = new(part2);
		HasPart1 = true;
		HasPart2 = true;
	}

}
