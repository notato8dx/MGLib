using System;

namespace MGLib;

public readonly struct Song {
	internal readonly Microsoft.Xna.Framework.Media.Song song;

	public Song(string filename) {
		if (Game.game == null) {
			throw new InvalidOperationException();
		}
		
		song = Game.game.Content.Load<Microsoft.Xna.Framework.Media.Song>(filename);
	}
}