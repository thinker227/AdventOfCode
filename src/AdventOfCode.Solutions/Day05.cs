using System.Collections;

namespace AdventOfCode.Solutions;

[Solver(5, @"input\05.txt")]
public sealed class Day05 : ISplitSolver {

	public Part SolvePart1(string input) {
		var lines = GetLines(input)
			.Where(l => l.IsAligned);
		int result = GetFrequencies(lines);
		return result;
	}
	public Part SolvePart2(string input) {
		var lines = GetLines(input);
		int result = GetFrequencies(lines);
		return result;
	}

	private static IEnumerable<Line> GetLines(string input) {
		const StringSplitOptions trimRemove =
			StringSplitOptions.RemoveEmptyEntries |
			StringSplitOptions.TrimEntries;
		var linesRaw = input.Split('\n', trimRemove);
		var lines = linesRaw.Select(l => l.Split("->", trimRemove))
			.Select(l => new Line(ToPoint(l[0]), ToPoint(l[1])));
		return lines;
	}
	private static Point ToPoint(string s) {
		var split = s.Split(',');
		return new(int.Parse(split[0]), int.Parse(split[1]));
	}
	private static int GetFrequencies(IEnumerable<Line> lines) {
		var points = lines.SelectMany(l => l);

		Dictionary<Point, int> frequencies = new();
		foreach (var p in points) {
			frequencies.TryAdd(p, 0);
			frequencies[p]++;
		}

		int result = frequencies
			.Where(v => v.Value >= 2)
			.Count();
		return result;
	}



	private readonly struct Line : IEnumerable<Point> {

		public Point Min { get; }
		public Point Max { get; }
		public bool IsAligned =>
			Min.X == Max.X || Min.Y == Max.Y;



		public Line(Point min, Point max) {
			Min = min;
			Max = max;
		}



		public IEnumerator<Point> GetEnumerator() {
			Point current = Min;
			Point increase = new(
				Math.Sign(Max.X - Min.X),
				Math.Sign(Max.Y - Min.Y));

			yield return current;
			while (current != Max) {
				current = new(current.X + increase.X, current.Y + increase.Y);
				yield return current;
			}
		}
		IEnumerator IEnumerable.GetEnumerator() =>
			GetEnumerator();

		public override string ToString() =>
			$"{Min} -> {Max}";

	}

}
