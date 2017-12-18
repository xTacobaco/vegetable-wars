using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vegetable_Wars.Visual {
  public class Title {
    Vector2 pos;
    Texture2D texture;
    float rotation = 0;
    bool right = true;
    public bool onScreen = true;

    public Title(Vector2 p, Texture2D t) {
      pos = p;
      texture = t;
    }

    public void Update(GameTime gameTime) {
      rotation += (right ? 0.001f : -0.001f);
      right = (rotation > 0.1f || rotation < -0.1f ? !right : right);
      pos.Y = (onScreen ? MathHelper.Lerp(pos.Y, 90, 0.01f) : MathHelper.Lerp(pos.Y, -90, 0.01f));
    }

    public void Draw(SpriteBatch spriteBatch) {
      spriteBatch.Draw(texture, pos, null, Color.White, rotation, new Vector2(texture.Width / 2, texture.Height / 2), 2, SpriteEffects.None, 0);
    }
  }
}
