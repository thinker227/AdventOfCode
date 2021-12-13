using System.Text;

namespace AdventOfCode.Solutions;

[Solver(13, @"input\13.txt")]
public sealed class Day13 : ISplitSolver {
	
	public Part SolvePart1(string input) {
		(var points, var folds) = ParseInput(input);
		var folded = FoldPoints(points, folds[0]);
		return folded.Count;
	}
	public Part SolvePart2(string input) {
		(var points, var folds) = ParseInput(input);

		HashSet<Point>? folded = null;
		foreach (var f in folds) {
			if (folded is null) {
				folded = FoldPoints(points, f);
				continue;
			}
			folded = FoldPoints(folded, f);
		}

		string str = PointsToString(folded!);
		return str;
	}

	private static (Point[], Fold[]) ParseInput(string s) {
		var span = s.AsSpan();
		bool parseFolds = false;
		List<Point> points = new();
		List<Fold> folds = new();

		foreach (var line in span.EnumerateLines()) {
			if (line.IsWhiteSpace()) {
				parseFolds = true;
				continue;
			}
			if (parseFolds) {
				var axis = line[11] == 'x' ? Axis.X : Axis.Y;
				int position = Parsing.ParseInt(line[13..]);
				Fold f = new(axis, position);
				folds.Add(f);
			} else {
				int separator = line.IndexOf(',');
				int x = Parsing.ParseInt(line[..separator]);
				int y = Parsing.ParseInt(line[(separator + 1)..]);
				Point p = new(x, y);
				points.Add(p);
			}
		}

		return (points.ToArray(), folds.ToArray());
	}

	private static HashSet<Point> FoldPoints(IEnumerable<Point> points, Fold f) {
		HashSet<Point> folded = new();

		foreach (var p in points) {
			Point foldedPoint;
			if (f.Axis == Axis.X) {
				if (p.X < f.Position) foldedPoint = p;
				else foldedPoint = new(f.Position - (p.X - f.Position), p.Y);
			} else {
				if (p.Y < f.Position) foldedPoint = p;
				else foldedPoint = new(p.X, f.Position - (p.Y - f.Position));
			}
			folded.Add(foldedPoint);
		}

		return folded;
	}

	private static string PointsToString(HashSet<Point> points) {
		int width = points.Max(p => p.X);
		int height = points.Max(p => p.Y);

		var builder = new StringBuilder().AppendLine();
		for (int y = 0; y <= height; y++) {
			for (int x = 0; x <= width; x++) {
				if (points.Contains(new(x, y))) builder.Append('#');
				else builder.Append(' ');
			}
			builder.AppendLine();
		}

		return builder.ToString();
	}



	private enum Axis {
		X, Y
	}
	private readonly record struct Fold(Axis Axis, int Position);
	
}
