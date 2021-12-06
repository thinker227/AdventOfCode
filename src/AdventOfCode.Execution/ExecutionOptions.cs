using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;

namespace AdventOfCode.Execution;

/// <summary>
/// Command-line options determining how to handle execution.
/// </summary>
internal sealed class ExecutionOptions {

	/// <summary>
	/// Options for run mode.
	/// </summary>
	[Verb("run", true)]
	public sealed class RunOptions {

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

	}

	/// <summary>
	/// Options for init mode.
	/// </summary>
	[Verb("init")]
	public sealed class InitOptions {

		/// <summary>
		/// The day to initialize.
		/// </summary>
		[Value(0, Required = true)]
		public int Day { get; private set; }
		/// <summary>
		/// The class name of the class to initialize.
		/// </summary>
		[Option("classname")]
		public string? ClassName { get; private set; }
		/// <summary>
		/// The file name for the input file to initialize.
		/// </summary>
		[Option("inputfilename")]
		public string? InputFileName { get; private set; }
		/// <summary>
		/// Whether to download the input for the day.
		/// </summary>
		[Option('d', "download")]
		public bool Download { get; private set; }

	}


	
	/// <summary>
	/// The options for run mode.
	/// </summary>
	public RunOptions? Run { get; private set; }
	/// <summary>
	/// The options for init mode.
	/// </summary>
	public InitOptions? Init { get; private set; }



	private int SetRunOptions(RunOptions options) {
		Run = options;
		return 1;
	}
	private int SetInitOptions(InitOptions options) {
		Init = options;
		return 1;
	}

	/// <summary>
	/// Gets the <see cref="ExecutionOptions"/> based on the command-line arguments.
	/// </summary>
	/// <returns>A new <see cref="ExecutionOptions"/> instance.</returns>
	public static ExecutionOptions GetOptions(IEnumerable<string>? args = null) {
		args ??= Environment.GetCommandLineArgs().Skip(1);

		ExecutionOptions options = new();
		Parser.Default.ParseArguments<RunOptions, InitOptions>(args)
			.MapResult<RunOptions, InitOptions, int>(
				options.SetRunOptions,
				options.SetInitOptions,
				(e) => 0
			);

		return options;
	}

}
