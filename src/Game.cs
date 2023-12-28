using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace MGLib;

public sealed class Game : Microsoft.Xna.Framework.Game {
	private const Keys ConfirmKey = Keys.Z;
	private const Keys CancelKey = Keys.X;
	private const Keys MoveUpKey = Keys.Up;
	private const Keys MoveDownKey = Keys.Down;
	private const Keys MoveLeftKey = Keys.Left;
	private const Keys MoveRightKey = Keys.Right;

	private static readonly (string, byte)[] CharacterNames = [
		("0", 6),
		("1", 3),
		("2", 6),
		("3", 6),
		("4", 6),
		("5", 6),
		("6", 6),
		("7", 6),
		("8", 6),
		("9", 6),
		("a_lowercase", 5),
		("b_lowercase", 5),
		("c_lowercase", 5),
		("d_lowercase", 5),
		("e_lowercase", 5),
		("f_lowercase", 5),
		("g_lowercase", 5),
		("h_lowercase", 5),
		("i_lowercase", 2),
		("j_lowercase", 5),
		("k_lowercase", 5),
		("l_lowercase", 5),
		("m_lowercase", 5),
		("n_lowercase", 5),
		("o_lowercase", 5),
		("p_lowercase", 5),
		("q_lowercase", 5),
		("r_lowercase", 5),
		("s_lowercase", 5),
		("t_lowercase", 5),
		("u_lowercase", 5),
		("v_lowercase", 5),
		("w_lowercase", 6),
		("x_lowercase", 5),
		("y_lowercase", 5),
		("z_lowercase", 4),
		("a_uppercase", 6),
		("b_uppercase", 6),
		("c_uppercase", 6),
		("d_uppercase", 6),
		("e_uppercase", 6),
		("f_uppercase", 6),
		("g_uppercase", 6),
		("h_uppercase", 6),
		("i_uppercase", 6),
		("j_uppercase", 6),
		("k_uppercase", 6),
		("l_uppercase", 6),
		("m_uppercase", 6),
		("n_uppercase", 6),
		("o_uppercase", 6),
		("p_uppercase", 6),
		("q_uppercase", 6),
		("r_uppercase", 6),
		("s_uppercase", 6),
		("t_uppercase", 6),
		("u_uppercase", 6),
		("v_uppercase", 6),
		("w_uppercase", 6),
		("x_uppercase", 6),
		("y_uppercase", 6),
		("z_uppercase", 6),
		("period", 2),
		("comma", 3),
		("exclamation_mark", 2),
		("question_mark", 6)
	];

	internal static Game? game;

	public static void Start<T>(int width, int height) where T : State, new() {
		game = new Game(width, height, () => {
			return new T();
		});

		MediaPlayer.IsRepeating = true;

		game.Run();
	}

	public static void ChangeState<T>() where T : State, new() {
		if (Game.game == null) {
			throw new InvalidOperationException();
		}

		game.state = new T();
	}

	public static void ChangeSong(Song song) {
		MediaPlayer.Play(song.song);
	}		

	internal State state = null!;

	private readonly Dictionary<Keys, bool> keysLocked = new Dictionary<Keys, bool>() {
		{ ConfirmKey, false },
		{ CancelKey, false },
		{ MoveUpKey, false },
		{ MoveDownKey, false },
		{ MoveLeftKey, false },
		{ MoveRightKey, false }
	};

	private readonly float scaleFactor;
	private readonly Vector2 offset;

#pragma warning disable CA2213 // Disposable fields should be disposed
	private SpriteBatch spriteBatch = null!;
#pragma warning restore CA2213 // Disposable fields should be disposed
	private readonly Character[] characters;
	private readonly InitializeState initializeState;
	
	internal Game(int internalWidth, int internalHeight, InitializeState initializeState) {
		int width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
		int height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
		scaleFactor = Math.Min(width / internalWidth, height / internalHeight);
		offset = new Vector2((width - internalWidth * scaleFactor) / 2, (height - internalHeight * scaleFactor) / 2);

		new GraphicsDeviceManager(this) {
			PreferredBackBufferWidth = width,
			PreferredBackBufferHeight = height,
			IsFullScreen = true
		}.Dispose();

		characters = new Character[CharacterNames.Length];

		this.initializeState = initializeState;
	}

	private void Draw(Texture2D texture, int x, int y) {
		spriteBatch.Draw(texture, new Vector2(x, y) * scaleFactor + offset, null, Color.White, 0, Vector2.Zero, scaleFactor, SpriteEffects.None, 0);
	}

	internal void Draw(Texture2D texture, int x, int y, Rectangle source) {
		spriteBatch.Draw(texture, new Vector2(x, y) * scaleFactor + offset, source, Color.White, 0, Vector2.Zero, scaleFactor, SpriteEffects.None, 0);
	}

	public static void Draw(Sprite sprite, int x, int y) {
		if (Game.game == null) {
			throw new InvalidOperationException();
		}
		
		game.Draw(sprite.texture, x, y);
	}

	public static void Draw(Sprite sprite) {
		Draw(sprite, 0, 0);
	}

	public static void DrawString(byte[] str, int x, int y) {
		if (Game.game == null) {
			throw new InvalidOperationException();
		}
		
		if (str == null) {
			return;
		}
		
		for (byte i = 0; i < str.Length; i++) {
			Character character = game.characters[str[i]];
			game.Draw(character.texture, x, y);
			x += (ushort) (character.width);
		}
	}

	protected override void Initialize() {
		spriteBatch = new SpriteBatch(GraphicsDevice);

		Content.RootDirectory = "Content";

		for (int i = 0; i < CharacterNames.Length; i += 1) {
			characters[i] = new(Content.Load<Texture2D>(CharacterNames[i].Item1), CharacterNames[i].Item2);
		}

		state = initializeState();

		base.Initialize();
	}

	protected override void Update(GameTime gameTime) {
		KeyboardState keyboard = Keyboard.GetState();

		UpdateKey(ConfirmKey, state.OnConfirm);
		UpdateKey(CancelKey, state.OnCancel);
		UpdateKey(MoveUpKey, state.OnMoveUp);
		UpdateKey(MoveDownKey, state.OnMoveDown);
		UpdateKey(MoveLeftKey, state.OnMoveLeft);
		UpdateKey(MoveRightKey, state.OnMoveRight);

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

	internal delegate State InitializeState();
}
