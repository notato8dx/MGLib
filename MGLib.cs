using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MGLib;

public abstract class NotatoGame : Game {
	private sealed class Character {
		public readonly Texture2D texture;
		internal readonly byte width;

		public Character(Texture2D texture, byte width) {
			this.texture = texture;
			this.width = width;
		}
	}

	public abstract class Scene {
		protected internal static NotatoGame game;
		protected internal Dictionary<Keys, Action> actions;

		protected internal abstract void Update();

		protected internal abstract void Draw();
	}

	private SpriteBatch spriteBatch;
	private Character[] characters;
	private readonly float scaleFactor;
	private readonly Vector2 offset;

	private Dictionary<Keys, bool> keysLocked = new Dictionary<Keys, bool>() {
		{ Keys.Up, false },
		{ Keys.Down, false },
		{ Keys.Left, false },
		{ Keys.Right, false },
		{ Keys.Z, false },
		{ Keys.X, false }
	};

	public Scene scene;

	protected NotatoGame(int internalWidth, int internalHeight, Scene initialScene) {
		int width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
		int height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
		scaleFactor = Math.Min(width / internalWidth, height / internalHeight);
		offset = new Vector2((width - internalWidth * scaleFactor) / 2, (height - internalHeight * scaleFactor) / 2);

		new GraphicsDeviceManager(this) {
			PreferredBackBufferWidth = width,
			PreferredBackBufferHeight = height,
			IsFullScreen = true
		};

		Scene.game = this;
		scene = initialScene;

		Run();
	}

	protected abstract void Load();

	protected sealed override void Initialize() {
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

		Load();

		base.Initialize();
	}

	protected sealed override void Update(GameTime gameTime) {
		KeyboardState keyboard = Keyboard.GetState();

		foreach (Keys key in keysLocked.Keys) {
			if (keyboard[key] == KeyState.Up) {
				keysLocked[key] = false;
			} else if (!keysLocked[key]) {
				keysLocked[key] = true;
				scene.actions[key]();
			}
		}

		scene.Update();
	}

	protected sealed override void Draw(GameTime gameTime) {
		spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
		scene.Draw();
		spriteBatch.End();
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
}
