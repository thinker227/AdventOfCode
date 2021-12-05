﻿using System.Collections;

namespace AdventOfCode.Solutions;

[Solver(5, @"input\05.txt")]
public sealed class Day05 : ISolver {

	public CombinedSolution Solve(string input) {
		const StringSplitOptions trimRemove =
			StringSplitOptions.RemoveEmptyEntries |
			StringSplitOptions.TrimEntries;
		var linesRaw = input.Split('\n', trimRemove);
		var lines = linesRaw.Select(l => l.Split("->", trimRemove))
			.Select(l => new Line(ToPoint(l[0]), ToPoint(l[1])))
			.Where(l => l.IsAligned);
		var points = lines.SelectMany(l => l);

		Dictionary<Point, int> frequencies = new();
		foreach (var p in points) {
			frequencies.TryAdd(p, 0);
			frequencies[p]++;
		}

		int result = frequencies
			.Where(v => v.Value >= 2)
			.Count();
		return new(result);
	}
	private static Point ToPoint(string s) {
		var split = s.Split(',');
		return new(int.Parse(split[0]), int.Parse(split[1]));
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
