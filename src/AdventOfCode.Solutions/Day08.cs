namespace AdventOfCode.Solutions;

[Solver(8, @"input\08.txt")]
public sealed class Day08 : ISplitSolver {

	private const StringSplitOptions removeTrim =
		StringSplitOptions.RemoveEmptyEntries |
		StringSplitOptions.TrimEntries;

	public Part SolvePart1(string input) {
		var entries = GetEntries(input);
		var outputs = entries
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

	/*
	 0000
	1    2
	1    2
	 3333
	4    5
	4    5
	 6666
	*/
	private static void GetNumbers(Entry entry) {
		var positions = new char[7];

		// Unique positions
		string d1 = entry.Input.First(d => d.Length == 2);
		string d7 = entry.Input.First(d => d.Length == 3);
		string d4 = entry.Input.First(d => d.Length == 4);
		string d8 = entry.Input.First(d => d.Length == 7);
		// 3 is the only 5-segment digit which contains all the segments of 7
		string d3 = entry.Input.Where(d => d.Length == 5)
			.First(d => d7.All(c => d.Contains(c)));

		// Segment 0 is the segment which 7 contains but not 1
		positions[0] = d7.First(c => !d1.Contains(c));
		// 6 is the only 6-segment digit which only contains a single segment present in 1
		// 0 and 9 both contain both segments in 1
		positions[5] = entry.Input.Where(d => d.Length == 6)
			.First(d => d.Count(c => d1.Contains(c)) == 1)
			.First(c => d1.Contains(c));
		// Segment 5 is the other segment in 1 which is not segment 5
		positions[2] = d1.First(c => c != positions[5]);
	}

	private readonly record struct Entry(string[] Input, string[] Output);

}
