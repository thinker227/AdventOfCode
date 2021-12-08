namespace AdventOfCode.Solutions;

[Solver(8, @"input\08.txt")]
public sealed class Day08 : ISplitSolver {

	private const StringSplitOptions removeTrim =
		StringSplitOptions.RemoveEmptyEntries |
		StringSplitOptions.TrimEntries;

	public Part SolvePart1(string input) {
		var entries = GetEntries(input);
		var outputs = entries
			.SelectMany(e => e.output);
			.SelectMany(e => e.Output);
		int result = outputs.Count(d => d.Length is 2 or 3 or 4 or 7);
		return result;
	}
	public Part SolvePart2(string input) {
		throw new NotImplementedException();
	}

	private static IEnumerable<Entry> GetEntries(string s) {
		return s.Split('\n')
			.Select(e => {
				var split = e.Split('|', removeTrim);
				return new Entry(SplitOnEmpty(split[0]),
					SplitOnEmpty(split[1]));
			});
	}
	private static string[] SplitOnEmpty(string s) =>
		s.Split(' ', removeTrim);
	
	private readonly record struct Entry(string[] Input, string[] Output);

}
