namespace AdventOfCode.Solutions;

[Solver(14, @"input\14.txt")]
public sealed class Day14 : ISplitSolver {
	
	public Part SolvePart1(string input) {
		var parseResult = ParseInput(input);
		var frequencies = Mutate(parseResult.Template, parseResult.Rules, 10);
		ulong result = frequencies.Max() - frequencies.Min();
		return result;
	}
	public Part SolvePart2(string input) {
		var parseResult = ParseInput(input);
		var frequencies = Mutate(parseResult.Template, parseResult.Rules, 40);
		ulong result = frequencies.Max() - frequencies.Min();
		return result;
	}

	private static ParseResult ParseInput(string input) {
		ReadOnlySpan<char> template = default;
		Dictionary<(char, char), char> rules = new();

		var lines = input.AsSpan().EnumerateLines();
		bool parsingRules = false;
		foreach (var line in lines) {
			if (line.IsWhiteSpace()) {
				parsingRules = true;
				continue;
			}
			if (parsingRules) {
				var @in = (line[0], line[1]);
				var @out = line[6];
				rules.Add(@in, @out);
			}
			else template = line;
			
		}

		return new ParseResult { Template = template, Rules = rules };
	}
	
	private static IEnumerable<ulong> Mutate(ReadOnlySpan<char> template, IReadOnlyDictionary<(char, char), char> rules, int iterations) {
		Dictionary<char, ulong> frequencies = new();
		Dictionary<(char, char), ulong> pairFrequencies = new();

		for (int i = 0; i < template.Length; i++) {
			char current = template[i];
			if (!frequencies.ContainsKey(current))
				frequencies.Add(current, 0);
			frequencies[current]++;

			if (i < template.Length - 1) {
				char next = template[i + 1];
				var pair = (current, next);
				if (!pairFrequencies.ContainsKey(pair))
					pairFrequencies.Add(pair, 0);
				pairFrequencies[pair]++;
			}
		}

		for (int i = 0; i < iterations; i++) {
			Dictionary<(char, char), ulong> currentPairs = new(pairFrequencies);
			foreach (var p in currentPairs) {
				var pair = p.Key;
				ulong frequency = p.Value;
				char replace = rules[pair];
				var a = (pair.Item1, replace);
				var b = (replace, pair.Item2);

				if (!frequencies.ContainsKey(replace))
					frequencies.Add(replace, 0);
				frequencies[replace] += frequency;

				if (!pairFrequencies.ContainsKey(a))
					pairFrequencies.Add(a, 0);
				if (!pairFrequencies.ContainsKey(b))
					pairFrequencies.Add(b, 0);

				pairFrequencies[a] += frequency;
				pairFrequencies[b] += frequency;

				pairFrequencies[pair] -= frequency;
				if (pairFrequencies[pair] == 0)
					pairFrequencies.Remove(pair);
			}
		}

		return frequencies.Values;
	}



	// Tuples cannot contain spans (and record structs cannot be ref structs)
	private readonly ref struct ParseResult {
		public ReadOnlySpan<char> Template { get; init; }
		public IReadOnlyDictionary<(char, char), char> Rules { get; init; }
	}

}
