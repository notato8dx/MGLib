using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MGLib;

public sealed class Clock<T> {
	private readonly ushort period;
	private readonly Action<T> action;

	private ushort time = 0;

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

public readonly struct Sprite {
	internal readonly Texture2D texture;

	public Sprite(string filename) {
		this.texture = Game.game.Content.Load<Texture2D>(filename);
	}
}

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
	protected Substate<T> substate;

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
	public virtual void OnConfirm(T superstate) {}

	public virtual void OnCancel(T superstate) {}

	public virtual void OnMoveUp(T superstate) {}

	public virtual void OnMoveDown(T superstate) {}

	public virtual void OnMoveLeft(T superstate) {}

	public virtual void OnMoveRight(T superstate) {}

	public virtual void Update(T superstate) {}

	public virtual void Draw(T superstate) {}
}

internal delegate State InitializeState();

public sealed class Game : Microsoft.Xna.Framework.Game {
	private const Keys confirmKey = Keys.Z;
	private const Keys cancelKey = Keys.X;
	private const Keys moveUpKey = Keys.Up;
	private const Keys moveDownKey = Keys.Down;
	private const Keys moveLeftKey = Keys.Left;
	private const Keys moveRightKey = Keys.Right;

	internal static Game game;

	public static void Start<T>(int width, int height) where T : State, new() {
		game = new Game(width, height, () => {
			return new T();
		});

		game.Run();
	}

	public static void ChangeState<T>() where T : State, new() {
		game.state = new T();
	}

	private readonly Dictionary<Keys, bool> keysLocked = new Dictionary<Keys, bool>() {
		{ confirmKey, false },
		{ cancelKey, false },
		{ moveUpKey, false },
		{ moveDownKey, false },
		{ moveLeftKey, false },
		{ moveRightKey, false }
	};

	private SpriteBatch spriteBatch;
	private readonly float scaleFactor;
	private readonly Vector2 offset;
	private Character[] characters;

	private InitializeState initializeState;
	internal State state;

	internal Game(int internalWidth, int internalHeight, InitializeState initializeState) {
		int width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
		int height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
		scaleFactor = Math.Min(width / internalWidth, height / internalHeight);
		offset = new Vector2((width - internalWidth * scaleFactor) / 2, (height - internalHeight * scaleFactor) / 2);

		new GraphicsDeviceManager(this) {
			PreferredBackBufferWidth = width,
			PreferredBackBufferHeight = height,
			IsFullScreen = true
		};

		this.initializeState = initializeState;
	}

	private void Draw(Texture2D texture, int x, int y) {
		spriteBatch.Draw(texture, new Vector2(x, y) * scaleFactor + offset, null, Color.White, 0, Vector2.Zero, scaleFactor, SpriteEffects.None, 0);
	}

	public static void Draw(Sprite sprite, int x, int y) {
		game.Draw(sprite.texture, x, y);
	}

	public static void Draw(Sprite sprite) {
		Draw(sprite, 0, 0);
	}

	public static void DrawString(byte[] str, int x, int y) {
		for (byte i = 0; i < str.Length; i++ ) {
			Character character = game.characters[str[i]];
			game.Draw(character.texture, x, y);
			x += (ushort) (character.width);
		}
	}

