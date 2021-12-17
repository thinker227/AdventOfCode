using System;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Common;

// https://github.com/WhiteBlackGoose/AoC2021/blob/main/Sources/Day4/Program.cs#L128
/// <summary>
/// A reader for reading and parsing a span of characters into integers.
/// </summary>
public unsafe struct IntReader {

    private char* current;
    private readonly char* upper;



    /// <summary>
    /// Whether the reader is at the end of the string.
    /// </summary>
    public bool AtEnd => current >= upper;



	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	/// <summary>
	/// Initializes a new <see cref="IntReader"/> instance.
	/// </summary>
	/// <param name="start">A pointer to the start of the span of character to read.</param>
	/// <param name="length">The length of the span to read.</param>
	public IntReader(char* start, int length) {
        current = start;
        upper = start + length;
        JumpToNextDigit();
    }



	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void JumpToNextDigit() {
        while (current < upper && current[0] is (< '0' or > '9') and not '-')
            current++;
    }

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
    /// <summary>
    /// Reads the next int from the span.
    /// </summary>
    public int Next() {
        bool negative = current[0] == '-';
        if (negative) current++;

        int result = current[0] - '0';
        current++;

        while (current < upper && current[0] is >= '0' and <= '9') {
            result = result * 10 + current[0] - '0';
            current++;
        }

        JumpToNextDigit();

        if (negative) result *= -1;
        return result;
    }

}
