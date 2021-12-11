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
		Queue<Point> points = new();
		HashSet<Point> visited = new();
		Stack<Point> flashes = new();
		points.Enqueue(new(0, 0));

		while (points.TryDequeue(out var current)) {
			if (visited.Contains(current)) continue;
			char c = field.GetAtPositionOrDefault(current);
			if (c == default) continue;

			visited.Add(current);
			if (c >= '9') flashes.Push(current);
			field.SetAtPosition(current, (char)(c + 1));

			points.Enqueue(new(current.X + 1, current.Y));
			points.Enqueue(new(current.X, current.Y + 1));
		}

		int totalFlashes = 0;
		visited.Clear();
		while (flashes.TryPop(out var flash)) {
			if (visited.Contains(flash)) continue;
			char c = field.GetAtPositionOrDefault(flash);
			if (c == default) continue;

			if (c <= '9') {
				field.SetAtPosition(flash, (char)(c + 1));
				if (c == '9') flashes.Push(flash);
				continue;
			}

			visited.Add(flash);
			field.SetAtPosition(flash, '0');
			totalFlashes++;

			flashes.Push(new(flash.X - 1, flash.Y - 1));
			flashes.Push(new(flash.X, flash.Y - 1));
			flashes.Push(new(flash.X + 1, flash.Y - 1));
			flashes.Push(new(flash.X - 1, flash.Y));
			flashes.Push(new(flash.X + 1, flash.Y));
			flashes.Push(new(flash.X - 1, flash.Y + 1));
			flashes.Push(new(flash.X, flash.Y + 1));
			flashes.Push(new(flash.X + 1, flash.Y + 1));
		}

		return totalFlashes;
	}
	
}
