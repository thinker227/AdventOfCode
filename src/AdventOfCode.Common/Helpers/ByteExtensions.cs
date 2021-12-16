using System;

namespace AdventOfCode.Common;

public static class ByteExtensions {

	public static byte GetBit(this byte[] bytes, int index) {
		byte b = bytes[index / 8];
		int byteIndex = index % 8;
		int shift = 7 - byteIndex;
		int shifted = b >> shift;
		int single = shifted & 1;
		return (byte)single;
	}

	public static ulong GetInteger(this byte[] bytes, int index, int length) {
		if (length > sizeof(ulong) * 8)
			throw new ArgumentOutOfRangeException(nameof(index));
		
		ulong result = 0;
		for (int i = 0; i < length; i++) {
			result <<= 1;
			result |= GetBit(bytes, index + i);
		}
		return result;
	}

}
