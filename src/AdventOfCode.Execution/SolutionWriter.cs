using System;
using System.Diagnostics;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Execution;

/// <summary>
/// Contains methods to write a
/// <see cref="Runner.SolverExecutionResult"/> to the console.
/// </summary>
public static class SolutionWriter {

	/* Color legend
	White:		Titles and important text
	DarkGray:	Labels and separation
	Yellow:		Solutions
	Magenta:	Type names
	Cyan:		Numbers
	Red:		Errors
	*/

	/// <summary>
	/// Writes a <see cref="Runner.SolverExecutionResult"/> to the console.
	/// </summary>
	/// <param name="solutionResult">The
	/// <see cref="Runner.SolverExecutionResult"/> to write.</param>
	public static void WriteSolution(Runner.SolverExecutionResult solutionResult) {
		// -- Day {day} --
		int day = solutionResult.Solver.GetDay();
		Text.FromString($"-- Day {day} --")
			.WithColor(ConsoleColor.White)
			.WithNewline()
			.Write();

		// Solver: {solver type}
		Text.FromString($"Solver: ")
			.WithColor(ConsoleColor.DarkGray)
			.Append($"{solutionResult.Solver.GetSolverType().FullName}")
			.WithColor(ConsoleColor.Magenta)
			.WithNewline()
			.Write();

		// Input: "{input (...)?}"
		GetFormattedInput(solutionResult.Input)
			.Write();

		// Website: {uri}
		Text.FromString("Website: ")
			.WithColor(ConsoleColor.DarkGray)
			.Append(Runner.GetWebsiteUri(day).ToString())
			.WithColor(ConsoleColor.White)
			.WithNewline()
			.Append("\n")
			.Write();

		if (solutionResult.ExecutionType == Runner.ExecutionType.Combined) {
			Text text = Text.FromString("Solutions:")
					.WithColor(ConsoleColor.White)
					.WithNewline();
			var elapsed = GetFormattedElapsedTime(solutionResult.Part1.ElapsedTime);

			if (solutionResult.Part1.HasException) {
				// Solutions:
				// {elapsed time}
				// {exception}
				text.Append(elapsed)
					.Append(GetFormattedException(solutionResult.Part1.Exception!))
					.Write();
			} else {
				// Solutions:
				// {part 1}
				// {part 2}
				// {elapsed time}
				text.Append(GetFormattedSolution(solutionResult.Part1.Solution, "Part 1"))
					.Append(GetFormattedSolution(solutionResult.Part2.Solution, "Part 2"))
					.Append(elapsed)
					.Write();
			}
		} else {
			// Part 1: {solution}
			GetFormattedPart(solutionResult.Part1, 1)
				.Append("\n")
				.Write();
			// Part 2: {solution}
			GetFormattedPart(solutionResult.Part2, 2)
				.Write();
		}
	}

	private static Text GetFormattedInput(string input) {
		const int maxInputLength = 80;
		string formattedInput = input.Replace("\n", "\\n");
		bool overflow = formattedInput.Length > maxInputLength;
		formattedInput = overflow ?
			formattedInput[..(maxInputLength - 3)] : formattedInput;
		formattedInput = string.Concat(formattedInput
			.Where(c => !char.IsControl(c)));

		Text overflowText = Text.FromString("...")
			.WithColor(ConsoleColor.DarkGray);
		// Input: {formatted input}
		return Text.FromString("Input: \"")
			.WithColor(ConsoleColor.DarkGray)
			.Append(formattedInput)
			.WithColor(ConsoleColor.White)
			.Append(overflow ? overflowText : null)
			.Append("\"")
			.WithColor(ConsoleColor.DarkGray)
			.WithNewline();
	}
	private static Text GetFormattedElapsedTime(TimeSpan elapsed) {
		// Elapsed time: {elapsed}
		return Text.FromString("Elapsed time: ")
			.WithColor(ConsoleColor.DarkGray)
			.Append(elapsed.ToString())
			.WithColor(ConsoleColor.Cyan)
			.WithNewline();
	}
	private static Text GetFormattedSolution(string? solution, string solutionName) {
		// {solution name}: {solution}
		return Text.FromString($"{solutionName}: ")
				.WithColor(ConsoleColor.DarkGray)
				.Append(solution)
				.WithColor(ConsoleColor.Yellow)
				.WithNewline();
	}
	private static Text GetFormattedException(Exception exception) {
		// Exception {type name}: {message}
		// {stack trace}
		return Text.FromString("Exception ")
				.WithColor(ConsoleColor.DarkGray)
				.Append(exception.GetType().FullName)
				.WithColor(ConsoleColor.Magenta)
				.Append(": ")
				.WithColor(ConsoleColor.DarkGray)
				.Append(exception.Message)
				.WithColor(ConsoleColor.Red)
				.WithNewline()
				.Append(GetFormattedExceptionStackTrace(exception));
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
				.WithColor(ConsoleColor.Magenta)
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
	private static Text GetFormattedPart(Runner.PartExecutionResult executionResult, int part) {
		// Part {part}:
		Text text = Text.FromString($"Part {part}:")
			.WithColor(ConsoleColor.White)
			.WithNewline();
		Text elapsed = GetFormattedElapsedTime(executionResult.ElapsedTime);

		text = executionResult.HasException ?
			// {elapsed time}
			// {exception}
			text.Append(elapsed)
				.Append(GetFormattedException(executionResult.Exception!)) :
			// {solution}
			// {elapsed time}
			text.Append(GetFormattedSolution(executionResult.Solution, "Solution"))
				.Append(elapsed);

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
		public Text Append(Text? text) {
			if (text is null) return this;
			text.Root().parent = this;
			return text;
		}
		public Text Append(string? str) {
			Text appended = new();
			appended.parent = this;
			appended.str = str;
			appended.color = color;
			return appended;
		}
		public Text Root() =>
			parent?.Root() ?? this;

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
