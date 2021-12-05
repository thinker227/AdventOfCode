namespace AdventOfCode.Common;

/// <summary>
/// An <see cref="ISolver"/> wrapper of a <see cref="ISplitSolver"/>.
/// </summary>
public sealed class SplitSolverWrapper : ISolver {

    /// <summary>
    /// The wrapped <see cref="ISplitSolver"/>.
    /// </summary>
    public ISplitSolver Solver { get; }



    /// <summary>
    /// Initializes a new <see cref="SplitSolverWrapper"/> instance.
    /// </summary>
    /// <param name="splitSolver">The wrapped <see cref="ISplitSolver"/>.</param>
    public SplitSolverWrapper(ISplitSolver splitSolver) {
        Solver = splitSolver;
    }



    public CombinedSolution Solve(string input) =>
        new(Solver.SolvePart1(input), Solver.SolvePart2(input));

}