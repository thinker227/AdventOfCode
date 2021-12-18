namespace AdventOfCode.Solutions;

[Solver(18, @"input\18.txt")]
public sealed class Day18 : ISolver {
	
	public CombinedSolution Solve(string input) {
		var pairs = ParseInput(input);
		throw new TooLazyException();
		//var sum = Sum(pairs);
		//var magnitude = Magnitude(sum);
		//return magnitude;
	}

	private static unsafe IList<Pair> ParseInput(string input) {
		List<Pair> pairs = new();

		fixed (char* c = input) {
			CharReader reader = new(c, input.Length);
			
			while (!reader.AtEnd) {
				var pair = ParsePair(ref reader);
				pairs.Add(pair);

				reader.ExpectChar('\n');
			}
		}

		return pairs;
	}
	private static Pair ParsePair(ref CharReader reader) {
		reader.ExpectChar('[');
		reader.MoveNext();

		Union<int, Pair> x;
		Union<int, Pair> y;
		if (reader.Current == '[') x = ParsePair(ref reader);
		else x = reader.NextInt();
		reader.ExpectChar(',');
		reader.MoveNext();
		if (reader.Current == '[') y = ParsePair(ref reader);
		else y = reader.NextInt();

		return new(x, y);
	}

	private static Pair Sum(IList<Pair> pairs) {
		while (pairs.Count > 1) {
			var x = pairs[0];
			var y = pairs[1];
			pairs.RemoveAt(0);
			pairs.RemoveAt(0);

			var sum = Add(x, y);
			pairs.Add(sum);
		}

		return pairs[0];
	}
	private static Pair Add(Pair x, Pair y) {
		Pair newPair = new(x, y);
		Reduce(newPair, null, 0);

		return newPair;
	}
	private static bool Reduce(Pair pair, Pair? parent, int nesting) {
		bool success = false;

		if (nesting == 4) {
			Explode(pair, parent!);
			success = true;
		}

		success = success || pair.X.Switch(
			i => { if (i >= 10) { pair.X = Split(i); return true; } return false; },
			p => Reduce(p, pair, nesting + 1));
		success = success || pair.Y.Switch(
			i => { if (i >= 10) { pair.Y = Split(i); return true; } return false; },
			p => Reduce(p, pair, nesting + 1));

		return success;
	}
	private static void Explode(Pair pair, Pair parent) {
		throw new NotImplementedException();
	}
	private static Pair Split(int n) {
		throw new NotImplementedException();
	}

	private static int Magnitude(Pair pair) {
		static int identity(int value) => value;
		int x = pair.X.Switch(identity, Magnitude) * 3;
		int y = pair.Y.Switch(identity, Magnitude) * 2;
		return x + y;
	}



	// Pair needs to be mutable and record structs cannot contain themselves
	private class Pair {

		public Union<int, Pair> X { get; set; }
		public Union<int, Pair> Y { get; set; }

		public Pair(Union<int, Pair> x, Union<int, Pair> y) {
			X = x;
			Y = y;
		}

		public override string ToString() =>
			$"[{X},{Y}]";

	}
	
}
