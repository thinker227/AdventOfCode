using System;
using System.Diagnostics;
using System.IO;
using AdventOfCode.Common;

namespace AdventOfCode.Execution;

// This is essentially a runtime source generator.
/// <summary>
/// Contains methods to initialize solution files.
/// </summary>
internal static class Initializer {

	private const string AOCSolutionsPathEnvvar = "AOCSolutionsPath";

	
	
	/// <summary>
	/// Initializes the files for solver for a day.
	/// </summary>
	/// <param name="options">The <see cref="ExecutionOptions.InitOptions"/>
	/// to use to generate the files.</param>
	public static void Initialize(ExecutionOptions.InitOptions options) {
		if (Debugger.IsAttached) // ENC0021: Adding namespace requires restarting the application.
			throw new InitializerException("Cannot initialize files while debugger is attached.");

		int day = options.Day;
		bool splitSolver = options.SplitSolver;
		string className = options.ClassName ?? GetDefaultClassName(day);
		string @namespace = options.Namespace ?? GetDefaultNamespace();
		string solutionsPath = GetSolutionsProjectPath();
		string solverFileName = options.SolverFileName ?? GetDefaultSolverFileName(className);
		string absoluteSolverFilePath = Path.Combine(solutionsPath, solverFileName);
		string relativeInputFilePath = options.InputFilePath ?? GetDefaultInputFilePath(day);
		string absoluteInputFilePath = Path.Combine(solutionsPath, relativeInputFilePath);
		string code = GetCode(day, splitSolver, className, @namespace, relativeInputFilePath);
		string input = "";

		WriteSolverFile(absoluteSolverFilePath, code);
		WriteInputFile(absoluteInputFilePath, input);
	}

	private static string GetDefaultClassName(int day) =>
		$"Day{day:D2}";
	private static string GetDefaultNamespace() =>
		"AdventOfCode.Solutions";

	private static string GetSolutionsProjectPath() {
		const EnvironmentVariableTarget target = EnvironmentVariableTarget.User;
		var path = Environment.GetEnvironmentVariable(AOCSolutionsPathEnvvar, target);
		if (path is null)
			throw new InitializerException($"Environment variable '{AOCSolutionsPathEnvvar}' does not exist.");
		if (!Directory.Exists(path))
			throw new FileNotFoundException($"Directory '{path}' does not exist.");
		return path;
	}
	private static string GetDefaultSolverFileName(string className) =>
		$"{className}.cs";
	private static string GetDefaultInputFilePath(int day) =>
		@$"input\{day:D2}.txt";

	private static string GetCode(int day, bool splitSolver, string className, string @namespace, string relativeInputFilePath) {
		if (!splitSolver) {
			return
@$"namespace {@namespace};

[Solver({day}, @""{relativeInputFilePath}"")]
public sealed class {className} : {nameof(ISolver)} {{
	
	public {nameof(CombinedSolution)} {nameof(ISolver.Solve)}(string input) {{
		throw new {nameof(NotImplementedException)}();
	}}
	
}}
";
		} else {
			return
@$"namespace {@namespace};

[Solver({day}, @""{relativeInputFilePath}"")]
public sealed class {className} : {nameof(ISplitSolver)} {{
	
	public {nameof(Part)} {nameof(ISplitSolver.SolvePart1)}(string input) {{
		throw new {nameof(NotImplementedException)}();
	}}

	public {nameof(Part)} {nameof(ISplitSolver.SolvePart2)}(string input) {{
		throw new {nameof(NotImplementedException)}();
	}}
	
}}
";
		}
	}
	private static string DownloadInput() {
		throw new NotImplementedException();
	}
	
	private static void WriteSolverFile(string filePath, string code) {
		File.WriteAllText(filePath, code);
	}
	private static void WriteInputFile(string filePath, string input) {
		File.WriteAllText(filePath, input);
	}



	/// <summary>
	/// Represents an exception caused by <see cref="Initializer"/>.
	/// </summary>
	public sealed class InitializerException : Exception {

		/// <summary>
		/// Initializes a new <see cref="InitializerException"/> instance.
		/// </summary>
		public InitializerException() { }
		/// <summary>
		/// Initializes a new <see cref="InitializerException"/> instance.
		/// </summary>
		/// <param name="message">The message describing the exception.</param>
		public InitializerException(string message) : base(message) { }
		/// <summary>
		/// Initializes a new <see cref="InitializerException"/> instance.
		/// </summary>
		/// <param name="message">The message describing the exception.</param>
		/// <param name="inner">The exception which caused the current exception.</param>
		public InitializerException(string message, Exception inner) : base(message, inner) { }

	}

}
