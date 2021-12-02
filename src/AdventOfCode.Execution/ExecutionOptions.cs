using CommandLine;

namespace AdventOfCode.Execution;

/// <summary>
/// Command-line options determining how to handle execution.
/// </summary>
internal sealed class ExecutionOptions {

	/// <summary>
	/// The day of the puzzle to run.
	/// </summary>
	[Option('d', "day", Required = false)]
	public int? Day { get; set; }
	/// <summary>
	/// The input to pass to the solver.
	/// </summary>
	[Option('i', "input", Required = false)]
	public string? Input { get; set; }

}
