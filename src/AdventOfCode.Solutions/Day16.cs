namespace AdventOfCode.Solutions;

[Solver(16, @"input\16.txt")]
public sealed class Day16 : ISolver {
	
	public CombinedSolution Solve(string input) {
		var bytes = HexToBinary(input);
		Packet packet = new(bytes, 0, bytes.Length * 8);
		_ = packet.Version;
		_ = packet.TypeId;

		throw new NotImplementedException();
	}

	private static byte[] HexToBinary(string hex) {
		int length = hex.Length / 2;
		byte[] bytes = new byte[length];
		var hexSpan = hex.AsSpan();

		for (int i = 0; i < length; i++) {
			var current = hexSpan.Slice(i * 2, 2);
			byte result = HexDigitToByte(current[0]);
			result <<= 4;
			result |= HexDigitToByte(current[1]);
			bytes[i] = result;
		}

		return bytes;
	}
	private static byte HexDigitToByte(char c) =>
		(byte)(c >= 'A' ? c - 'A' + 10 : c - '0');



	private readonly struct Packet {

		private readonly byte[] bytes;
		private readonly int offset;
		private readonly int length;

		public byte Version =>
			bytes.GetByte(offset, 3);
		public byte TypeId =>
			bytes.GetByte(offset + 3, 3);

		public Packet(byte[] bytes, int offset, int length) {
			this.bytes = bytes;
			this.offset = offset;
			this.length = length;
		}

	}
	
}
