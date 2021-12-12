namespace AdventOfCode.Solutions;

[Solver(12, @"input\12.txt")]
public sealed class Day12 : ISolver {
	
	public CombinedSolution Solve(string input) {
		CaveSystem system = new();
		var lines = input.Split('\n', StringSplitOptions.TrimEntries);
		foreach (string line in lines)
			system.ParseAddCave(line);

		Path startPath = new(
			ImmutableArray.Create<Cave>(),
			ImmutableHashSet.Create<Cave>()
		);
		var paths = GetAllPaths(system.Start, startPath);

		return new(paths.Length);
	}

	private static Path[] GetAllPaths(Cave current, Path currentPath) {
		if (current.IsEnd) {
			var path = currentPath with {
				Visited = currentPath.Visited.Add(current)
			};
			return new[] { path };
		}

		ImmutableHashSet<Cave>? visitedSmall = null;
		if (current.IsSmall) {
			if (currentPath.VisitedSmall.Contains(current))
				return Array.Empty<Path>();
			visitedSmall = currentPath.VisitedSmall.Add(current);
		}
		visitedSmall ??= currentPath.VisitedSmall;

		var visited = currentPath.Visited.Add(current);

		Path newPath = new(visited, visitedSmall);
		List<Path> paths = new();
		foreach (var c in current.Connections)
			paths.AddRange(GetAllPaths(c, newPath));
		return paths.ToArray();
	}



	private record Cave(string Name) {
		public HashSet<Cave> Connections { get; } = new();
		public bool IsSmall => Name.All(c => char.IsLower(c));
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
		public Cave GetCave(string name) =>
			caves[name];

	}

	private record Path(ImmutableArray<Cave> Visited, ImmutableHashSet<Cave> VisitedSmall) {
		public int Length => Visited.Length;
		
		public override string? ToString() {
			var visited = Visited
				.Select(c => c.Name);
			string visitedString = string.Join(" > ", visited);
			return visitedString;
		}
	}

}
