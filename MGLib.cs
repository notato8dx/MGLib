using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MGLib;

public abstract class NotatoGame : Game {
	protected abstract class Drawable {
		public readonly Texture2D texture;

		protected Drawable(Texture2D texture) {
			this.texture = texture;
		}
	}

	private sealed class Character : Drawable {
		public readonly byte width;

		public Character(Texture2D texture, byte width) : base(texture) {
			this.width = width;
		}
	}

	private SpriteBatch spriteBatch;
	private Character[] characters;
	private readonly float scaleFactor;
	private readonly Vector2 offset;

	protected NotatoGame(int internalWidth, int internalHeight) {
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

	protected abstract void InitializeMethod();

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

		InitializeMethod();

		base.Initialize();
	}

	protected abstract void DrawMethod();

	protected sealed override void Draw(GameTime gameTime) {
		spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

		DrawMethod();

		spriteBatch.End();
	}

	protected void Draw(Texture2D texture, int x, int y) {
		spriteBatch.Draw(texture, new Vector2(x, y) * scaleFactor + offset, null, Color.White, 0, Vector2.Zero, scaleFactor, SpriteEffects.None, 0);
	}

	protected void DrawString(byte[] str, int x, int y) {
		for (byte i = 0; i < str.Length; i++ ) {
			Character character = characters[str[i]];
			Draw(character.texture, x, y);
			x += (ushort) (character.width);
		}
	}
}
