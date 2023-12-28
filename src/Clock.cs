using System;

namespace MGLib;

public sealed class Clock<T> {
	private readonly ushort period;
	private readonly Action<T> action;

	private ushort time;

	public Clock(ushort period, Action<T> action) {
		this.period = period;
		this.action = action;
	}

	public void Tick(T owner) {
		if (time < period - 1) {
			time += 1;
		} else {
			time = 0;
			action(owner);
		}
	}
}