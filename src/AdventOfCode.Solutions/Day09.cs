//namespace AdventOfCode.Solutions;

[Solver(9, @"input\09.txt")]
public sealed class Day09 : ISolver {
	
	public CombinedSolution Solve(string input) {
		var lines = input.Split('\n', StringSplitOptions.TrimEntries);
		int width = input.IndexOf('\r');
		int height = lines.Length;
		int result = 0;
		
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				char current = lines[y][x];
				char up = y > 0 ? lines[y - 1][x] : '9';
				char down = y < height - 1 ? lines[y + 1][x] : '9';
				char left = x > 0 ? lines[y][x - 1] : '9';
				char right = x < width - 1 ? lines[y][x + 1] : '9';

				if (current >= up || current >= down || current >= left || current >= right)
					continue;
				result += current - '0' + 1;
			}
		}

		return new(result);
	}
	
}
