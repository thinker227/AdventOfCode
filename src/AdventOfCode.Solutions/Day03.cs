namespace AdventOfCode.Solutions;

[Solver(3, @"input\03.txt")]
public sealed class Day03 : IDualSolver {
    
    public string? SolvePart1(string? input) {
        var span = input!.Trim().AsSpan();
        int width = span[..(span.IndexOf('\n') + 1)].Length; // Width of each line
        int bitCount = width - 2; // Amount of bits in each number (width excluding control characters)
        int length = (span!.Length + 2) / width; // Funky newline stuff
        int halfLength = length / 2;
        int[] nums = new int[width]; // Last two indicies are unused

        for (int i = 0; i < span.Length; i++)
            // Any character with an ascii value under 49 ('1') will result in adding 0
            nums[i % width] += span[i] / '1';

        ushort gamma = 0;
        for (int i = 0; i < bitCount; i++) {
            gamma <<= 1;
            if (nums[i] >= halfLength) // Assume inclusive
                gamma |= 1;
        }
        var bitSize = sizeof(ushort) * 8;
        // The largest value formed by a number with an amount of bits equal to bitCount
        ushort largest = (ushort)(ushort.MaxValue >> (bitSize - bitCount));
        ushort epsilon = (ushort)(gamma ^ largest);
        int result = gamma * epsilon;

		return result.ToString();
    }

	public string? SolvePart2(string? input) {
		throw new NotImplementedException();
	}

}
