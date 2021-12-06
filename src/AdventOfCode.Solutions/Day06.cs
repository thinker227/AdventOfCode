namespace AdventOfCode.Solutions;

[Solver(6, @"input\06.txt")]
public sealed class Day06 : ISolver {

    public CombinedSolution Solve(string input) {
        const int generations = 80;
        var fishCounts = new int[10];

        var startNums = input.Split(',')
            .Select(int.Parse);
        foreach (int n in startNums)
            fishCounts[n]++;

        for (int generation = 1; generation <= generations; generation++) {
            fishCounts[7] += fishCounts[0];
            fishCounts[9] += fishCounts[0];
            for (int i = 0; i < fishCounts.Length - 1; i++)
                fishCounts[i] = fishCounts[i + 1];
            fishCounts[9] = 0;
        }

        int result = fishCounts.Sum();
        return new(result);

        throw new NotImplementedException();
    }

}
