﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using AdventOfCode.Common;

namespace AdventOfCode.Execution;

/// <summary>
/// Contains methods for running and retrieving solutions.
/// </summary>
public static class Runner {

	private const string inputPath = @"res\input";
	private const string solutionsAssemblyName = "AdventOfCode.Solutions";



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
		return new(solver, solution, elapsedTime, exception);
	}

	/// <summary>
	/// Gets the input of a specified day.
	/// </summary>
	/// <param name="day">The day to get the input of.</param>
	/// <returns>A <see cref="ReadOnlySpan{T}"/> of characters representing the input.</returns>
	/// <exception cref="FileNotFoundException">
	/// No input file for the specified day exists.
	/// </exception>
	public static string GetInput(int day) {
		var path = @$"{inputPath}\{day:D2}.txt";
		if (!File.Exists(path))
			throw new FileNotFoundException($"Could not find input file for day {day} - '{path}' is missing.");
		return File.ReadAllText(path);
	}

	/// <summary>
	/// Gets the solver for a specified day.
	/// </summary>
	/// <param name="day">The day to get the solver for.</param>
	/// <returns>An <see cref="ISolver"/> for the specified day.</returns>
	/// <exception cref="InvalidOperationException">
	/// None or multiple solvers are found for the specified day or
	/// the solver for the specified day does not contains a parameterless constructor.
	/// </exception>
	public static ISolver GetSolver(int day) {
		var types = GetSolverTypes();
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
			throw new InvalidOperationException(message);
		}

		var solverType = solverTypes[0];
		var constructor = solverType.GetConstructor(Array.Empty<Type>());
		if (constructor is null)
			throw new InvalidOperationException($"Type '{solverType.FullName}' does not contain a parameterless constructor.");
		var instance = constructor.Invoke(Array.Empty<object>());
		return (ISolver)instance;
	}
	private static Type[] GetSolverTypes() =>
		Assembly.Load(solutionsAssemblyName)
			.GetTypes()
			.Where(IsSolverType)
			.ToArray();
	private static bool IsSolverType(Type type) {
		if (type.GetCustomAttribute<SolverAttribute>() is null) return false;
		return type
			.GetInterfaces()
			.Contains(typeof(ISolver));
	}



	/// <summary>
	/// Contains information about the execution of an <see cref="ISolver"/>.
	/// </summary>
	/// <param name="Solver">The <see cref="ISolver"/> which generated the solution.</param>
	/// <param name="Solution">The generated solution.</param>
	/// <param name="ElapsedTime">The elapsed time the solution took to execute.</param>
	/// <param name="Exception">The possible exception which occured during the solution execution.</param>
	public readonly record struct SolutionExecutionResult(ISolver Solver, Solution Solution, TimeSpan ElapsedTime, Exception? Exception) {

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