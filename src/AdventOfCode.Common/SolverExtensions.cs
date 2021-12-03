using System;
using System.Collections.Generic;
using System.Reflection;

namespace AdventOfCode.Common;

public static class SolverExtensions {

	private static readonly Dictionary<Type, int> solverDays = new();
	private static int GetDay(Type type) {
		if (solverDays.TryGetValue(type, out int value))
			return value;

		var attribute = type.GetCustomAttribute<SolverAttribute>();
		if (attribute is null) throw new InvalidOperationException($"Solver type {type.FullName} is not attributed with {nameof(SolverAttribute)}.");
		solverDays.Add(type, attribute.Day);
		return attribute.Day;
	}
	/// <summary>
	/// Gets the day of a specified solver.
	/// </summary>
	/// <param name="solver">The <see cref="ISolver"/> to get the day of.</param>
	/// <returns>The day of <paramref name="solver"/>.</returns>
	/// <exception cref="InvalidOperationException">
	/// <paramref name="solver"/> is not attributed with <see cref="SolverAttribute"/>.
	/// </exception>
	public static int GetDay(this ISolver solver) =>
		GetDay(solver.GetType());
	/// <summary>
	/// Gets the day of a specified solver.
	/// </summary>
	/// <param name="solver">The <see cref="IDualSolver"/> to get the day of.</param>
	/// <returns>The day of <paramref name="solver"/>.</returns>
	/// <exception cref="InvalidOperationException">
	/// <paramref name="solver"/> is not attributed with <see cref="SolverAttribute"/>.
	/// </exception>
	public static int GetDay(this IDualSolver dualSolver) =>
		GetDay(dualSolver.GetType());

	/// <summary>
	/// Converts a <see cref="IDualSolver"/> to a <see cref="ISolver"/>.
	/// </summary>
	/// <param name="solver">The <see cref="IDualSolver"/> to convert.</param>
	/// <returns>An <see cref="ISolver"/> wrapper of
	/// <paramref name="solver"/>.</returns>
	/// <exception cref="InvalidOperationException">
	/// <paramref name="solver"/> is not attributed with <see cref="SolverAttribute"/>.
	/// </exception>
	public static ISolver ToSolver(this IDualSolver dualSolver) =>
		new DualSolverWrapper(dualSolver);

}
