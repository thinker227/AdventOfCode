using System;
using AdventOfCode.Common;

namespace AdventOfCode.Execution;

/// <summary>
/// Contains methods to write a
/// <see cref="Runner.SolutionExecutionResult"/> to the console.
/// </summary>
public static class SolutionWriter {

	/// <summary>
	/// Writes a <see cref="Runner.SolutionExecutionResult"/> to the console.
	/// </summary>
	/// <param name="solutionResult">The
	/// <see cref="Runner.SolutionExecutionResult"/> to write.</param>
	public static void WriteSolution(Runner.SolutionExecutionResult solutionResult) {
		Text.FromString($"-- Day {solutionResult.Solver.GetDay()} --\n")
			.WithColor(ConsoleColor.White)
			.WithNewline()
			.Write();

		Text.FromString($"Solver: ")
			.WithColor(ConsoleColor.DarkGray)
			.Append($"{solutionResult.Solver.GetType().FullName}")
			.WithColor(ConsoleColor.White)
			.WithNewline()
			.Write();

		Text.FromString("Elapsed time: ")
			.WithColor(ConsoleColor.DarkGray)
			.Append(solutionResult.ElapsedTime.ToString())
			.WithColor(ConsoleColor.Yellow)
			.WithNewline()
			.Write();

		if (solutionResult.HasException)
			Text.FromString($"Exception ")
				.WithColor(ConsoleColor.DarkGray)
				.Append($"{solutionResult.Exception!.GetType().FullName}")
				.WithColor(ConsoleColor.White)
				.Append(": ")
				.WithColor(ConsoleColor.DarkGray)
				.Append(solutionResult.Exception!.Message)
				.WithColor(ConsoleColor.Red)
				.WithNewline()
				.Write();
		else {
			Text text = new();
			if (solutionResult.HasPart1)
				text = text.WithString("\nPart 1: ")
					.WithColor(ConsoleColor.DarkGray)
					.Append(solutionResult.Solution.Part1)
					.WithColor(ConsoleColor.White)
					.WithNewline();
			if (solutionResult.HasPart2)
				text = text.Append("Part 2: ")
					.WithColor(ConsoleColor.DarkGray)
					.Append(solutionResult.Solution.Part2)
					.WithColor(ConsoleColor.White)
					.WithNewline();
			text.Write();
		}
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
