using System;

namespace MGLib;

public static class Utility {
	public static void ForEach(byte count, Action<byte> action) {
		if (action == null) {
			throw new InvalidOperationException();
		}

		for (byte i = 0; i < count; i += 1) {
			action(i);
		}
	}

	public static void IncrementToMax(ref byte value, int maximum) {
		if (value < maximum - 1) {
			value += 1;
		}
	}

	public static void IncrementToMax(ref int value, int maximum) {
		if (value < maximum - 1) {
			value += 1;
		}
	}

	public static void DecrementToMin(ref byte value, int minimum) {
		if (value > minimum) {
			value -= 1;
		}
	}

	public static void DecrementToMin(ref int value, int minimum) {
		if (value > minimum) {
			value -= 1;
		}
	}
}