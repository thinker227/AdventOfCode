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
		int p1 = consumptions.OrderBy(c => c.Consumption).First().Consumption;
		int p2 = consumptions.OrderBy(c => c.ActualConsumption).First().ActualConsumption;

		return new(p1, p2);
	}

	// Asynchronous for some reason
	private static Task<ConsumptionInfo> GetConsumption(int position, IEnumerable<int> nums) {
		int consumption = 0;
		int actualConsumption = 0;
		foreach (int c in nums) {
			int steps = Math.Abs(c - position);
			consumption += steps;
			actualConsumption += Enumerable.Range(0, steps + 1).Sum();
		}
		return Task.FromResult(new ConsumptionInfo(consumption, actualConsumption));
	}

	// Consumption is part 1, ActualConsumption is part 2
	private readonly record struct ConsumptionInfo(int Consumption, int ActualConsumption);
	
}
