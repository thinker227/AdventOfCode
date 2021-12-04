namespace AdventOfCode.Solutions;

[Solver(1, @"input\01.txt")]
public sealed class Day01 : ISolver {

	public Solution Solve(string input) {
		var nums = input
			.Split('\n', StringSplitOptions.RemoveEmptyEntries)
			.Select(s => int.Parse(s.Trim()))
			.View(3)
			.Select(s => s.ToImmutableArray());
		var first = nums.First();

		int p1 = 0; // Solution to part 1 only comparing individual elements
		int p2 = 0;
		IEnumerable<int>? previous = null;

		if (first[1] > first[0]) p1++; // The first grouping has to be specially treated
		foreach (var grouping in nums) {
			if (grouping[2] > grouping[1]) p1++;
			if (grouping.Sum() > previous?.Sum()) p2++;
			previous = grouping;
		}

		return new(p1, p2);
	}

}
