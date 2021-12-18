using System;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Common;

// https://github.com/WhiteBlackGoose/AoC2021/blob/main/Sources/Day4/Program.cs#L128
/// <summary>
/// A reader for reading and parsing a span of characters.
/// </summary>
public unsafe struct CharReader {

    private char* current;
    private readonly char* upper;



    /// <summary>
    /// Whether the reader is at the end of the string.
    /// </summary>
    public bool AtEnd => current >= upper;
	/// <summary>
	/// The <see cref="char"/> at the current position.
	/// </summary>
	public char Current => current < upper ? *current : '\0';



	/// <summary>
	/// Initializes a new <see cref="CharReader"/> instance.
	/// </summary>
	/// <param name="start">A pointer to the start of the span of character to read.</param>
	/// <param name="length">The length of the span to read.</param>

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public CharReader(char* start, int length) {
        current = start;
        upper = start + length;
    }



	/// <summary>
	/// Reads characters from the span until a digit is found or the span ends.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ExpectDigit() {
        while (current < upper && *current is (< '0' or > '9') and not '-')
            current++;
	}
	/// <summary>
	/// Reads characters from the span until the specified character is found or the span ends.
	/// </summary>
	/// <param name="c">The character to find.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ExpectChar(char c) {
		while (current < upper && *current != c)
			current++;
	}

	/// <summary>
	/// Reads the next <see cref="int"/> from the span.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int NextInt() {
		if (current >= upper) return 0;

		ExpectDigit();

		bool negative = *current == '-';
        if (negative) current++;

        int result = *current - '0';
        current++;

        while (current < upper && *current is >= '0' and <= '9') {
            result = result * 10 + current[0] - '0';
            current++;
        }

        if (negative) result *= -1;
        return result;
    }
	/// <summary>
	/// Reads the next <see cref="char"/> from the span.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public char NextChar() {
		if (current >= upper) return '\0';
		return *current++;
	}
	/// <summary>
	/// Moves to the next character.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void MoveNext() {
		if (current < upper) current++;
	}

	public override string? ToString() =>
		current < upper ? $"{(int)*current} '{*current}'" : "";

}
