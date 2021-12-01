using System;
using System.Collections.Generic;

namespace AdventOfCode.Common;

public static class EnumerableExtensions {

	/// <summary>
	/// Creates a moving window of elements in a sequence.
	/// </summary>
	/// <typeparam name="TElement">The type of the elements in <paramref name="source"/>.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="count">The amount of items to view in each window.</param>
	/// <returns>An <see cref="IEnumerable{T}"/> of sequences containing the elements of
	/// <paramref name="source"/> viewed in succession in a size of <paramref name="count"/>.</returns>
	public static IEnumerable<IEnumerable<TElement>> View<TElement>(this IEnumerable<TElement> source, int count) {
		if (source is null) throw new ArgumentNullException(nameof(source));
		if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be zero or negative.");
		
		var view = new Queue<TElement>();
		int index = 0;
		foreach (var element in source) {
			view.Enqueue(element);
			if (index++ < count - 1) continue;
			yield return view;
			view.Dequeue();
		}
	}

}
