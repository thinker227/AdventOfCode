﻿using System;
using System.Diagnostics;
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
		// -- Day {day} --
		Text.FromString($"-- Day {solutionResult.Solver.GetDay()} --\n")
			.WithColor(ConsoleColor.White)
			.WithNewline()
			.Write();

		// Solver: {solver type}
		Text.FromString($"Solver: ")
			.WithColor(ConsoleColor.DarkGray)
			.Append($"{solutionResult.Solver.GetType().FullName}")
			.WithColor(ConsoleColor.White)
			.WithNewline()
			.Write();

		// Elapsed time: {time} (debug{?})
		Text elapsed = Text.FromString("Elapsed time: ")
			.WithColor(ConsoleColor.DarkGray)
			.Append($"{solutionResult.ElapsedTime}")
			.WithColor(ConsoleColor.Yellow);
		if (solutionResult.Debug)
			elapsed = elapsed
				.Append(" (debug)")
				.WithColor(ConsoleColor.Green);
		elapsed.Append("\n\n")
			.Write();

		if (solutionResult.HasException) {
			// Exception {exception type}: {exception message}
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

			// Stack trace: {stack trace}
			Text.FromString("Stack trace: ")
				.WithColor(ConsoleColor.DarkGray)
				.WithNewline()
				.Write();
			GetFormattedExceptionStackTrace(solutionResult.Exception!).Write();
		}
		else {
			Text text = new();
			if (solutionResult.HasPart1)
				// Part 1: {solution}
				text = text.WithString("Part 1: ")
					.WithColor(ConsoleColor.DarkGray)
					.Append(solutionResult.Part1)
					.WithColor(ConsoleColor.White)
					.WithNewline();
			if (solutionResult.HasPart2)
				// Part 2: {solution}
				text = text.Append("Part 2: ")
					.WithColor(ConsoleColor.DarkGray)
					.Append(solutionResult.Part2)
					.WithColor(ConsoleColor.White)
					.WithNewline();
			text.Write();
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
