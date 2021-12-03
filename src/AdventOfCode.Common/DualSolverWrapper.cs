namespace AdventOfCode.Common;

/// <summary>
/// An <see cref="ISolver"/> wrapper of a <see cref="IDualSolver"/>.
/// </summary>
public sealed class DualSolverWrapper : ISolver {

    /// <summary>
    /// The wrapped <see cref="IDualSolver"/>.
    /// </summary>
    public IDualSolver Solver { get; }



    /// <summary>
    /// Initializes a new <see cref="DualSolverWrapper"/> instance.
    /// </summary>
    /// <param name="dualSolver">The wrapped <see cref="IDualSolver"/>.</param>
    public DualSolverWrapper(IDualSolver dualSolver) {
        Solver = dualSolver;
    }



    public Solution Solve(string? input) =>
        new(Solver.SolvePart1(input), Solver.SolvePart2(input));

}