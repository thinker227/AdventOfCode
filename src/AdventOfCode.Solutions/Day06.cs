namespace AdventOfCode.Solutions;

[Solver(6, @"input\06.txt")]
public sealed class Day06 : ISplitSolver {

    public Part SolvePart1(string input) {
        var startFishes = ParseInput(input);
        ulong result = Simulate(80, startFishes);
        return result;
    }
    public Part SolvePart2(string input) {
        var startFishes = ParseInput(input);
        ulong result = Simulate(256, startFishes);
        return result;
    }

    private static int[] ParseInput(string input) =>
        input.Split(',').Select(int.Parse).ToArray();

    private static ulong Simulate(int generations, int[] startFishes) {
        var fishCounts = new ulong[10];

        foreach (int n in startFishes)
            fishCounts[n]++;

        for (int generation = 1; generation <= generations; generation++) {
            fishCounts[7] += fishCounts[0];
            fishCounts[9] += fishCounts[0];
            for (int i = 0; i < fishCounts.Length - 1; i++)
                fishCounts[i] = fishCounts[i + 1];
            fishCounts[9] = 0;
        }

        ulong result = 0;
        for (int i = 0; i < fishCounts.Length; i++)
            result += fishCounts[i];
        return result;
    }

}
