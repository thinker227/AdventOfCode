using System;
using System.Collections.Generic;
using System.Reflection;

namespace AdventOfCode.Common;

public static class SolverExtensions {

	private static readonly Dictionary<Type, int> solverDays = new();
	/// <summary>
	/// Gets the day of a specified solver.
	/// </summary>
	/// <param name="solver">The <see cref="ISolver"/> to get the day of.</param>
	/// <returns>The day of <paramref name="solver"/>.</returns>
	/// <exception cref="InvalidOperationException">
	/// <paramref name="solver"/> is not attributed with <see cref="SolverAttribute"/>.
	/// </exception>
	public static int GetDay(this ISolver solver) {
		Type type = solver.GetType();
		if (solverDays.TryGetValue(type, out int value))
			return value;

		var attribute = type.GetCustomAttribute<SolverAttribute>();
		if (attribute is null) throw new InvalidOperationException($"Solver type {type.FullName} is not attributed with {nameof(SolverAttribute)}.");
		solverDays.Add(type, attribute.Day);
		return attribute.Day;
	} 

}
