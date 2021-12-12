namespace AdventOfCode.Solutions;

[Solver(12, @"input\12.txt")]
public sealed class Day12 : ISplitSolver {

	public Part SolvePart1(string input) {
		CaveSystem system = ParseCaveSystem(input);
		var paths = GetAllPaths(system.Start, Path.Empty, false);
		return paths.Length;
	}
	public Part SolvePart2(string input) {
		CaveSystem system = ParseCaveSystem(input);
		var paths = GetAllPaths(system.Start, Path.Empty, true);
		return paths.Length;
	}

	private static CaveSystem ParseCaveSystem(string input) {
		CaveSystem system = new();
		var lines = input.Split('\n', StringSplitOptions.TrimEntries);
		foreach (string line in lines)
			system.ParseAddCave(line);
		return system;
	}

	private static Path[] GetAllPaths(Cave current, Path currentPath, bool part2, bool spent = false) {
		if (current.IsEnd) {
			var path = currentPath with {
				Visited = currentPath.Visited.Add(current)
			};
			return new[] { path };
		}

		List<Path> paths = new();
		var visited = currentPath.Visited.Add(current);

		ImmutableHashSet<Cave> visitedSmall;
		if (current.IsSmall) {
			if (currentPath.VisitedSmall.Contains(current))
				return Array.Empty<Path>();

			if (!current.IsSpecial && part2 && !spent) {
				Path otherPath = currentPath with { Visited = visited };
				foreach (var c in current.Connections)
					paths.AddRange(GetAllPaths(c, otherPath, true, true));
			}

			visitedSmall = currentPath.VisitedSmall.Add(current);
		}
		else visitedSmall = currentPath.VisitedSmall;

		Path newPath = new(visited, visitedSmall);
		foreach (var c in current.Connections)
			paths.AddRange(GetAllPaths(c, newPath, part2, spent));
		return paths.ToArray();
	}



	private record Cave(string Name) {
		public HashSet<Cave> Connections { get; } = new();
		public bool IsSmall => Name.All(c => char.IsLower(c));
		public bool IsSpecial => IsStart || IsEnd;
		public bool IsStart => Name == "start";
		public bool IsEnd => Name == "end";

		public override string? ToString() {
			var connections = Connections
				.Select(c => c.Name);
			string connectionsString = string.Join(", ", connections);
			return $"{Name} -> {connectionsString}";
		}
	}

	private sealed class CaveSystem {

		private readonly Dictionary<string, Cave> caves = new();

		public Cave Start { get; private set; } = default!;
		public Cave End { get; private set; } = default!;

		private Cave AddCave(string name) {
			if (caves.TryGetValue(name, out var result))
				return result;

			Cave cave = new(name);
			caves.Add(name, cave);
			if (cave.IsStart) Start = cave;
			if (cave.IsEnd) End = cave;
			return cave;
		}
		public void ParseAddCave(string s) {
			int separator = s.IndexOf('-');
			var from = s[..(separator)];
			var to = s[(separator + 1)..];

			var fromCave = AddCave(from);
			var toCave = AddCave(to);
			fromCave.Connections.Add(toCave);
			toCave.Connections.Add(fromCave);
		}

	}

	private record Path(ImmutableArray<Cave> Visited, ImmutableHashSet<Cave> VisitedSmall) {
		public static Path Empty { get; } = new (
			ImmutableArray.Create<Cave>(),
			ImmutableHashSet.Create<Cave>()
		);
		public int Length => Visited.Length;
		
		public override string? ToString() {
			var visited = Visited
				.Select(c => c.Name);
			string visitedString = string.Join(" > ", visited);
			return visitedString;
		}
	}

}
