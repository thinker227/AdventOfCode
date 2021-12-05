namespace AdventOfCode.Solutions;

[Solver(4, @"input\04.txt")]
public sealed class Day04 : ISplitSolver {

	private static string[] SplitInput(string input) =>
		input.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);
	private static int[] GetSequence(string[] split) =>
		split.First()
			.Split(',')
			.Select(int.Parse)
			.ToArray();
	private static IEnumerable<Board> GetBoards(string[] split) =>
		split.Skip(1)
			.Select(b => b
				.Split("\r\n", StringSplitOptions.RemoveEmptyEntries)
				.Select(r => r
					.Split(' ', StringSplitOptions.RemoveEmptyEntries)
					.Select(int.Parse)
					.ToArray())
				.ToArray())
			.Select(b => new Board(b));

	public Part SolvePart1(string input) {
		var split = SplitInput(input);
		var sequence = GetSequence(split);
		var boards = GetBoards(split).ToArray();

		int result = 0;
		List<int> marks = new();
		foreach (int number in sequence) {
			marks.Add(number);
			if (marks.Count < 5) continue;

			var board = boards.FirstOrDefault(b => b.Check(marks));
			if (board is not null) {
				result = board.Unmarked(marks).Sum() * number;
				break;
			}
		}

		return result;
		throw new NotImplementedException();
	}
	public Part SolvePart2(string input) {
		var split = SplitInput(input);
		var sequence = GetSequence(split);
		var boards = GetBoards(split).ToList();

		List<int> marks = new();
		foreach (int number in sequence) {
			marks.Add(number);
			if (marks.Count < 5) continue;

			var solved = boards.Where(b => b.Check(marks)).ToArray();
			foreach (var b in solved) {
				if (boards.Count == 1) {
					int result = boards[0].Unmarked(marks).Sum() * number;
					return result.ToString();
				}
				boards.Remove(b);
			}
		}

		throw new InvalidOperationException("Unreachable");
	}

	private sealed class Board {

		private const int width = 5;
		private const int height = 5;
		private readonly int[][] rows;
		private readonly int[][] columns;

		public Board(int[][] numbers) {
			rows = numbers;
			columns = Columns(numbers);
		}

		// Converts an array of rows into an array of columns
		private static int[][] Columns(int[][] rows) {
			IEnumerable<int> column(int x) {
				foreach (var row in rows) {
					int index = 0;
					foreach (int n in row) {
						if (index++ % width == x)
							yield return n;
					}
				}
			}

			int[][] columns = new int[width][];
			for (int x = 0; x < width; x++)
				columns[x] = column(x).ToArray();
			return columns;
		}
		public IEnumerable<int> Unmarked(IEnumerable<int> marks) {
			foreach (var row in rows)
				foreach (var element in row)
					if (!marks.Contains(element))
						yield return element;
		}
		public bool Check(IEnumerable<int> marks) {
			bool all(int[] nums) => nums.All(n => marks.Contains(n));
			return rows.Any(all) || columns.Any(all);
		}

	}

}
