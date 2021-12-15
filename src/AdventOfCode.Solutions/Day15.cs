namespace AdventOfCode.Solutions;

[Solver(15, @"input\15.txt")]
public sealed class Day15 : ISolver {
	
	public CombinedSolution Solve(string input) {
		var grid = input.Split('\n', StringSplitOptions.TrimEntries)
			.Select(i => i.ToCharArray())
			.ToArray();
		int width = input.IndexOf('\r');
		int height = grid.Length;
		var path = GetLowestPath(grid, width, height);
		int result = path
			.Select(i => i - '0')
			.Aggregate((a, b) => a + b);
		return result;
		throw new NotImplementedException();
	}

	private static IEnumerable<char> GetLowestPath(char[][] grid, int width, int height) {
		HashSet<Point> visited = new();
		PriorityQueue<Point, char[]> queue = new(new PathComparer());
		queue.Enqueue(new(0, 0), new char[] { });

		while (queue.TryDequeue(out var current, out var path)) {
			if (current.X == width - 1 && current.Y == height - 1)
				return path;
			
			if (visited.Contains(current)) continue;
			visited.Add(current);

			Point u = new(current.X, current.Y - 1);
			Point d = new(current.X, current.Y + 1);
			Point l = new(current.X - 1, current.Y);
			Point r = new(current.X + 1, current.Y);

			char up = grid.GetAtPositionOrDefault(u);
			char down = grid.GetAtPositionOrDefault(d);
			char left = grid.GetAtPositionOrDefault(l);
			char right = grid.GetAtPositionOrDefault(r);

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

		return Enumerable.Empty<char>();
	}



	private sealed class PathComparer : IComparer<char[]> {
		public int Compare(char[]? x, char[]? y) {
			int? xx = x?.Sum(c => c - '0');
			int? yy = y?.Sum(c => c - '0');

			if (xx is null || yy is null) return 0;
			return xx.Value - yy.Value;
		}
	}
	
}
