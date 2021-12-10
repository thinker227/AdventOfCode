namespace AdventOfCode.Solutions;

[Solver(10, @"input\10.txt")]
public sealed class Day10 : ISolver {
	
	public CombinedSolution Solve(string input) {
		var span = input.AsSpan();
		Stack<char> expected = new();
		Span<int> errors = stackalloc int[4];

		foreach (var line in span.EnumerateLines()) {
			expected.Clear();

			foreach (char c in line) {
				if (IsOpen(c)) {
					expected.Push(GetPair(c));
					continue;
				}
				if (c != expected.Pop())
					errors[GetRank(c)]++;
			}
		}

		int total =
			errors[0] * 3 +
			errors[1] * 57 +
			errors[2] * 1197 +
			errors[3] * 25137;
		return new(total);
	}

	private static bool IsOpen(char c) =>
		c switch {
			'(' or '[' or '{' or '<' => true,
			_ => false
		};
	private static char GetPair(char c) =>
		c switch {
			'(' => ')',
			'[' => ']',
			'{' => '}',
			'<' => '>',
			_ => '\0'
		};
	private static int GetRank(char c) =>
		c switch {
			')' => 0,
			']' => 1,
			'}' => 2,
			'>' => 3,
			_ => -1
		};
	
}
