namespace AdventOfCode.Solutions;

[Solver(2, @"input\02.txt")]
public sealed class Day02 : ISolver {

	public Solution Solve(string? input) {
		var span = input!.AsSpan();
		var lines = span.EnumerateLines();
		var (horizontal, depth) = (0, 0);

		foreach (var line in lines) {
			var space = line.IndexOf(' ');
			var number = int.Parse(line[(space + 1)..^0]);
			if (space == 2) number = -number;

			if (space == 7) horizontal += number;
			else depth += number;
		}

		return new(horizontal * depth);
	}

}
