using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;	
using AdventOfCode.Common;

namespace AdventOfCode.Execution;

/// <summary>
/// Contains methods for running and retrieving solutions.
/// </summary>
public static class Runner {

	/// <summary>
	/// Runs the solver for a specified day.
	/// </summary>
	/// <param name="day">The day to run the solver of.</param>
	/// <returns>A <see cref="SolutionExecutionResult"/> instance containing
	/// information about the execution of the solver for <paramref name="day"/>.</returns>
	public static SolutionExecutionResult RunSolver(ISolver solver, string? input) {
		Solution solution = default;
		TimeSpan elapsedTime;
		Exception? exception = null;
		bool debug = Debugger.IsAttached;

		Stopwatch sw = new();
		sw.Start();

		try {
			solution = solver.Solve(input);
		} catch (Exception e) {
			exception = e;
		} finally {
			sw.Stop();
			if (solver is IDisposable disposable)
				disposable.Dispose();
		}

		elapsedTime = sw.Elapsed;
		return new(solver, solution, elapsedTime, exception, debug);
	}

	/// <summary>
	/// Gets the current day.
	/// </summary>
	/// <returns>The current day as supplied <see cref="DateTime.Now"/>.</returns>
	public static int GetDay() =>
		DateTime.Now.Day;

	/// <summary>
	/// Gets an <see cref="ISolver"/> for a specified day.
	/// </summary>
	/// <param name="day">The day to get the solver for.</param>
	/// <param name="assembly">The <see cref="Assembly"/> to load the solver from.</param>
	/// <returns>An <see cref="ISolver"/> for the specified day.</returns>
	/// <exception cref="InvalidOperationException">
	/// None or multiple solvers are found for the specified day or
	/// the solver for the specified day does not contains a parameterless constructor.
	/// </exception>
	public static ISolver GetSolver(int day, Assembly assembly) {
		var types = GetSolverTypes(assembly);
		var solverTypes = types
			.Where(t => t.GetCustomAttribute<SolverAttribute>()!.Day == day)
			.ToArray();
		if (solverTypes.Length != 1) {
			string message;
			if (solverTypes.Length == 0)
				message = $"No solver type for day {day} was found.";
			else {
				string typesString = string.Join(", ", solverTypes.Select(t => $"'{t.FullName}'"));
				message = $"Multiple solvers types for day {day} were found: {typesString}.";
			}
			throw new RunnerException(message);
		}

		var solverType = solverTypes[0];
		var instance = CreateSolver(solverType);
		return instance;
	}
	/// <summary>
	/// Gets all solvers in a specified <see cref="Assembly"/>.
	/// </summary>
	/// <param name="assembly">The <see cref="Assembly"/> to get the solvers in.</param>
	/// <returns>A collection of solvers in <paramref name="assembly"/>.</returns>
	public static ImmutableArray<ISolver> GetAllSolvers(Assembly assembly) {
		return GetSolverTypes(assembly)
			.Select(t => CreateSolver(t))
			.ToImmutableArray();
	}
	private static ImmutableArray<Type> GetSolverTypes(Assembly assembly) =>
		assembly.GetTypes()
			.Where(IsSolverType)
			.ToImmutableArray();
	private static bool IsSolverType(Type type) {
		if (type.GetCustomAttribute<SolverAttribute>() is null) return false;
		return type
			.GetInterfaces()
			.Any(t => t == typeof(ISolver) || t == typeof(IDualSolver));
	}
	private static ISolver CreateSolver(Type solverType) {
		var constructor = solverType.GetConstructor(Array.Empty<Type>());
		if (constructor is null)
			throw new RunnerException($"Type '{solverType.FullName}' does not contain a parameterless constructor.");
		var instance = constructor.Invoke(Array.Empty<object>());
		return instance is ISolver solver ?
			solver : ((IDualSolver)instance).ToSolver();
	}

	/// <summary>
	/// Gets the input of a specified solver.
	/// </summary>
	/// <param name="solver">The solver to get the input of.</param>
	/// <returns>The input of <paramref name="solver"/>.</returns>
	/// <exception cref="RunnerException">
	/// <paramref name="solver"/> is not attributed with <see cref="SolverAttribute"/>.
	/// </exception>
	/// <exception cref="FileNotFoundException">
	/// The file specified by <see cref="SolverAttribute.InputPath"/> does not exist.
	/// </exception>
	public static string GetInput(ISolver solver) {
		var type = solver.GetSolverType();
		var attribute = solver.GetSolverAttribute();
		var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		var path = $@"{directory}\{attribute.InputPath}";
		if (!File.Exists(path))
			throw new FileNotFoundException($"No input file for solver type '{type.FullName}' was found.", path);
		return File.ReadAllText(path);
	}



	/// <summary>
	/// Represents an exception caused by <see cref="Runner"/>.
	/// </summary>
	public sealed class RunnerException : Exception {

		/// <summary>
		/// Initializes a new <see cref="RunnerException"/> instance.
		/// </summary>
		public RunnerException() { }
		/// <summary>
		/// Initializes a new <see cref="RunnerException"/> instance.
		/// </summary>
		/// <param name="message">The message describing the exception.</param>
		public RunnerException(string message) : base(message) { }
		/// <summary>
		/// Initializes a new <see cref="RunnerException"/> instance.
		/// </summary>
		/// <param name="message">The message describing the exception.</param>
		/// <param name="inner">The exception which caused the current exception.</param>
		public RunnerException(string message, Exception inner) : base(message, inner) { }

	}

	/// <summary>
	/// Contains information about the execution of an <see cref="ISolver"/>.
	/// </summary>
	/// <param name="Solver">The <see cref="ISolver"/> which generated the solution.</param>
	/// <param name="Solution">The generated solution.</param>
	/// <param name="ElapsedTime">The elapsed time the solution took to execute.</param>
	/// <param name="Exception">The possible exception which occured during the solution execution.</param>
	public readonly record struct SolutionExecutionResult(ISolver Solver, Solution Solution, TimeSpan ElapsedTime, Exception? Exception, bool Debug) {

		/// <summary>
		/// Whether an exception was raised during execution.
		/// </summary>
		public bool HasException => Exception is not null;
		/// <summary>
		/// Whether the solution has a part 1.
		/// </summary>
		public bool HasPart1 => Solution.Part1 is not null;
		/// <summary>
		/// Whether the solution has a part 2.
		/// </summary>
		public bool HasPart2 => Solution.Part2 is not null;

	}

}
