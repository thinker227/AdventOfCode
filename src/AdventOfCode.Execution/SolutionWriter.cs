using System;
using System.Diagnostics;
using AdventOfCode.Common;

namespace AdventOfCode.Execution;

/// <summary>
/// Contains methods to write a
/// <see cref="Runner.SolverExecutionResult"/> to the console.
/// </summary>
public static class SolutionWriter {

	/// <summary>
	/// Writes a <see cref="Runner.SolverExecutionResult"/> to the console.
	/// </summary>
	/// <param name="solutionResult">The
	/// <see cref="Runner.SolverExecutionResult"/> to write.</param>
	public static void WriteSolution(Runner.SolverExecutionResult solutionResult) {
		// -- Day {day} --
		Text.FromString($"-- Day {solutionResult.Solver.GetDay()} --")
			.WithColor(ConsoleColor.White)
			.WithNewline()
			.Write();

		// Solver: {solver type}
		Text.FromString($"Solver: ")
			.WithColor(ConsoleColor.DarkGray)
			.Append($"{solutionResult.Solver.GetSolverType().FullName}")
			.WithColor(ConsoleColor.White)
			.WithNewline()
			.Write();

		if (solutionResult.ExecutionType == Runner.ExecutionType.Combined) {

		} else {

		}
	}

	private static Text GetFormattedExceptionStackTrace(Exception exception) {
		Text text = new();
		var frames = new StackTrace(exception, true).GetFrames();
		
		foreach (var frame in frames) {
			if (frame.GetMethod()?.DeclaringType == typeof(Runner)) break;

			string indentation = new(' ', 4);
			var method = frame.GetMethod();
			string type = method?.DeclaringType?.FullName ?? "Unknown type";
			string methodString = method?.ToString() ?? "unknown method";
			string lineNumber = frame.GetFileLineNumber().ToString();
			string? fileName = frame.GetFileName();

			// {solver type}: method {method} line {line number}
			text = text
				.Append(indentation)
				.Append(type)
				.WithColor(ConsoleColor.White)
				.Append(": method ")
				.WithColor(ConsoleColor.DarkGray)
				.Append(methodString)
				.WithColor(ConsoleColor.White)
				.Append(" line ")
				.WithColor(ConsoleColor.DarkGray)
				.Append(lineNumber)
				.WithColor(ConsoleColor.Cyan)
				.WithNewline();
		}

		return text;
	}



	private sealed class Text {

		private string? str;
		private bool newline;
		private ConsoleColor? color;
		private Text? parent;

		public static Text FromString(string? str) =>
			new Text().WithString(str);
		public Text WithString(string? str) {
			this.str = str;
			return this;
		}
		public Text WithColor(ConsoleColor color) {
			this.color = color;
			return this;
		}
		public Text WithNewline() {
			newline = true;
			return this;
		}
		public Text Append(string? str) {
			Text appended = new();
			appended.parent = this;
			appended.str = str;
			appended.color = color;
			return appended;
		}

		public void Write() {
			parent?.Write();

			ConsoleColor prev = Console.ForegroundColor;
			if (color is not null) Console.ForegroundColor = color.Value;
			Console.Write(str);
			Console.ForegroundColor = prev;

			if (newline) Console.WriteLine();
		}

	}

}
