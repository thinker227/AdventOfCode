namespace AdventOfCode.Solutions;

[Solver(1)]
public sealed class Day01 : ISolver {

	public Solution Solve(string? input) {
		var nums = input!
			.Split('\n')
			.Select(s => int.Parse(s.Trim()))
			.ToImmutableArray();

		int largerCount = 0;
		for (int i = 0; i < nums.Length; i++) {
			if (i == 0) continue;
			int current = nums[i];
			int previous = nums[i - 1];
			if (current > previous) largerCount++;
		}

		return new(largerCount.ToString());
	}

}
