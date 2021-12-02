using System;
using System.Reflection;
using AdventOfCode.Execution;



int day = DateTime.Now.Day;
if (args.Length >= 1) {
	if (int.TryParse(args[0], out int result))
		day = result;
	else Console.WriteLine($"Could not parse argument '{args[0]}'.");
}
if (day is < 1 or > 25)
	throw new FormatException($"Invalid day {day}.");

var solver = Runner.GetSolver(day, Assembly.Load("AdventOfCode.Solutions"));
string input = args.Length >= 2 ? args[1] : Runner.GetInput(solver);
var solutionResult = Runner.RunSolver(solver, input);

SolutionWriter.WriteSolution(solutionResult);
