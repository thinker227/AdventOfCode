using System;

namespace AdventOfCode.Common;

public static class ByteExtensions {

	public static byte GetBit(this byte[] bytes, int index) =>
		(byte)(bytes[index / 8] >> index % 8 & 1);

	public static byte GetByte(this byte[] bytes, int index, int length) {
		if (length >= 8) throw new ArgumentOutOfRangeException(nameof(index));
		
		byte result = 0;
		for (int i = index; i < index + length; i++) {
			result |= GetBit(bytes, index + i);
			result <<= 1;
		}
		return result;
	}

}
