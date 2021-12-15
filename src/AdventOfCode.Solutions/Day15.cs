namespace AdventOfCode.Solutions;

[Solver(15, @"input\15.txt")]
public sealed class Day15 : ISolver {
	
	public CombinedSolution Solve(string input) {
		var grid = input.Split('\n', StringSplitOptions.TrimEntries)
			.Select(i => i.ToCharArray())
			.ToArray();
		var path = GetLowestPath(grid);
		int result = path
			.Select(i => i - '0')
			.Aggregate((a, b) => a + b);
		return result;
		throw new NotImplementedException();
	}

	private static IEnumerable<char> GetLowestPath(char[][] grid) {
		PriorityQueue<Point, char[]> queue = new(new PathComparer());
	}



	private sealed class PathComparer : IComparer<char[]> {
		public int Compare(char[]? x, char[]? y) {
			int? xx = x?.Sum(c => c - '0');
			int? yy = y?.Sum(c => c - '0');

			if (xx is null && yy is null) return 0;
			if (xx is null) return -yy!.Value;
			if (yy is null) return xx.Value;
			return xx.Value - yy.Value;
		}
	}
	
}
