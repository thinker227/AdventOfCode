using System.Runtime.CompilerServices;

namespace AdventOfCode.Solutions;

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

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong Simulate(int generations, string nums) {
        const int newbornDelay = 2; // The additional delay before newborn fish can reproduce
        const int reproductionRate = 7; // The rate at which fish reproduce
        const int spanLength = reproductionRate + newbornDelay;
        const int newbornIndex = spanLength - 1; // The index newborn fish are spawned at
        
        // Each index represents how many fish have a specific amount of days left until reproduction
        Span<ulong> fishes = stackalloc ulong[spanLength];

        // Parse input into fish count
        unsafe {
            fixed (char* c = nums) {
                IntReader reader = new(c, nums.Length);
                while (!reader.AtEnd) {
                    int n = reader.Next();
                    fishes[n]++;
                }
            }
        }

        for (int generation = 1; generation <= generations; generation++) {
            // The amount of fishes which will reproduce this generation
            ulong reproductionCount = fishes[0];

            // Shift the entire array one step backwards
            for (int i = 0; i < spanLength - 1; i++)
                fishes[i] = fishes[i + 1];

            // Spawn one new fish with the standard reproduction rate
            fishes[reproductionRate - 1] += reproductionCount;
            // Spawn one new fish with the standard repreoduction rate plus the additional newborn delay
            fishes[newbornIndex] = reproductionCount;
        }

        ulong result = 0;
        // Sum amount of fishes
        for (int i = 0; i < spanLength; i++)
            result += fishes[i];
        return result;
    }

}
