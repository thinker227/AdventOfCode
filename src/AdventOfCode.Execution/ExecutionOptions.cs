using System;
using CommandLine;

namespace AdventOfCode.Execution;

/// <summary>
/// Command-line options determining how to handle execution.
/// </summary>
internal sealed class ExecutionOptions {

	private static ExecutionOptions? cached;



	/// <summary>
	/// The day of the puzzle to run.
	/// </summary>
	[Option('d', "day", Required = false)]
	public int? Day { get; private set; }
	/// <summary>
	/// The input to pass to the solver.
	/// </summary>
	[Option('i', "input", Required = false)]
	public string? Input { get; private set; }
	[Option('b', "benchmark", Required = false)]
	public bool Benchmark { get; private set; }



	/// <summary>
	/// Gets the <see cref="ExecutionOptions"/> based on the command-line arguments.
	/// </summary>
	/// <returns>A new <see cref="ExecutionOptions"/> instance.</returns>
	/// <remarks>The command-line arguments are only parsed the first time
	/// <see cref="GetOptions"/> is called and the options are cached.</remarks>
	public static ExecutionOptions GetOptions() {
		if (cached is not null) return cached;

		var args = Environment.GetCommandLineArgs();
		ExecutionOptions result = null!;
		Parser.Default.ParseArguments<ExecutionOptions>(args)
			.WithParsed(o => result = o);
		cached = result;
		return result;
	}

}
