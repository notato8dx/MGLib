namespace MGLib;

public abstract class State {
	protected internal virtual void OnConfirm() {}

	protected internal virtual void OnCancel() {}

	protected internal virtual void OnMoveUp() {}

	protected internal virtual void OnMoveDown() {}

	protected internal virtual void OnMoveLeft() {}

	protected internal virtual void OnMoveRight() {}

	protected internal virtual void Update() {}

	protected internal virtual void Draw() {}
}

public abstract class Superstate<T> : State where T : Superstate<T> {
	private Substate<T> substate;

	protected Superstate(Substate<T> substate) {
		this.substate = substate;
	}

	protected void ChangeSubstate<T2>() where T2 : Substate<T>, new() {
		substate = new T2();
	}

	protected internal override void OnConfirm() {
		substate.OnConfirm((T) this);
	}

	protected internal override void OnCancel() {
		substate.OnCancel((T) this);
	}

	protected internal override void OnMoveUp() {
		substate.OnMoveUp((T) this);
	}

	protected internal override void OnMoveDown() {
		substate.OnMoveDown((T) this);
	}

	protected internal override void OnMoveLeft() {
		substate.OnMoveLeft((T) this);
	}

	protected internal override void OnMoveRight() {
		substate.OnMoveRight((T) this);
	}

	protected internal override void Update() {
		substate.Update((T) this);
	}

	protected internal override void Draw() {
		substate.Draw((T) this);
	}
}

public abstract class Substate<T> where T : Superstate<T> {
	protected internal virtual void OnConfirm(T superstate) {}

	protected internal virtual void OnCancel(T superstate) {}

	protected internal virtual void OnMoveUp(T superstate) {}

	protected internal virtual void OnMoveDown(T superstate) {}

	protected internal virtual void OnMoveLeft(T superstate) {}

	protected internal virtual void OnMoveRight(T superstate) {}

	protected internal virtual void Update(T superstate) {}

	protected internal virtual void Draw(T superstate) {}
}