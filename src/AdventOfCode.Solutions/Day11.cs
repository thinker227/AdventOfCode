namespace AdventOfCode.Solutions;

[Solver(11, @"input\11.txt")]
public sealed class Day11 : ISolver {
	
	public CombinedSolution Solve(string input) {
		var field = input.Split('\n', StringSplitOptions.TrimEntries)
			.Select(i => i.ToCharArray())
			.ToArray();
		int width = input.IndexOf('\r');
		int height = field.Length;
		int total = width * height;

		int flashes = -1;
		int totalFlashes = 0;
		int i;
		for (i = 0; flashes != total; i++) {
			flashes = Simulate(field, width, height);
			if (i < 100) totalFlashes += flashes;
		}

		return new(totalFlashes, i);
	}

	private static int Simulate(char[][] field, int width, int height) {
		List<Point> flashes = new();

		foreach (var current in Point.GetPointGrid(width, height)) {
			char c = field.GetAtPositionOrDefault(current);
			if (c == default) continue;

			if (c >= '9') flashes.Add(current);
			field.SetAtPosition(current, (char)(c + 1));
		}

		Stack<Point> points = new(flashes);
		HashSet<Point> flashed = new();
		int totalFlashes = 0;
		flashed.Clear();
		while (points.TryPop(out var current)) {
			if (flashed.Contains(current)) continue;
			char c = field.GetAtPositionOrDefault(current);
			if (c == default) continue;

			if (c <= '9') {
				field.SetAtPosition(current, (char)(c + 1));
				if (c == '9') points.Push(current);
				continue;
			}

			flashed.Add(current);
			field.SetAtPosition(current, '0');
			totalFlashes++;

			points.Push(new(current.X - 1, current.Y - 1));
			points.Push(new(current.X, current.Y - 1));
			points.Push(new(current.X + 1, current.Y - 1));
			points.Push(new(current.X - 1, current.Y));
			points.Push(new(current.X + 1, current.Y));
			points.Push(new(current.X - 1, current.Y + 1));
			points.Push(new(current.X, current.Y + 1));
			points.Push(new(current.X + 1, current.Y + 1));
		}

		return totalFlashes;
	}
	
}
