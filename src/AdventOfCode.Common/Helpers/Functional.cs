using System;

namespace AdventOfCode.Common;

/// <summary>
/// Contains general functions.
/// </summary>
public static class Functional {

	/// <summary>
	/// An identity function returning the input value.
	/// </summary>
	/// <typeparam name="T">The type of the in/out value.</typeparam>
	/// <param name="value">The value to return.</param>
	/// <returns><paramref name="value"/></returns>
	public static T Identity<T>(T value) => value;

	/// <summary>
	/// Returns a <see cref="Func{T, TResult}"/> returning a specified result.
	/// </summary>
	/// <typeparam name="T">The type of the argument of the function.</typeparam>
	/// <typeparam name="TResult">The type of the result of the function.</typeparam>
	/// <param name="result">The value to return.</param>
	/// <returns>A <see cref="Func{T, TResult}"/>
	/// returning <paramref name="result"/>.</returns>
	public static Func<T, TResult> Result<T, TResult>(TResult result) => _ => result;

	/// <summary>
	/// An <see cref="Action{T}"/> which does nothing.
	/// </summary>
	/// <typeparam name="T">The type of the argument of the action.</typeparam>
	/// <returns>An <see cref="Action{T}"/> which does nothing.</returns>
	public static Action<T> Nothing<T>() => _ => { };

}
