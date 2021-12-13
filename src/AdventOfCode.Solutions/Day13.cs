namespace AdventOfCode.Solutions;

[Solver(13, @"input\13.txt")]
public sealed class Day13 : ISolver {
	
	public CombinedSolution Solve(string input) {
		(var points, var folds) = ParseInput(input);
		var folded = points = FoldPoints(points, folds[0]);
		return folded.Length;
		throw new NotImplementedException();
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

	private static Point[] FoldPoints(Point[] points, Fold f) {
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

		return folded.ToArray();
	}



	private enum Axis {
		X, Y
	}
	private readonly record struct Fold(Axis Axis, int Position);
	
}
