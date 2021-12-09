using System;
using System.Reflection;
using AdventOfCode.Execution;



var executionOptions = ExecutionOptions.GetOptions(args);
ExecutionOptions.RunOptions? runOptions = executionOptions.Run;

if (executionOptions.Init is not null) {
	Initializer.Initialize(executionOptions.Init);
	return;
}

int day = runOptions?.Day ?? Runner.GetDay();
if (day is < 1 or > 25)
	throw new FormatException($"Invalid day {day}.");

var assembly = Assembly.Load("AdventOfCode.Solutions");
var solverType = Runner.GetSolverType(day, assembly);
string input = runOptions?.Input ?? Runner.GetInput(solverType);

if (runOptions?.Benchmark ?? false) {
	SolverBenchmarkRunner.RunBenchmarks(solverType, input);
	return;
}

var solver = Runner.CreateSolverOrWrapper(solverType);
var solutionResult = Runner.RunSolver(solver, input);
SolutionWriter.WriteSolution(solutionResult);
