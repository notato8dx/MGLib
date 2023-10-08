using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MGLib;

public abstract class State<T> where T : Scene {
	public virtual void OnConfirm(T scene) {}

	public virtual void OnCancel(T scene) {}

	public virtual void OnMoveUp(T scene) {}

	public virtual void OnMoveDown(T scene) {}

	public virtual void OnMoveLeft(T scene) {}

	public virtual void OnMoveRight(T scene) {}

	public virtual void Update(T scene) {}

	public virtual void Draw(NotatoGame game, T scene) {}
}

public abstract class Scene {
	protected internal virtual void Initialize(NotatoGame game) {}

	protected internal virtual void OnConfirm(NotatoGame game) {}

	protected internal virtual void OnCancel(NotatoGame game) {}

	protected internal virtual void OnMoveUp(NotatoGame game) {}

	protected internal virtual void OnMoveDown(NotatoGame game) {}

	protected internal virtual void OnMoveLeft(NotatoGame game) {}

	protected internal virtual void OnMoveRight(NotatoGame game) {}

	protected internal virtual void Update(NotatoGame game) {}

	protected internal virtual void Draw(NotatoGame game) {}
}

public sealed class NotatoGame : Game {
	private const Keys confirmKey = Keys.Z;
	private const Keys cancelKey = Keys.X;
	private const Keys moveUpKey = Keys.Up;
	private const Keys moveDownKey = Keys.Down;
	private const Keys moveLeftKey = Keys.Left;
	private const Keys moveRightKey = Keys.Right;

	private Scene scene;

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

	public NotatoGame(int internalWidth, int internalHeight, Scene initialScene) {
		scene = initialScene;

		int width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
		int height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
		scaleFactor = Math.Min(width / internalWidth, height / internalHeight);
		offset = new Vector2((width - internalWidth * scaleFactor) / 2, (height - internalHeight * scaleFactor) / 2);

		new GraphicsDeviceManager(this) {
			PreferredBackBufferWidth = width,
			PreferredBackBufferHeight = height,
			IsFullScreen = true
		};

		Run();
	}

	public void ChangeScene(Scene scene) {
		this.scene = scene;
		this.scene.Initialize(this);
	}

	public void Draw(Texture2D texture, int x, int y) {
		spriteBatch.Draw(texture, new Vector2(x, y) * scaleFactor + offset, null, Color.White, 0, Vector2.Zero, scaleFactor, SpriteEffects.None, 0);
	}

	public void DrawString(byte[] str, int x, int y) {
		for (byte i = 0; i < str.Length; i++ ) {
			Character character = characters[str[i]];
			Draw(character.texture, x, y);
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

		scene.Initialize(this);

		base.Initialize();
	}

	protected override void Update(GameTime gameTime) {
		KeyboardState keyboard = Keyboard.GetState();

		UpdateKey(confirmKey, scene.OnConfirm);
		UpdateKey(cancelKey, scene.OnCancel);
		UpdateKey(moveUpKey, scene.OnMoveUp);
		UpdateKey(moveDownKey, scene.OnMoveDown);
		UpdateKey(moveLeftKey, scene.OnMoveLeft);
		UpdateKey(moveRightKey, scene.OnMoveRight);

		scene.Update(this);

		void UpdateKey(Keys key, Action<NotatoGame> action) {
			if (keyboard[key] == KeyState.Up) {
				keysLocked[key] = false;
			} else if (!keysLocked[key]) {
				keysLocked[key] = true;
				action(this);
			}
		}
	}

	protected override void Draw(GameTime gameTime) {
		spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
		scene.Draw(this);
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
