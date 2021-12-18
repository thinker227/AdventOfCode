using System;
using System.Diagnostics;

namespace AdventOfCode.Common;

/// <summary>
/// Represents a discriminated unions between two types.
/// </summary>
/// <typeparam name="T1">The first type in the union.</typeparam>
/// <typeparam name="T2">The second type in the union.</typeparam>
[DebuggerTypeProxy(typeof(UnionDebuggerProxy<,>))]
public readonly struct Union<T1, T2> {

	private readonly T1? value1 = default;
	private readonly T2? value2 = default;



	/// <summary>
	/// Whether the union has a value of type <typeparamref name="T1"/>.
	/// </summary>
	public bool HasValue1 { get; } = false;
	/// <summary>
	/// The value of type <typeparamref name="T1"/>.
	/// </summary>
	/// <exception cref="InvalidOperationException">
	/// The value is not set.
	/// </exception>
	public T1 Value1 =>
		HasValue1 ? value1! :
		throw new InvalidOperationException();
	/// <summary>
	/// Whether the union has a value of type <typeparamref name="T2"/>.
	/// </summary>
	public bool HasValue2 { get; } = false;
	/// <summary>
	/// The value of type <typeparamref name="T2"/>.
	/// </summary>
	/// <exception cref="InvalidOperationException">
	/// The value is not set.
	/// </exception>
	public T2 Value2 =>
		HasValue2 ? value2! :
		throw new InvalidOperationException();



	/// <summary>
	/// Initializes a new <see cref="Union{T1, T2}"/> instance.
	/// </summary>
	/// <param name="value">The value of type <typeparamref name="T1"/>.</param>
	public Union(T1 value) {
		value1 = value;
		HasValue1 = true;
	}
	/// <summary>
	/// Initializes a new <see cref="Union{T1, T2}"/> instance.
	/// </summary>
	/// <param name="value">The value of type <typeparamref name="T2"/>.</param>
	public Union(T2 value) {
		value2 = value;
		HasValue2 = true;
	}



	/// <summary>
	/// Performs a switch over the union.
	/// </summary>
	/// <param name="action1">The <see cref="Action{T1}"/> to execute
	/// if the union has a value of type <typeparamref name="T1"/>.</param>
	/// <param name="action2">The <see cref="Action{T2}"/> to execute
	/// if the union has a value of type <typeparamref name="T2"/>.</param>
	/// <exception cref="InvalidOperationException">
	/// No value is set.
	/// </exception>
	public void Switch(Action<T1> action1, Action<T2> action2) {
		if (HasValue1) action1(Value1);
		if (HasValue2) action2(Value2);
		throw new InvalidOperationException();
	}

	/// <summary>
	/// Performs a switch over the union returning a value.
	/// </summary>
	/// <param name="func1">The <see cref="Func{T1, TResult}"/> to execute
	/// if the union has a value of type <typeparamref name="T1"/>.</param>
	/// <param name="func2">The <see cref="Func{T1, TResult}"/> to execute
	/// if the union has a value of type <typeparamref name="T2"/>.</param>
	/// <returns>The result of the switch.</returns>
	/// <exception cref="InvalidOperationException">
	/// No value is set.
	/// </exception>
	public TResult Switch<TResult>(Func<T1, TResult> func1, Func<T2, TResult> func2) {
		if (HasValue1) return func1(Value1);
		if (HasValue2) return func2(Value2);
		throw new InvalidOperationException();
	}
	/// <summary>
	/// Performs a switch over the union returning a value.
	/// </summary>
	/// <param name="value1">The value to return
	/// if the union has a value of type <typeparamref name="T1"/>.</param>
	/// <param name="func2">The <see cref="Func{T1, TResult}"/> to execute
	/// if the union has a value of type <typeparamref name="T2"/>.</param>
	/// <returns>The result of the switch.</returns>
	/// <exception cref="InvalidOperationException">
	/// No value is set.
	/// </exception>
	public TResult Switch<TResult>(TResult value1, Func<T2, TResult> func2) =>
		Switch(Functional.Result<T1, TResult>(value1), func2);
	/// <summary>
	/// Performs a switch over the union returning a value.
	/// </summary>
	/// <param name="func1">The <see cref="Func{T1, TResult}"/> to execute
	/// if the union has a value of type <typeparamref name="T1"/>.</param>
	/// <param name="value2">The value to return
	/// if the union has a value of type <typeparamref name="T2"/>.</param>
	/// <returns>The result of the switch.</returns>
	/// <exception cref="InvalidOperationException">
	/// No value is set.
	/// </exception>
	public TResult Switch<TResult>(Func<T1, TResult> func1, TResult value2) =>
		Switch(func1, Functional.Result<T2, TResult>(value2));
	/// <summary>
	/// Performs a switch over the union returning a value.
	/// </summary>
	/// <param name="value1">The value to return
	/// if the union has a value of type <typeparamref name="T1"/>.</param>
	/// <param name="value2">The value to return
	/// if the union has a value of type <typeparamref name="T2"/>.</param>
	/// <returns>The result of the switch.</returns>
	/// <exception cref="InvalidOperationException">
	/// No value is set.
	/// </exception>
	public TResult Switch<TResult>(TResult value1, TResult value2) =>
		Switch(Functional.Result<T1, TResult>(value1), Functional.Result<T2, TResult>(value2));

	public override string? ToString() {
		if (HasValue1) return Value1?.ToString();
		if (HasValue2) return Value2?.ToString();
		return "none";
	}



	/// <summary>
	/// Implicitly converts a value of type <typeparamref name="T1"/>
	/// to a <see cref="Union{T1, T2}"/> instance.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Union<T1, T2>(T1 value) =>
		new(value);
	/// <summary>
	/// Implicitly converts a value of type <typeparamref name="T2"/>
	/// to a <see cref="Union{T1, T2}"/> instance.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Union<T1, T2>(T2 value) =>
		new(value);

	/// <summary>
	/// Converts a <see cref="Union{T1, T2}"/> to a value of type <typeparamref name="T1"/>.
	/// </summary>
	/// <param name="union">The union to get the value of.</param>
	/// <exception cref="InvalidOperationException">
	/// <see cref="Value1"/> is not set.
	/// </exception>
	public static explicit operator T1(Union<T1, T2> union) =>
		union.Value1;
	/// <summary>
	/// Converts a <see cref="Union{T1, T2}"/> to a value of type <typeparamref name="T2"/>.
	/// </summary>
	/// <param name="union">The union to get the value of.</param>
	/// <exception cref="InvalidOperationException">
	/// <see cref="Value2"/> is not set.
	/// </exception>
	public static explicit operator T2(Union<T1, T2> union) =>
		union.Value2;

}

internal class UnionDebuggerProxy<T1, T2> {
	private readonly Union<T1, T2> union;
	private object[]? cached;

	public UnionDebuggerProxy(Union<T1, T2> union) {
		this.union = union;
	}

	[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
	public object[] Contents {
		get {
			if (cached is null) {
				if (union.HasValue1) cached = new object[] { union.Value1! };
				else if (union.HasValue2) cached = new object[] { union.Value2! };
				else cached = Array.Empty<object>();
			}
			return cached;
		}
	}
}
