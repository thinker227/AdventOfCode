namespace AdventOfCode.Solutions;

[Solver(9, @"input\09.txt")]
public sealed class Day09 : ISplitSolver {
	
	public Part SolvePart1(string input) {
		var field = input.Split('\n', StringSplitOptions.TrimEntries);
		var points = GetLowpoints(field);
		int result = points.Count;
		for (int i = 0; i < points.Count; i++)
			result += points[i].Value - '0';

		return result;
	}
	public Part SolvePart2(string input) {
		var field = input.Split('\n', StringSplitOptions.TrimEntries);
		var points = GetLowpoints(field);
		int result = GetBasins(field, points)
			.OrderByDescending(i => i)
			.Take(3)
			.Aggregate((a, b) => a * b);

		return result;
	}

	private static IReadOnlyList<FieldPoint> GetLowpoints(string[] fields) {

		int width = fields[0].Length;
		int height = fields.Length;
		List<FieldPoint> points = new();

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				char current = fields[y][x];
				char up = y > 0 ? fields[y - 1][x] : '9';
				char down = y < height - 1 ? fields[y + 1][x] : '9';
				char left = x > 0 ? fields[y][x - 1] : '9';
				char right = x < width - 1 ? fields[y][x + 1] : '9';

				if (current >= up || current >= down || current >= left || current >= right)
					continue;
				points.Add(new(new(x, y), current));
			}
		}

		return points;
	}
	// Returns a collection of basin sizes
	private static IEnumerable<int> GetBasins(string[] field, IEnumerable<FieldPoint> lowpoints) {
		var basins = lowpoints.Select(p => Floodfill(field, p.Point));
		return basins;
	}
	// Returns the size of a floodfill area
	private static int Floodfill(string[] field, Point from) {
		Queue<Point> points = new();
		points.Enqueue(from);
		HashSet<Point> visited = new();
		int width = field[0].Length;
		int height = field.Length;

		void process(Point p, Point source) {
			if (p.X < 0 || p.X >= width || p.Y < 0 || p.Y >= height) return;
			if (visited.Contains(p)) return;

			int pValue = field[p.Y][p.X];
			int sourceValue = field[source.Y][source.X];

			if (pValue > sourceValue && pValue < '9')
				points.Enqueue(p);
		}

		while (points.Count > 0) {
			Point current = points.Dequeue();
			visited.Add(current);

			process(new(current.X - 1, current.Y), current);
			process(new(current.X + 1, current.Y), current);
			process(new(current.X, current.Y - 1), current);
			process(new(current.X, current.Y + 1), current);
		}

		return visited.Count;
	}



	private readonly record struct FieldPoint(Point Point, char Value);
	
}