	protected override void Initialize() {
		spriteBatch = new SpriteBatch(GraphicsDevice);

		Content.RootDirectory = "Content";

		characters = new Character[66] {
			new Character(Content.Load<Texture2D>("0"), 6),
			new Character(Content.Load<Texture2D>("1"), 3),
			new Character(Content.Load<Texture2D>("2"), 6),
			new Character(Content.Load<Texture2D>("3"), 6),
			new Character(Content.Load<Texture2D>("4"), 6),
			new Character(Content.Load<Texture2D>("5"), 6),
			new Character(Content.Load<Texture2D>("6"), 6),
			new Character(Content.Load<Texture2D>("7"), 6),
			new Character(Content.Load<Texture2D>("8"), 6),
			new Character(Content.Load<Texture2D>("9"), 6),
			new Character(Content.Load<Texture2D>("a_lowercase"), 5),
			new Character(Content.Load<Texture2D>("b_lowercase"), 5),
			new Character(Content.Load<Texture2D>("c_lowercase"), 5),
			new Character(Content.Load<Texture2D>("d_lowercase"), 5),
			new Character(Content.Load<Texture2D>("e_lowercase"), 5),
			new Character(Content.Load<Texture2D>("f_lowercase"), 5),
			new Character(Content.Load<Texture2D>("g_lowercase"), 5),
			new Character(Content.Load<Texture2D>("h_lowercase"), 5),
			new Character(Content.Load<Texture2D>("i_lowercase"), 2),
			new Character(Content.Load<Texture2D>("j_lowercase"), 5),
			new Character(Content.Load<Texture2D>("k_lowercase"), 5),
			new Character(Content.Load<Texture2D>("l_lowercase"), 5),
			new Character(Content.Load<Texture2D>("m_lowercase"), 5),
			new Character(Content.Load<Texture2D>("n_lowercase"), 5),
			new Character(Content.Load<Texture2D>("o_lowercase"), 5),
			new Character(Content.Load<Texture2D>("p_lowercase"), 5),
			new Character(Content.Load<Texture2D>("q_lowercase"), 5),
			new Character(Content.Load<Texture2D>("r_lowercase"), 5),
			new Character(Content.Load<Texture2D>("s_lowercase"), 5),
			new Character(Content.Load<Texture2D>("t_lowercase"), 5),
			new Character(Content.Load<Texture2D>("u_lowercase"), 5),
			new Character(Content.Load<Texture2D>("v_lowercase"), 5),
			new Character(Content.Load<Texture2D>("w_lowercase"), 6),
			new Character(Content.Load<Texture2D>("x_lowercase"), 5),
			new Character(Content.Load<Texture2D>("y_lowercase"), 5),
			new Character(Content.Load<Texture2D>("z_lowercase"), 4),
			new Character(Content.Load<Texture2D>("a_uppercase"), 6),
			new Character(Content.Load<Texture2D>("b_uppercase"), 6),
			new Character(Content.Load<Texture2D>("c_uppercase"), 6),
			new Character(Content.Load<Texture2D>("d_uppercase"), 6),
			new Character(Content.Load<Texture2D>("e_uppercase"), 6),
			new Character(Content.Load<Texture2D>("f_uppercase"), 6),
			new Character(Content.Load<Texture2D>("g_uppercase"), 6),
			new Character(Content.Load<Texture2D>("h_uppercase"), 6),
			new Character(Content.Load<Texture2D>("i_uppercase"), 6),
			new Character(Content.Load<Texture2D>("j_uppercase"), 6),
			new Character(Content.Load<Texture2D>("k_uppercase"), 6),
			new Character(Content.Load<Texture2D>("l_uppercase"), 6),
			new Character(Content.Load<Texture2D>("m_uppercase"), 6),
			new Character(Content.Load<Texture2D>("n_uppercase"), 6),
			new Character(Content.Load<Texture2D>("o_uppercase"), 6),
			new Character(Content.Load<Texture2D>("p_uppercase"), 6),
			new Character(Content.Load<Texture2D>("q_uppercase"), 6),
			new Character(Content.Load<Texture2D>("r_uppercase"), 6),
			new Character(Content.Load<Texture2D>("s_uppercase"), 6),
			new Character(Content.Load<Texture2D>("t_uppercase"), 6),
			new Character(Content.Load<Texture2D>("u_uppercase"), 6),
			new Character(Content.Load<Texture2D>("v_uppercase"), 6),
			new Character(Content.Load<Texture2D>("w_uppercase"), 6),
			new Character(Content.Load<Texture2D>("x_uppercase"), 6),
			new Character(Content.Load<Texture2D>("y_uppercase"), 6),
			new Character(Content.Load<Texture2D>("z_uppercase"), 6),
			new Character(Content.Load<Texture2D>("period"), 2),
			new Character(Content.Load<Texture2D>("comma"), 3),
			new Character(Content.Load<Texture2D>("exclamation_mark"), 2),
			new Character(Content.Load<Texture2D>("question_mark"), 6)
		};

		state = initializeState();
		initializeState = null;

		base.Initialize();
	}

	protected override void Update(GameTime gameTime) {
		KeyboardState keyboard = Keyboard.GetState();

		UpdateKey(confirmKey, state.OnConfirm);
		UpdateKey(cancelKey, state.OnCancel);
		UpdateKey(moveUpKey, state.OnMoveUp);
		UpdateKey(moveDownKey, state.OnMoveDown);
		UpdateKey(moveLeftKey, state.OnMoveLeft);
		UpdateKey(moveRightKey, state.OnMoveRight);

		state.Update();

		void UpdateKey(Keys key, Action action) {
			if (keyboard[key] == KeyState.Up) {
				keysLocked[key] = false;
			} else if (!keysLocked[key]) {
				keysLocked[key] = true;
				action();
			}
		}
	}

	protected override void Draw(GameTime gameTime) {
		GraphicsDevice.Clear(Color.Black);
		spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
		state.Draw();
		spriteBatch.End();
	}

	private struct Character {
		public readonly Texture2D texture;
		internal readonly byte width;

		public Character(Texture2D texture, byte width) {
			this.texture = texture;
			this.width = width;
		}
	}
}
