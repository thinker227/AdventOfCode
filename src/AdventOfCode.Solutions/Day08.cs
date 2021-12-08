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
		var entries = GetEntries(input);
		var nums = entries
			.Select(e => GetNumber(e, GetDigits(e)));
		int result = nums.Sum();

		return result;
	}

	private static IEnumerable<Entry> GetEntries(string s) {
		return s.Split('\n')
			.Select(e => {
				var split = e.Split('|', removeTrim);
				var input = SplitOnEmpty(split[0]);
				var output = SplitOnEmpty(split[1]);
				return new Entry(input, output);
			}).ToArray();
	}
	private static string[] SplitOnEmpty(string s) =>
		s.Split(' ', removeTrim)
		.Select(s => string.Concat(s.OrderBy(c => c)))
		.ToArray();

	/*
	 0000
	1    2
	1    2
	 3333
	4    5
	4    5
	 6666
	*/
	private static string[] GetDigits(Entry entry) {
		var segments = new char[7];
		var digits = new string[10];

		// Unique positions
		digits[1] = entry.Input.First(d => d.Length == 2);
		digits[7] = entry.Input.First(d => d.Length == 3);
		digits[4] = entry.Input.First(d => d.Length == 4);
		digits[8] = entry.Input.First(d => d.Length == 7);

		var segment5s = entry.Input.Where(d => d.Length == 5);
		// 3 is the only 5-segment digit which contains all the segments of 7
		digits[3] = segment5s
			.First(d => digits[7].All(c => d.Contains(c)));

		// Segment 0 is the segment which 7 contains but not 1
		segments[0] = digits[7].First(c => !digits[1].Contains(c));
		// 6 is the only 6-segment digit which only contains a single segment present in 1
		// 0 and 9 both contain both segments in 1
		segments[5] = entry.Input.Where(d => d.Length == 6)
			.First(d => d.Count(c => digits[1].Contains(c)) == 1)
			.First(c => digits[1].Contains(c));
		// Segment 5 is the other segment in 1 which is not segment 5
		segments[2] = digits[1].First(c => c != segments[5]);

		// 2 is the only digit which contains segment 2 but not 5
		digits[2] = segment5s
			.First(d => d.Contains(segments[2]) && !d.Contains(segments[5]));
		// 5 is the only digit which contains segment 5 but not 2
		digits[5] = segment5s
			.First(d => d.Contains(segments[5]) && !d.Contains(segments[2]));

		// Segment 3 is the only segment which is contained within both 2 and 4 and is not segment 2
		segments[3] = digits[2].First(c => c != segments[2] && digits[4].Contains(c));
		
		var segment6s = entry.Input.Where(d => d.Length == 6);
		// 6 is the only 6-segment digit which does not contain segment 2
		digits[6] = segment6s
			.First(d => !d.Contains(segments[2]));
		// 0 is the only digit which does not contain segment 3 and is not 0
		digits[0] = segment6s
			.First(d => d != digits[6] && !d.Contains(segments[3]));
		// 0 is the only digit which contains segment 3 and is not 0
		digits[9] = segment6s
			.First(d => d != digits[6] && d.Contains(segments[3]));

		return digits;
	}
	private static int GetNumber(Entry entry, string[] digits) {
		int result = 0;
		foreach (string s in entry.Output) {
			int num = digits.IndexOf(s);
			result = result * 10 + num;
		}
		return result;
	}



	private readonly record struct Entry(string[] Input, string[] Output);

}
