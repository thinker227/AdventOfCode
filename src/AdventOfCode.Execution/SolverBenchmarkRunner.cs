using System;
using System.IO;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;
using AdventOfCode.Common;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Execution;

/// <summary>
/// Provides methods for benchmarking the performance of solvers.
/// </summary>
public static class SolverBenchmarkRunner {

	/// <summary>
	/// Runs benchmarks on a <see cref="ISolver"/> or <see cref="ISplitSolver"/>
	/// of a specified type.
	/// </summary>
	/// <param name="solverType">The type of solver to benchmark.</param>
	/// <param name="input">The input of the solver.</param>
	public static void RunBenchmarks(Type solverType, string input) {
#if DEBUG
		throw new Runner.RunnerException("Cannot run benchmarks while in debug configuration.");
#else
		WriteSolverInfo(solverType, input);
		if (solverType.IsSingleSolver())
			BenchmarkRunner.Run<SolverRunner>();
		else
			BenchmarkRunner.Run<SplitSolverRunner>();
#endif
	}

	private static void WriteSolverInfo(Type type, string input) {
		string solverType = type.AssemblyQualifiedName!;
		string solverInfoPath = GetSolverInfoPath();
		if (!Directory.Exists(solverInfoPath))
			Directory.CreateDirectory(solverInfoPath);
		File.WriteAllText(GetSolverTypePath(), solverType);
		File.WriteAllText(GetInputPath(), input);
	}
	private static (Type solverType, string input) ReadSolverInfo() {
		string solverTypePath = GetSolverTypePath();
		string inputPath = GetInputPath();
		if (!File.Exists(solverTypePath) || !File.Exists(inputPath))
			throw new FileNotFoundException($"Could not find files in '{GetSolverInfoPath()}'.");

		string solverTypeName = File.ReadAllText(solverTypePath);
		Type solverType = Type.GetType(solverTypeName)!;
		string input = File.ReadAllText(inputPath);

		return (solverType, input);
	}

	private static string GetTempPath() =>
		Environment.GetEnvironmentVariable("TEMP")!;
	private static string GetSolverInfoPath() =>
		Path.Combine(GetTempPath(), "AOCSolverInfo");
	private static string GetSolverTypePath() =>
		Path.Combine(GetSolverInfoPath(), "solverType");
	private static string GetInputPath() =>
		Path.Combine(GetSolverInfoPath(), "input");



	[MemoryDiagnoser]
	public class SolverRunner {

		private ISolver solver = null!;
		private string input = null!;



		[GlobalSetup]
		public void GlobalSetup() {
			var read = ReadSolverInfo();
			solver = Runner.CreateSolver(read.solverType);
			input = read.input;
		}

		[Benchmark]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Solve() {
			solver.Solve(input);
		}

	}

	[MemoryDiagnoser]
	public class SplitSolverRunner {

		private ISplitSolver solver = null!;
		private string input = null!;



		[GlobalSetup]
		public void GlobalSetup() {
			var read = ReadSolverInfo();
			solver = Runner.CreateSplitSolver(read.solverType);
			input = read.input;
		}

		[Benchmark]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SolvePart1() {
			solver.SolvePart1(input);
		}
		[Benchmark]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SolvePart2() {
			solver.SolvePart2(input);
		}

	}

}
