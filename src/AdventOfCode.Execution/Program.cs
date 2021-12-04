using System;
using System.Reflection;
using AdventOfCode.Execution;



var options = ExecutionOptions.GetOptions();

int day = options.Day ?? Runner.GetDay();
if (day is < 1 or > 25)
	throw new FormatException($"Invalid day {day}.");

var assembly = Assembly.Load("AdventOfCode.Solutions");
var solverType = Runner.GetSolverType(day, assembly);
string input = options.Input ?? Runner.GetInput(solverType);

if (options.Benchmark) {
	SolverBenchmarkRunner.RunBenchmarks(solverType, input);
	return;
}

var solver = Runner.CreateSolverOrWrapper(solverType);
var solutionResult = Runner.RunSolver(solver, input);
SolutionWriter.WriteSolution(solutionResult);
