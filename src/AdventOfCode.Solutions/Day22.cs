namespace AdventOfCode.Solutions;

[Solver(22, @"input\22.txt")]
public sealed class Day22 : ISolver {
	
	public CombinedSolution Solve(string input) {
		var steps = ParseInput(input)
			.Where(s =>
				s.Area.Min.X >= -50 &&
				s.Area.Min.Y >= -50 &&
				s.Area.Min.Z >= -50 &&
				s.Area.Max.X <= 50 &&
				s.Area.Max.Y <= 50 &&
				s.Area.Max.Z <= 50);
		var points = Reboot(steps);
		int result = points.Count;
		return result;
	}

	private static unsafe IEnumerable<RebootStep> ParseInput(string input) {
		List<RebootStep> steps = new();
		fixed (char* c = input) {
			CharReader reader = new(c, input.Length);
			while (!reader.AtEnd) {
				var step = ParseStep(ref reader);
				steps.Add(step);
				reader.ExpectChar('o');
			}
		}
		return steps;
	}
	private static RebootStep ParseStep(ref CharReader reader) {
		bool state = reader.NextChar() == 'n';

		int xMin = reader.NextInt();
		int xMax = reader.NextInt();
		int yMin = reader.NextInt();
		int yMax = reader.NextInt();
		int zMin = reader.NextInt();
		int zMax = reader.NextInt();
		Cuboid area = new(xMin, yMin, zMin, xMax, yMax, zMax);

		return new(area, state);
	}

	private static IReadOnlyCollection<Point3> Reboot(IEnumerable<RebootStep> steps) {
		HashSet<Point3> points = new();
		foreach (var step in steps) {
			foreach (var p in step.Area.EnumeratePoints()) {
				if (step.State) points.Add(p);
				else points.Remove(p);
			}
		}
		return points;
	}



	private readonly record struct RebootStep(Cuboid Area, bool State);
	
}
