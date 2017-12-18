using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vegetable_Wars.Visual {
  public class Island {
    public Vector2 pos;
    Vector2 offset;
    Texture2D islandTexture;
    Texture2D waterTexture;
    public bool Focus = false;

    public Island(Vector2 p, Texture2D islandText, Texture2D waterText) {
      pos.X = p.X - (islandText.Width * 3 / 2);
      pos.Y = p.Y;
      islandTexture = islandText;
      waterTexture = waterText;
    }

    public void Update(GameTime gameTime) {
      pos.Y = (Focus ? MathHelper.Lerp(pos.Y, 275, 0.01f) : MathHelper.Lerp(pos.Y, 360, 0.01f));
      offset.Y = VW.FrameCount % 120 <= 60 ? 4 : 0;
    }

    public void Draw(SpriteBatch spriteBatch) {
      spriteBatch.Draw(islandTexture, pos, null, Color.White, 0, new Vector2(0, 0), 3, SpriteEffects.None, 0);
      spriteBatch.Draw(waterTexture, pos + offset, null, Color.White, 0, new Vector2(0, 0), 3, SpriteEffects.None, 0);
    }
  }
}
