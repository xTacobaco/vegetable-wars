using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vegetable_Wars.Funcs;

namespace Vegetable_Wars.Visual {
  class Loading {
    Vector2 pos;
    Texture2D texture = VW.LoadTexture;
    SpriteManager sprite;
    float rotation = 0;

    public Loading(Vector2 p) {
      pos = p;
      sprite = new SpriteManager() {
        SpriteSheet = texture,
        ScaleSize = 2,
        Orgin = new Point(8, 8),
        Pos = pos,
        TileSize = new Point(16, 16),
        Speed = 7.5
      };
      sprite.Inject(0, 1, 2);
    }

    public void Update(GameTime gameTime) {
      sprite.Rotation = rotation;
      sprite.Pos = pos;
      sprite.Update(gameTime);
      if (sprite.Index == sprite.Sequence.Length - sprite.Speed / 60)
        rotation -= 1.57079633f;
    }

    public void Draw(SpriteBatch spriteBatch) {
      sprite.Draw(spriteBatch);
    }
  }
}
