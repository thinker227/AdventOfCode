using System.Threading.Tasks;

namespace AdventOfCode.Solutions;

[Solver(7, @"input\07.txt")]
public sealed class Day07 : ISolver {
	
	public CombinedSolution Solve(string input) {
		var nums = input.Split(',')
			.Select(int.Parse)
			.ToArray();
		int min = nums.Min();
		int max = nums.Max();

		var tasks = Enumerable.Range(min, max - min)
			.Select(i => GetConsumption(i, nums));
		var consumptions = Task.WhenAll(tasks).GetAwaiter().GetResult();
		int result = consumptions.OrderBy(d => d.consuption).First().consuption;

		return new(result);
	}

	private static Task<(int consuption, int distance)> GetConsumption(int distance, IEnumerable<int> nums) {
		int consumption = 0;
		foreach (int c in nums)
			consumption += Math.Abs(c - distance);
		return Task.FromResult((consumption, distance));
	}
	
}
