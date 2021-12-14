namespace AdventOfCode.Solutions;

[Solver(14, @"input\14.txt")]
public sealed class Day14 : ISolver {
	
	public CombinedSolution Solve(string input) {
		(var template, var rules) = ParseInput(input);
		IList<char> chars = template;
		for (int i = 0; i < 10; i++)
			chars = Mutate(chars, rules);

		var frequencies = chars.GroupBy(x => x).Select(x => x.Count());
		int result = frequencies.Max() - frequencies.Min();
		return result;
	}

	private static (IList<char>, IDictionary<(char, char), char>) ParseInput(string input) {
		IList<char> list = null!;
		Dictionary<(char, char), char> dictionary = new();
		var span = input.AsSpan();

		bool parsingRules = false;
		foreach (var line in span.EnumerateLines()) {
			if (line.IsWhiteSpace()) {
				parsingRules = true;
				continue;
			}
			if (parsingRules) {
				var @in = (line[0], line[1]);
				var @out = line[6];
				dictionary.Add(@in, @out);

			} else {
				list = line.ToArray().ToList();
			}
		}

		return (list, dictionary);
	}
	
	private static IList<char> Mutate(IList<char> input, IDictionary<(char, char), char> rules) {
		List<char> mutated = new();

		for (int i = 0; i < input.Count - 1; i++) {
			var current = (input[i], input[i + 1]);
			if (rules.TryGetValue(current, out char replace)) {
				mutated.Add(current.Item1);
				mutated.Add(replace);
			}
		}
		mutated.Add(input[^1]);

		return mutated;
	}

}
