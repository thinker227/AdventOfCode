using System;
using System.Reflection;
using CommandLine;
using AdventOfCode.Execution;



ExecutionOptions options = null!;
Parser.Default.ParseArguments<ExecutionOptions>(args)
	.WithParsed(e => options = e);

int day = options.Day ?? DateTime.Now.Day;
if (day is < 1 or > 25)
	throw new FormatException($"Invalid day {day}.");

var solver = Runner.GetSolver(day, Assembly.Load("AdventOfCode.Solutions"));
string input = options.Input ?? Runner.GetInput(solver);
var solutionResult = Runner.RunSolver(solver, input);

SolutionWriter.WriteSolution(solutionResult);
