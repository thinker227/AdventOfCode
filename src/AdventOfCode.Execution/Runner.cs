using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AdventOfCode.Common;

namespace AdventOfCode.Execution;

internal static class Runner {

	private const string inputPath = @"res\input";
	private const string solutionsAssemblyName = "AdventOfCode.Solutions";



	public static ReadOnlySpan<char> GetInput(int day) {
		var path = @$"{Directory.GetCurrentDirectory()}\{inputPath}\{day:D2}.txt";
		if (!File.Exists(path))
			throw new FileNotFoundException($"Could not find input file for day {day} - '{path}' is missing.");
		return File.ReadAllText(path);
	}

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
			throw new Exception(message);
		}

		var solverType = solverTypes[0];
		var constructor = solverType.GetConstructor(Array.Empty<Type>());
		if (constructor is null)
			throw new Exception($"Type '{solverType.FullName}' does not contain a parameterless constructor.");
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

}
