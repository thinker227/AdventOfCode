namespace AdventOfCode.Solutions;

[Solver(17, @"input\17.txt")]
public sealed class Day17 : ISolver {
	
	public CombinedSolution Solve(string input) {
		var target = ParseRectangle(input);
		int result = Triangle(target.Min.Y);
		return result;
	}

	private static unsafe Rectangle ParseRectangle(string input) {
		int xMin, yMin, xMax, yMax;
		fixed (char* c = input) {
			IntReader reader = new(c, input.Length);
			xMin = reader.Next();
			xMax = reader.Next();
			yMin = reader.Next();
			yMax = reader.Next();
		}
		return new(xMin, yMin, xMax, yMax);
	}

	private static int Triangle(int n) =>
		((n + 1) * n) / 2;

}
