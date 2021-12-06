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
	/// <returns>A <see cref="SolverExecutionResult"/> instance containing
	/// information about the execution of the solver for <paramref name="day"/>.</returns>
	public static SolverExecutionResult RunSolver(ISolver solver, string input) {
		PartExecutionResult part1;
		PartExecutionResult part2;
		ExecutionType executionType;
		bool debug = Debugger.IsAttached;

		ISplitSolver? splitSolver = solver is SplitSolverWrapper wrapper ?
			wrapper.Solver : null;

		if (splitSolver is null) {
			executionType = ExecutionType.Combined;
			(part1, part2) = GetSingleExecutionResult(solver, input);
		} else {
			executionType = ExecutionType.Split;
			(part1, part2) = GetSplitExecutionResult(splitSolver, input);
		}

		return new(solver, executionType, part1, part2, debug);
	}
	private static (PartExecutionResult part1, PartExecutionResult part2) GetSingleExecutionResult(ISolver solver, string input) {
		CombinedSolution solution = default;
		Exception? exception = null;
		TimeSpan elapsed;
		Stopwatch sw = new();

		sw.Start();
		try {
			solution = solver.Solve(input);
		} catch (Exception e) {
			exception = e;
		}
		sw.Stop();
		elapsed = sw.Elapsed;

		if (solver is IDisposable disposable)
			disposable.Dispose();

		PartExecutionResult p1 = new(solution.Part1.ToString(), elapsed, exception);
		PartExecutionResult p2 = new(solution.Part2.ToString(), elapsed, exception);
		return (p1, p2);
	}
	private static (PartExecutionResult part1, PartExecutionResult part2) GetSplitExecutionResult(ISplitSolver splitSolver, string input) {
		Part part1Solution = default;
		Part part2Solution = default;
		(Exception? part1, Exception? part2) exception = (null, null);
		(TimeSpan part1, TimeSpan part2) elapsed;
		Stopwatch sw = new();

		sw.Start();
		try {
			part1Solution = splitSolver.SolvePart1(input);
		}
		catch (Exception e) {
			exception.part1 = e;
		}
		sw.Stop();
		elapsed.part1 = sw.Elapsed;

		sw.Reset();
		sw.Start();
		try {
			part2Solution = splitSolver.SolvePart1(input);
		}
		catch (Exception e) {
			exception.part2 = e;
		}
		sw.Stop();
		elapsed.part2 = sw.Elapsed;

		if (splitSolver is IDisposable disposable)
			disposable.Dispose();

		PartExecutionResult p1 = new(part1Solution.ToString(), elapsed.part1, exception.part1);
		PartExecutionResult p2 = new(part2Solution.ToString(), elapsed.part2, exception.part2);
		return (p1, p2);
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
	/// <exception cref="RunnerException">
	/// None or multiple solvers are found for the specified day, or
	/// the solver for the specified day does not contains a parameterless constructor.
	/// </exception>
	public static ISolver GetSolver(int day, Assembly assembly) {
		var solverType = GetSolverType(day, assembly);
		var instance = CreateSolver(solverType);
		return instance;
	}
	/// <summary>
	/// Get the <see cref="Type"/> of the solver for a specified day-
	/// </summary>
	/// <param name="day">The day to get the type of solver for.</param>
	/// <param name="assembly">The <see cref="Assembly"/> to load the
	/// type of the solver from.</param>
	/// <returns>The <see cref="Type"/> of the solver for the specified day.</returns>
	/// <exception cref="RunnerException">
	/// None or multiple solver types are found for the specified day.
	/// </exception>
	public static Type GetSolverType(int day, Assembly assembly) {
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

		return solverTypes[0];
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
		return type.IsSolver();
	}
	/// <summary>
	/// Creates an <see cref="ISolver"/> from a specified <see cref="Type"/>.
	/// </summary>
	/// <param name="solverType">The type of solver to create.</param>
	/// <returns>A new instance of <paramref name="solverType"/>.</returns>
	/// <exception cref="RunnerException">
	/// <paramref name="solverType"/> does not implement <see cref="ISolver"/>,
	/// or <paramref name="solverType"/> does not contains a parameterless constructor.
	/// </exception>
	public static ISolver CreateSolver(Type solverType) {
		if (!solverType.IsSingleSolver())
			throw new RunnerException($"'{solverType.FullName}' does not implement {nameof(ISolver)}.");
		var instance = (ISolver)CreateInstance(solverType);
		return instance;
	}
	/// <summary>
	/// Creates an <see cref="ISplitSolver"/> from a specified <see cref="Type"/>.
	/// </summary>
	/// <param name="solverType">The type of solver to create.</param>
	/// <returns>A new instance of <paramref name="solverType"/>.</returns>
	/// <exception cref="RunnerException">
	/// <paramref name="solverType"/> does not implement <see cref="ISplitSolver"/>,
	/// or <paramref name="solverType"/> does not contains a parameterless constructor.
	/// </exception>
	public static ISplitSolver CreateSplitSolver(Type solverType) {
		if (!solverType.IsSplitSolver())
			throw new RunnerException($"'{solverType.FullName}' does not implement {nameof(ISplitSolver)}.");
		var instance = (ISplitSolver)CreateInstance(solverType);
		return instance;
	}
	/// <summary>
	/// Creates an <see cref="ISolver"/> or <see cref="SplitSolverWrapper"/>
	/// from a specified <see cref="Type"/>.
	/// </summary>
	/// <param name="solverType">The type of solver to create.</param>
	/// <returns>A new instance of <paramref name="solverType"/> or a
	/// <see cref="SplitSolverWrapper"/> if <paramref name="solverType"/>
	/// implements <see cref="ISplitSolver"/>.</returns>
	/// <exception cref="RunnerException">
	/// <paramref name="solverType"/> does not implement
	/// <see cref="ISolver"/> or <see cref="ISplitSolver"/>,
	/// or <paramref name="solverType"/> does not contains a parameterless constructor.
	/// </exception>
	public static ISolver CreateSolverOrWrapper(Type solverType) {
		return solverType.IsSingleSolver() ?
			CreateSolver(solverType) :
			CreateSplitSolver(solverType).ToSolver();
	}
	private static object CreateInstance(Type type) {
		var constructor = type.GetConstructor(Array.Empty<Type>());
		if (constructor is null)
			throw new RunnerException($"Type '{type.FullName}' does not contain a parameterless constructor.");
		var instance = constructor.Invoke(Array.Empty<object>());
		return instance;
	}

	/// <summary>
	/// Gets the input of a specified solver type.
	/// </summary>
	/// <param name="solverType">The solver to get the input of.</param>
	/// <returns>The input of <paramref name="solverType"/>.</returns>
	/// <exception cref="RunnerException">
	/// <paramref name="solverType"/> is not attributed with <see cref="SolverAttribute"/>.
	/// </exception>
	/// <exception cref="FileNotFoundException">
	/// The file specified by <see cref="SolverAttribute.InputPath"/> does not exist.
	/// </exception>
	public static string GetInput(Type solverType) {
		var attribute = solverType.GetSolverAttribute();
		var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		var path = $@"{directory}\{attribute.InputPath}";
		if (!File.Exists(path))
			throw new FileNotFoundException($"No input file for solver type '{solverType.FullName}' was found.", path);
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
	/// Contains information about the execution of a part solution.
	/// </summary>
	/// <param name="Solution">The string representation of the solution.</param>
	/// <param name="ElapsedTime">The elapsed time the solution took to execute.</param>
	/// <param name="Exception">The possible exception which occured during execution.</param>
	public readonly record struct PartExecutionResult(string? Solution, TimeSpan ElapsedTime, Exception? Exception) {

		/// <summary>
		/// Whether there is a solution.
		/// </summary>
		public bool HasSolution => Solution is not null;
		/// <summary>
		/// Whether an exception was raised during execution.
		/// </summary>
		public bool HasException => Exception is not null;

	}

	/// <summary>
	/// Contains information about the execution of an <see cref="ISolver"/>.
	/// </summary>
	/// <param name="Solver">The <see cref="ISolver"/> which generated the solution.</param>
	/// <param name="Part1">The information about the execution of part 1.
	/// If <paramref name="Part2"/> is <see langword="null"/> then
	/// contains information about the combined execution of part 1 and part 2.</param>
	/// <param name="Part2">The solution to part 1 of the puzzle.</param>
	/// <param name="Debug">Whether the solver was executed while a debugger was attached.</param>
	public readonly record struct SolverExecutionResult(ISolver Solver, ExecutionType ExecutionType, PartExecutionResult Part1, PartExecutionResult Part2, bool Debug);

	/// <summary>
	/// Represents the type of execution of a <see cref="SolverExecutionResult"/>.
	/// </summary>
	public enum ExecutionType {
		/// <summary>
		/// The execution was a combined execution through an <see cref="ISolver"/>.
		/// </summary>
		Combined,
		/// <summary>
		/// The execution was a split execution through an <see cref="ISplitSolver"/>.
		/// </summary>
		Split
	}

}
