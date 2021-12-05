namespace AdventOfCode.Solutions;

[Solver(2, @"input\02.txt")]
public sealed class Day02 : ISolver {

	public CombinedSolution Solve(string input) {
		var span = input.AsSpan();
		var lines = span.EnumerateLines();

		// depth is part 1, accurateDepth is part 2
		var (horizontal, depth, accurateDepth, aim) = (0, 0, 0, 0);

		foreach (var line in lines) {
			var space = line.IndexOf(' ');
			var number = int.Parse(line[(space + 1)..]);
			if (space == 2) number = -number;

			if (space == 7) {
				horizontal += number;
				accurateDepth += aim * number;
			}
			else {
				aim += number;
				depth += number;
			}
		}

		return new(horizontal * depth, horizontal * accurateDepth);
	}

}
