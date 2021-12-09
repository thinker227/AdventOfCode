//namespace AdventOfCode.Solutions;

[Solver(9, @"input\09.txt")]
public sealed class Day09 : ISolver {
	
	public CombinedSolution Solve(string input) {
		var field = input.Split('\n', StringSplitOptions.TrimEntries);
		var points = GetLowpoints(field);
		int result = points.Count;
		for (int i = 0; i < points.Count; i++)
			result += points[i].Value - '0';

		return new(result);
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



	private readonly record struct FieldPoint(Point Point, char Value);
	
}
