namespace AdventOfCode.Solutions;

[Solver(16, @"input\16.txt")]
public sealed class Day16 : ISolver {
	
	public CombinedSolution Solve(string input) {
		var bytes = HexToBinary(input);
		Packet packet = new(bytes, 0, bytes.Length * 8);
		var subpacket = packet.GetSubpacket();

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

		public byte Version { get; }
		public byte TypeId { get; }
		public PacketType Type =>
			TypeId == 4 ?
			PacketType.Literal : PacketType.Operator;

		public Packet(byte[] bytes, int offset, int length) {
			this.bytes = bytes;
			this.offset = offset;
			this.length = length;

			Version = (byte)bytes.GetInteger(offset, 3);
			TypeId = (byte)bytes.GetInteger(offset + 3, 3);
		}

		public ulong GetLiteral() {
			if (Type != PacketType.Literal)
				throw new InvalidOperationException();

			int from = offset + 6;
			byte leading = 1;
			ulong result = 0;

			for (int i = from; leading != 0; i += 5) {
				leading = bytes.GetBit(i);
				byte current = (byte)bytes.GetInteger(i + 1, 4);
				result <<= 4;
				result |= current;
			}

			return result;
		}

		private byte GetLengthTypeId() =>
			bytes.GetBit(offset + 6);
		private int GetSubpacketSizeLength() =>
			GetLengthTypeId() == 0 ? 15 : 11;
		private int GetSubpacketSize(int length) =>
			(int)bytes.GetInteger(offset + 6 + 1, length);
		public Packet GetSubpacket() {
			if (Type != PacketType.Operator)
				throw new InvalidOperationException();
			
			int sizeLength = GetSubpacketSizeLength();
			int length = GetSubpacketSize(sizeLength);
			int subpacketOffset = offset + 6 + 1 + sizeLength;
			return new(bytes, subpacketOffset, length);
		}

	}

	private enum PacketType {
		Literal,
		Operator
	}
	
}
