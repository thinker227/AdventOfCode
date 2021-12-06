// namespace AdventOfCode.Solutions;

[Solver(6, @"input\06.txt")]
public sealed class Day06 : ISplitSolver {

    public Part SolvePart1(string input) {
        ulong result = Simulate(80, input);
        return result;
    }
    public Part SolvePart2(string input) {
        ulong result = Simulate(256, input);
        return result;
    }

    private static ulong Simulate(int generations, string nums) {
        Span<ulong> fishCounts = stackalloc ulong[10];

        unsafe {
            fixed (char* c = nums) {
                IntReader reader = new(c, nums.Length);
                while (!reader.AtEnd) {
                    int n = reader.Next();
                    fishCounts[n]++;
                }
            }
        }

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
