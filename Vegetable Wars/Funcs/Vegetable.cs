using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vegetable_Wars.Funcs {
  public class Vegetable {
    public int Health = 100;
    Vector2 position;
    SpriteManager sprite;

    public Vegetable(Vector2 pos, Texture2D spriteSheet) {
      position = pos;
      sprite = new SpriteManager() {
        SpriteSheet = spriteSheet,
        Speed = 7.5,
        ScaleSize = 3,
        Pos = position,
        TileSize = new Point(17, 18),
        Orgin = new Point(17 / 2, 18),
        SpriteEffect = (pos.X < VW.GameWindow.X / 2 ? SpriteEffects.FlipHorizontally : SpriteEffects.None)
      };
      sprite.Inject(0, 0, 0, 0, 1, 2);
    }

    public void Update(GameTime gameTime) {
      position.Y = VW.island.pos.Y;
      sprite.Pos = position;
      sprite.HOffset = (int)(position.X < VW.GameWindow.X / 2 ? VW.Type : VW.EnemyType);
      sprite.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch) {
      sprite.Draw(spriteBatch);
    }
  }
  public enum VegetableId {
    Eggplant,
    Tomato,
    Carrot
  }
}
