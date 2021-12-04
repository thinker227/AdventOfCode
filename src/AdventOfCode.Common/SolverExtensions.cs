using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdventOfCode.Common;

public static class SolverExtensions {

	private static readonly Dictionary<Type, int> solverDays = new();
	private static int GetDay(Type type) {
		if (solverDays.TryGetValue(type, out int value))
			return value;

		var attribute = GetSolverAttribute(type);
		solverDays.Add(type, attribute.Day);
		return attribute.Day;
	}
	/// <summary>
	/// Gets the day of a specified <see cref="ISolver"/>.
	/// </summary>
	/// <remarks>
	/// If <paramref name="solver"/> is a <see cref="SplitSolverWrapper"/>
	/// then the day of the wrapped <see cref="ISplitSolver"/> will be returned.
	/// </remarks>
	/// <param name="solver">The <see cref="ISolver"/> to get the day of.</param>
	/// <returns>The day of <paramref name="solver"/>.</returns>
	/// <exception cref="InvalidOperationException">
	/// <paramref name="solver"/> is not attributed with <see cref="SolverAttribute"/>.
	/// </exception>
	public static int GetDay(this ISolver solver) {
		Type type = solver is SplitSolverWrapper wrapper ?
			wrapper.Solver.GetType() : solver.GetType();
		return GetDay(type);
	}
	/// <summary>
	/// Gets the day of a specified solver <see cref="ISplitSolver"/>.
	/// </summary>
	/// <param name="solver">The <see cref="ISplitSolver"/> to get the day of.</param>
	/// <returns>The day of <paramref name="solver"/>.</returns>
	/// <exception cref="InvalidOperationException">
	/// <paramref name="solver"/> is not attributed with <see cref="SolverAttribute"/>.
	/// </exception>
	public static int GetDay(this ISplitSolver splitSolver) =>
		GetDay(splitSolver.GetType());

	/// <summary>
	/// Gets the <see cref="SolverAttribute"/> of a specified type.
	/// </summary>
	/// <param name="solver">The type
	/// to get the <see cref="SolverAttribute"/> of.</param>
	/// <returns>The <see cref="SolverAttribute"/> of <paramref name="type"/>.</returns>
	/// <exception cref="InvalidOperationException">
	/// <paramref name="type"/> is not attributed with <see cref="SolverAttribute"/>.
	/// </exception>
	public static SolverAttribute GetSolverAttribute(this Type type) {
		var attribute = type.GetCustomAttribute<SolverAttribute>();
		if (attribute is null) throw new InvalidOperationException($"Solver type {type.FullName} is not attributed with {nameof(SolverAttribute)}.");
		return attribute;
	}
	/// <summary>
	/// Gets the <see cref="SolverAttribute"/> of a specified <see cref="ISolver"/>.
	/// </summary>
	/// <remarks>
	/// If <paramref name="solver"/> is a <see cref="SplitSolverWrapper"/>
	/// then the <see cref="SolverAttribute"/> of the
	/// wrapped <see cref="ISplitSolver"/> will be returned.
	/// </remarks>
	/// <param name="solver">The <see cref="ISolver"/>
	/// to get the <see cref="SolverAttribute"/> of.</param>
	/// <returns>The <see cref="SolverAttribute"/> of <paramref name="solver"/>.</returns>
	/// <exception cref="InvalidOperationException">
	/// <paramref name="solver"/> is not attributed with <see cref="SolverAttribute"/>.
	/// </exception>
	public static SolverAttribute GetSolverAttribute(this ISolver solver) {
		Type type = GetSolverType(solver);
		return GetSolverAttribute(type);
	}
	/// <summary>
	/// Gets the <see cref="SolverAttribute"/> of a specified <see cref="ISolver"/>.
	/// </summary>
	/// <param name="splitSolver">The <see cref="ISolver"/>
	/// to get the <see cref="SolverAttribute"/> of.</param>
	/// <returns>The <see cref="SolverAttribute"/> of <paramref name="splitSolver"/>.</returns>
	/// <exception cref="InvalidOperationException">
	/// <paramref name="splitSolver"/> is not attributed with <see cref="SolverAttribute"/>.
	/// </exception>
	public static SolverAttribute GetSolverAttribute(this ISplitSolver splitSolver) =>
		GetSolverAttribute(splitSolver.GetType());

	/// <summary>
	/// Gets whether a specified <see cref="Type"/> implements
	/// either <see cref="ISolver"/> or <see cref="ISplitSolver"/>.
	/// </summary>
	/// <param name="type">The type to check.</param>
	/// <returns>Whether <paramref name="type"/> implements
	/// either <see cref="ISolver"/> or <see cref="ISplitSolver"/>.</returns>
	public static bool IsSolver(this Type type) =>
		type.GetInterfaces()
			.Any(t => t == typeof(ISolver) || t == typeof(ISplitSolver));
	/// <summary>
	/// Gets whether a specified <see cref="Type"/> implements
	/// <see cref="ISolver"/>.
	/// </summary>
	/// <param name="type">The type to check.</param>
	/// <returns>Whether <paramref name="type"/> implements
	/// <see cref="ISolver"/>.</returns>
	public static bool IsSingleSolver(this Type type) =>
		type.GetInterfaces()
			.Contains(typeof(ISolver));
	/// <summary>
	/// Gets whether a specified <see cref="Type"/> implements
	/// <see cref="ISplitSolver"/>.
	/// </summary>
	/// <param name="type">The type to check.</param>
	/// <returns>Whether <paramref name="type"/> implements
	/// <see cref="ISplitSolver"/>.</returns>
	public static bool IsSplitSolver(this Type type) =>
		type.GetInterfaces()
			.Contains(typeof(ISplitSolver));

	/// <summary>
	/// Gets the <see cref="Type"/> of a specified <see cref="ISolver"/>.
	/// </summary>
	/// <param name="solver">The <see cref="ISolver"/> to get the type of.</param>
	/// <returns>The type of <paramref name="solver"/>, or the type of the wrapped
	/// <see cref="ISplitSolver"/> if <paramref name="solver"/>
	/// is a <see cref="SplitSolverWrapper"/>.</returns>
	public static Type GetSolverType(this ISolver solver) =>
		solver is SplitSolverWrapper wrapper ?
			wrapper.Solver.GetType() : solver.GetType();

	/// <summary>
	/// Converts a <see cref="ISplitSolver"/> to a <see cref="ISolver"/>.
	/// </summary>
	/// <param name="solver">The <see cref="ISplitSolver"/> to convert.</param>
	/// <returns>An <see cref="ISolver"/> wrapper of
	/// <paramref name="solver"/>.</returns>
	/// <exception cref="InvalidOperationException">
	/// <paramref name="solver"/> is not attributed with <see cref="SolverAttribute"/>.
	/// </exception>
	public static ISolver ToSolver(this ISplitSolver splitSolver) =>
		new SplitSolverWrapper(splitSolver);

}
