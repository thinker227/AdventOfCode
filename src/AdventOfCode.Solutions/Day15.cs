namespace AdventOfCode.Solutions;

[Solver(15, @"input\15.txt")]
public sealed class Day15 : ISplitSolver {
	
	public Part SolvePart1(string input) {
		var grid = input.Split('\n', StringSplitOptions.TrimEntries)
			.Select(i => i.ToCharArray())
			.ToArray();
		int width = input.IndexOf('\r');
		int height = grid.Length;

		char selector(Point p) => grid.GetAtPositionOrDefault(p);
		var path = GetLowestPath(selector, width, height);
		int result = path
			.Select(i => i - '0')
			.Aggregate((a, b) => a + b);
		return result;
	}
	public Part SolvePart2(string input) {
		var grid = input.Split('\n', StringSplitOptions.TrimEntries)
			.Select(i => i.ToCharArray())
			.ToArray();
		int width = input.IndexOf('\r');
		int height = grid.Length;

		char selector(Point p) {
			Point pReal = new(p.X % width, p.Y % height);
			Point square = new(p.X / width, p.Y / height);
			int offset = square.X + square.Y;
			char cReal = grid.GetAtPositionOrDefault(pReal);
			if (cReal == default) return default;
			char c = (char)(cReal + offset);
			if (c > '9') c = (char)(c - 9);
			return c;
		}

		var path = GetLowestPath(selector, width * 5, height * 5);
		int result = path
			.Select(i => i - '0')
			.Aggregate((a, b) => a + b);
		return result;
	}

	private static IEnumerable<char> GetLowestPath(Func<Point, char> gridSelector, int width, int height) {
		HashSet<Point> visited = new();
		List<char[]> endingPaths = new();
		PriorityQueue<Point, char[]> queue = new(PathComparer.Instance);
		queue.Enqueue(new(0, 0), Array.Empty<char>());

		while (queue.TryDequeue(out var current, out var path)) {
			if (current.X < 0 || current.X >= width || current.Y < 0 || current.Y >= height) continue;

			if (current.X == width - 1 && current.Y == height - 1)
				endingPaths.Add(path);

			if (visited.Contains(current)) continue;
			visited.Add(current);

			Point u = new(current.X, current.Y - 1);
			Point d = new(current.X, current.Y + 1);
			Point l = new(current.X - 1, current.Y);
			Point r = new(current.X + 1, current.Y);
			
			char up = gridSelector(u);
			char down = gridSelector(d);
			char left = gridSelector(l);
			char right = gridSelector(r);

			int pathLength = path.Length + 1;

			if (up != default) {
				var newPath = new char[pathLength];
				path.CopyTo(newPath, 0);
				newPath[^1] = up;
				queue.Enqueue(u, newPath);
			}
			if (down != default) {
				var newPath = new char[pathLength];
				path.CopyTo(newPath, 0);
				newPath[^1] = down;
				queue.Enqueue(d, newPath);
			}
			if (left != default) {
				var newPath = new char[pathLength];
				path.CopyTo(newPath, 0);
				newPath[^1] = left;
				queue.Enqueue(l, newPath);
			}
			if (right != default) {
				var newPath = new char[pathLength];
				path.CopyTo(newPath, 0);
				newPath[^1] = right;
				queue.Enqueue(r, newPath);
			}
		}

		return endingPaths.Min(PathComparer.Instance)!;
	}



	private sealed class PathComparer : IComparer<char[]> {
		public static PathComparer Instance { get; } = new();
		public int Compare(char[]? x, char[]? y) {
			int? xx = x?.Sum(c => c - '0');
			int? yy = y?.Sum(c => c - '0');

			if (xx is null || yy is null) return 0;
			return xx.Value - yy.Value;
		}
	}
	
}
