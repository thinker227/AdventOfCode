namespace AdventOfCode.Solutions;

[Solver(21, @"input\21.txt")]
public sealed class Day21 : ISolver {
	
	public CombinedSolution Solve(string input) {
		var (p1, p2) = ParseInput(input);

		int die = 0;
		int player = 1;
		while (true) {
			int steps = 0;
			steps += (die++ % 100) + 1;
			steps += (die++ % 100) + 1;
			steps += (die++ % 100) + 1;

			if (player == 1) p1 = Turn(p1, steps);
			else p2 = Turn(p2, steps);

			if (p1.Score >= 1000 || p2.Score >= 1000) break;

			player = player == 1 ? 2 : 1;
		}

		int losingScore = player == 1 ? p2.Score : p1.Score;
		int result = losingScore * die;
		return result;
	}

	private static (Player a, Player b) ParseInput(string input) {
		int aStart = input[28] - '0';
		int bStart = input[59] - '0';
		return (new(aStart, 0), new(bStart, 0));
	}

	private static Player Turn(Player player, int steps) {
		int newPosition = player.Position + steps;
		while (newPosition > 10) newPosition -= 10;
		int newScore = player.Score + newPosition;
		return new(newPosition, newScore);
	}



	private readonly record struct Player(int Position, int Score);
	
}
