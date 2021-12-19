namespace AdventOfCode.Solutions;

[Solver(19, @"input\19.txt")]
public sealed class Day19 : ISolver {
	
	public CombinedSolution Solve(string input) {
		throw new TooLazyException();
	}

	private static readonly int[][][] rotations = new[] {
		new[] {
			new[] { 1, 0, 0 },
			new[] { 0, 1, 0 },
			new[] { 0, 0, 1 }
		},
		new[] {
			new[] { 1, 0, 0 },
			new[] { 0, 0, -1},
			new[] { 0, 1, 0 }
		},
		new[] {
			new[] { 1, 0, 0 },
			new[] { 0,-1, 0 },
			new[] { 0, 0,-1 }
		},
		new[] {
			new[] { 1, 0, 0 },
			new[] { 0, 0, 1 },
			new[] { 0,-1, 0 }
		},
		new[] {
			new[] { 0,-1, 0 },
			new[] { 1, 0, 0 },
			new[] { 0, 0, 1 }
		},
		new[] {
			new[] { 0, 0, 1 },
			new[] { 1, 0, 0 },
			new[] { 0, 1, 0 }
		},
		new[] {
			new[] { 0, 1, 0 },
			new[] { 1, 0, 0 },
			new[] { 0, 0,-1 }
		},
		new[] {
			new[] { 0, 0,-1 },
			new[] { 1, 0, 0 },
			new[] { 0,-1, 0 }
		},
		new[] {
			new[] {-1, 0, 0 },
			new[] { 0,-1, 0 },
			new[] { 0, 0, 1 }
		},
		new[] {
			new[] {-1, 0, 0 },
			new[] { 0, 0,-1 },
			new[] { 0,-1, 0 }
		},
		new[] {
			new[] {-1, 0, 0 },
			new[] { 0, 1, 0 },
			new[] { 0, 0,-1 }
		},
		new[] {
			new[] {-1, 0, 0 },
			new[] { 0, 0, 1 },
			new[] { 0, 1, 0 }
		},
		new[] {
			new[] { 0, 1, 0 },
			new[] {-1, 0, 0 },
			new[] { 0, 0, 1 }
		},
		new[] {
			new[] { 0, 0, 1 },
			new[] {-1, 0, 0 },
			new[] { 0,-1, 0 }
		},
		new[] {
			new[] { 0,-1, 0 },
			new[] {-1, 0, 0 },
			new[] { 0, 0,-1 }
		},
		new[] {
			new[] { 0, 0,-1 },
			new[] {-1, 0, 0 },
			new[] { 0, 1, 0 }
		},
		new[] {
			new[] { 0, 0,-1 },
			new[] { 0, 1, 0 },
			new[] { 1, 0, 0 }
		},
		new[] {
			new[] { 0, 1, 0 },
			new[] { 0, 0, 1 },
			new[] { 1, 0, 0 }
		},
		new[] {
			new[] { 0, 0, 1 },
			new[] { 0,-1, 0 },
			new[] { 1, 0, 0 }
		},
		new[] {
			new[] { 0,-1, 0 },
			new[] { 0, 0,-1 },
			new[] { 1, 0, 0 }
		},
		new[] {
			new[] { 0, 0,-1 },
			new[] { 0,-1, 0 },
			new[] {-1, 0, 0 }
		},
		new[] {
			new[] { 0,-1, 0 },
			new[] { 0, 0, 1 },
			new[] {-1, 0, 0 }
		},
		new[] {
			new[] { 0, 0, 1 },
			new[] { 0, 1, 0 },
			new[] {-1, 0, 0 }
		},
		new[] {
			new[] { 0, 1, 0 },
			new[] { 0, 0,-1 },
			new[] {-1, 0, 0 }
		},
	};
	private static IReadOnlyList<Point3> RotatePoints(IReadOnlyList<Point3> points, int[][] matrix) {
		var rotated = new Point3[points.Count];
		for (int i = 0; i < points.Count; i++)
			rotated[i] = points[i].Rotate(matrix);
		return rotated;
	}
	private static IReadOnlyList<IReadOnlyList<Point3>> RotatePointsAll(IReadOnlyList<Point3> points) {
		var rotated = new IReadOnlyList<Point3>[points.Count];
		for (int i = 0; i < rotations.Length; i++)
			rotated[i] = RotatePoints(points, rotations[i]);
		return rotated;
	}



	private class Record {

		public IReadOnlyCollection<Point3> Points { get; }
		public Point3? Position { get; private set; }

	}
	
}
