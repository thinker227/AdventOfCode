//namespace AdventOfCode.Solutions;

[Solver(3, @"input\03.txt")]
public sealed class Day03 : ISolver {
    
    public Solution Solve(string? input) {
        var span = input!.Trim().AsSpan();
        int width = span[..(span.IndexOf('\n') + 1)].Length;
        int numersWidth = width - 2; // Width excluding control characters
        int length = (span!.Length + 2) / width; // Funky newline stuff
        int halfLength = length / 2;
        int[] nums = new int[width]; // Last two indicies are unused

        for (int i = 0; i < span.Length; i++) {
            nums[i % width] += span[i] / '1';
        }

        ushort gamma = 0;
        for (int i = 0; i < numersWidth; i++) {
            gamma <<= 1;
            if (nums[i] >= halfLength) // Assume inclusive
                gamma |= 1;
        }
        ushort epsilon = (ushort)(gamma ^ ushort.MaxValue);
        int result = gamma * epsilon;

        return new(result);
    }

}
