namespace AdventOfCode.Solutions;

[Solver(20, @"input\20.txt")]
public sealed class Day20 : ISolver {
	
	public CombinedSolution Solve(string input) {
		var (algorithm, image) = ParseInput(input);
		Console.WriteLine(ImageToString(image));

		for (int i = 0; i < 2; i++) {
			image = Enhance(image, algorithm);
			Console.WriteLine(ImageToString(image));
		}
		
		return image.Points.Count;
	}

	private static (bool[], Image) ParseInput(string input) {
		var span = input.AsSpan();
		int newline = span.IndexOf('\r');
		var algorithm = span[..newline];
		var points = span[(newline+4)..];

		return(ParseAlgorithm(algorithm), ParseImage(points));
	}
	private static bool[] ParseAlgorithm(ReadOnlySpan<char> algorithm) {
		var bits = new bool[algorithm.Length];
		for (int i = 0; i < algorithm.Length; i++)
			bits[i] = algorithm[i] == '#';
		return bits;
	}
	private static Image ParseImage(ReadOnlySpan<char> points) {
		HashSet<Point> hashPoints = new();

		int width = 0;
		int y = 0;
		foreach (var line in points.EnumerateLines()) {
			width = line.Length;
			for (int x = 0; x < line.Length; x++)
				if (line[x] == '#') hashPoints.Add(new(x, y));
			y++;
		}

		int height = y;
		Rectangle bounds = new(0, 0, width, height);

		return new(hashPoints, bounds, false);
	}
	
	private static Image Enhance(Image image, bool[] algorithm) {
		HashSet<Point> enhanced = new();
		var bounds = image.Bounds;
		Rectangle newBounds = new(
			bounds.Min.X - 1,
			bounds.Min.Y - 1,
			bounds.Max.X + 1,
			bounds.Max.Y + 1);

		// Add borders if infinite pixel is active
		if (image.InfinitePixel) {
			for (int x = newBounds.Min.X; x <= newBounds.Max.X; x++) {
				enhanced.Add(new(x, newBounds.Min.Y));
				enhanced.Add(new(x, newBounds.Max.Y));
			}
			for (int y = newBounds.Min.Y; y <= newBounds.Max.Y; y++) {
				enhanced.Add(new(newBounds.Min.X, y));
				enhanced.Add(new(newBounds.Max.X, y));
			}
		}

		for (int x = newBounds.Min.X; x <= newBounds.Max.X; x++) {
			for (int y = newBounds.Min.Y; y <= newBounds.Max.Y; y++) {
				Point current = new(x, y);
				bool lit = GetEnhancedPoint(image, current, algorithm);
				if (lit) enhanced.Add(current);
			}
		}

		bool infinitePixel = image.InfinitePixel;
		if (algorithm[0] && !image.InfinitePixel) infinitePixel = true;
		if (!algorithm[^1] && image.InfinitePixel) infinitePixel = false;
		
		return new(enhanced, newBounds, infinitePixel);
	}
	private static bool GetEnhancedPoint(Image image, Point point, bool[] algorithm) {
		bool checkPixel(Point point) =>
			image.Bounds.PointInRectangle(point) ?
				image.Points.Contains(point) :
				image.InfinitePixel;

		var bits = new bool[9] {
			checkPixel(new(point.X - 1, point.Y - 1)),
			checkPixel(new(point.X,     point.Y - 1)),
			checkPixel(new(point.X + 1, point.Y - 1)),
			checkPixel(new(point.X - 1, point.Y    )),
			checkPixel(point),
			checkPixel(new(point.X + 1, point.Y    )),
			checkPixel(new(point.X - 1, point.Y + 1)),
			checkPixel(new(point.X,     point.Y + 1)),
			checkPixel(new(point.X + 1, point.Y + 1)),
		};

		int position = 0;
		for (int i = 0; i < 9; i++) {
			position <<= 1;
			if (bits[i]) position |= 1;
		}

		return algorithm[position];
	}

	private static string ImageToString(Image image) {
		System.Text.StringBuilder builder = new();
		for (int y = image.Bounds.Min.Y; y <= image.Bounds.Max.Y; y++) {
			for (int x = image.Bounds.Min.X; x <= image.Bounds.Max.X; x++) {
				if (image.Points.Contains(new(x, y))) builder.Append('#');
				else builder.Append('.');
			}
			builder.Append('\n');
		}
		return builder.ToString();
	}



	private readonly record struct Image(HashSet<Point> Points, Rectangle Bounds, bool InfinitePixel); 

}
