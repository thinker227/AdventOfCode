namespace AdventOfCode.Solutions;

[Solver(8, @"input\08.txt")]
public sealed class Day08 : ISolver {

	private const StringSplitOptions removeTrim =
		StringSplitOptions.RemoveEmptyEntries |
		StringSplitOptions.TrimEntries;

	public CombinedSolution Solve(string input) {
		var entries = input.Split('\n')
			.Select(e => {
				var split = e.Split('|');
				return (input: split[0], output: split[1]);
			});
		var outputs = entries
			.SelectMany(e => e.output
				.Split(' ', removeTrim));
		int result = outputs.Count(d => d.Length is 2 or 3 or 4 or 7);
		return new(result);
	}
	
}
