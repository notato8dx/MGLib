using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MGLib;

public readonly struct Sprite {
	internal readonly Texture2D texture;

	public Sprite(string filename) {
		if (Game.game == null) {
			throw new InvalidOperationException();
		}
		
		texture = Game.game.Content.Load<Texture2D>(filename);
	}
}

public sealed class Animation {
	internal readonly Texture2D texture;
	internal readonly int width;
	internal readonly int height;
	internal readonly int loopFrame;
	internal readonly int frames;

	private readonly Clock<Animation> frameClock;
	private int frame;

	public Animation(string filename, int width, int height, ushort framesPerFrame, int loopFrame, int frames) {
		if (Game.game == null) {
			throw new InvalidOperationException();
		}

		texture = Game.game.Content.Load<Texture2D>(filename);
		this.width = width;
		this.height = height;
		this.frameClock = new(framesPerFrame, (Animation animation) => {
			animation.frame += 1;
			if (animation.frame == animation.frames) {
				animation.frame = animation.loopFrame;
			}
		});
		this.loopFrame = loopFrame;
		this.frames = frames;
	}

	public void Draw() {
		if (Game.game == null) {
			throw new InvalidOperationException();
		}

		int framesPerRow = 2048 / width;
		int column = frame % framesPerRow;
		int row = frame / framesPerRow;

		Game.game.Draw(texture, 0, 0, new Rectangle(width * column, height * row, width, height));
		frameClock.Tick(this);
	}
}