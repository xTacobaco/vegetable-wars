using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vegetable_Wars.Funcs {
  class SpriteManager {
    public Texture2D SpriteSheet { get; set; }
    public SpriteEffects SpriteEffect { get; set; } = SpriteEffects.FlipHorizontally;
    public Point TileSize { get; set; }
    public float ScaleSize { get; set; }
    public int HOffset { get; set; }

    public float Rotation { get; set; }
    public Point Orgin { get; set; }
    public Vector2 Pos { get; set; }

    public double Index { get; set; }
    public int[] Sequence { get; set; }
    public double Speed { get; set; }

    public SpriteManager() { }

    public void Update(GameTime gameTime) {
      Index += Speed / 60;
      if (Index >= Sequence.Length)
        Index = 0;
    }

    public void Draw(SpriteBatch spriteBatch) { Draw(spriteBatch, Color.White); }
    public void Draw(SpriteBatch spriteBatch, Color color) {
      var srcRect = new Rectangle(Sequence[(int)Index] * TileSize.X, HOffset * TileSize.Y, TileSize.X, TileSize.Y);
      spriteBatch.Draw(SpriteSheet, Pos, srcRect, color, Rotation, Orgin.ToVector2(), ScaleSize, SpriteEffect, 0);
    }

    public int[] Inject(params int[] sequence) {
      Sequence = sequence;
      return sequence;
    }

  }
}
