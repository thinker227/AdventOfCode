using System;

namespace AdventOfCode.Common;

/// <summary>
/// An exception thrown when I'm too lazy to finish a day.
/// </summary>
public class TooLazyException : Exception {

	/// <summary>
	/// Initializes a new <see cref="TooLazyException"/> instance.
	/// </summary>
	public TooLazyException() : base("I'm too lazy to finish this day.") { }

}
