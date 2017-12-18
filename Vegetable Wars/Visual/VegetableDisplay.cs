using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Vegetable_Wars.Forms;

namespace Vegetable_Wars.Visual {
  public class VegetableDisplay {
    Texture2D texture;
    Vector2 position;
    public bool onScreen = false;
    Label username;
    public string content = "";

    public VegetableDisplay(Vector2 pos) {
      texture = VW.DisplaySprite;
      position = pos;
      username = new Label((pos + new Vector2(100, 100) + new Vector2((pos.X < VW.GameWindow.X / 2 ? -20 : 20), 0)).ToPoint(), content);
      username.Centered = true;
    }

    public void Update(GameTime gameTime) {
      content = (position.X < VW.GameWindow.X / 2 ? VW.Username : VW.Enemy);
      position.X = (onScreen ? MathHelper.Lerp(position.X, (position.X < VW.GameWindow.X / 2 ? 0 : VW.GameWindow.X - 200), 0.01f) : MathHelper.Lerp(position.X, (position.X < VW.GameWindow.X / 2 ? -300 : VW.GameWindow.X + 100), 0.01f));
      username.Text = content;
      username.Pos = (position + new Vector2(100, 100) + new Vector2((position.X < VW.GameWindow.X / 2 ? -20 : 20), 0)).ToPoint();
    }

    public void Draw(SpriteBatch spriteBatch) {
      var spriteEffect = (position.X < VW.GameWindow.X / 2 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
      var rectangle = new Rectangle(0, 100 * (int)(position.X < VW.GameWindow.X / 2 ? VW.Type : VW.EnemyType), 200, 100);
      spriteBatch.Draw(texture, position, rectangle, Color.White, 0, new Vector2(0), 1, spriteEffect, 0);
      username.Draw(spriteBatch);
    }
  }
}
