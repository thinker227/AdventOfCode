namespace AdventOfCode.Solutions;

[Solver(16, @"input\16.txt")]
public sealed class Day16 : ISolver {
	
	public CombinedSolution Solve(string input) {
		var bytes = HexToBinary(input);
		PacketReader reader = new(bytes);
		var packet = reader.ParsePacket();

		int sum = SumVersions(packet);
		return sum;
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
	
	private static int SumVersions(Packet root) {
		int result = root.Version;
		if (root.Subpackets is null) return result;
		foreach (var packet in root.Subpackets)
			result += SumVersions(packet);
		return result;
	}



	private struct PacketReader {

		private readonly byte[] bytes;
		private int position;

		public PacketReader(byte[] bytes) {
			this.bytes = bytes;
			position = 0;
		}

		public Packet ParsePacket() {
			byte version = (byte)ReadInteger(3);
			byte typeId = (byte)ReadInteger(3);

			if (typeId == 4) {
				ulong literal = ReadLiteral();
				return new(version, typeId, literal, null);
			}
			else {
				byte lengthTypeId = ReadBit();
				int readBits = lengthTypeId == 0 ? 15 : 11;
				int subpacketSizeOrCount = (int)ReadInteger(readBits);
				var subpackets = lengthTypeId == 0 ?
					ReadPacketsFromSize(subpacketSizeOrCount) :
					ReadPacketsFromCount(subpacketSizeOrCount);
				return new(version, typeId, null, subpackets);
			}
		}
		private ulong ReadLiteral() {
			byte leading = 1;
			ulong result = 0;

			while (leading != 0) {
				leading = ReadBit();
				result <<= 4;
				result |= ReadInteger(4);
			}

			return result;
		}
		private IReadOnlyCollection<Packet> ReadPacketsFromSize(int size) {
			List<Packet> packets = new();
			int end = position + size;

			while (position < end) {
				var packet = ParsePacket();
				packets.Add(packet);
			}

			return packets;
		}
		private IReadOnlyCollection<Packet> ReadPacketsFromCount(int count) {
			var packets = new Packet[count];

			for (int i = 0; i < count; i++) {
				var packet = ParsePacket();
				packets[i] = packet;
			}

			return packets;
		}

		private static byte GetBit(byte[] bytes, int position) {
			byte b = bytes[position / 8];
			int byteIndex = position % 8;
			int shift = 7 - byteIndex;
			int shifted = b >> shift;
			int single = shifted & 1;
			return (byte)single;
		}
		private byte ReadBit() =>
			GetBit(bytes, position++);
		private ulong ReadInteger(int length) {
			ulong result = 0;
			for (int i = 0; i < length; i++) {
				result <<= 1;
				result |= GetBit(bytes, position + i);
			}
			position += length;
			return result;
		}

	}

	private readonly record struct Packet(int Version, int TypeId, ulong? Literal, IReadOnlyCollection<Packet>? Subpackets);

}