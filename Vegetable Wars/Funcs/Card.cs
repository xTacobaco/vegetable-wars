using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Vegetable_Wars.Funcs {
  class Card {
    Texture2D texture;

    public CardType Type;

    Vector2 currentPosition;
    public Vector2 GoToPos;

    public bool Selected = false;
    bool down = false;

    public Card(CardType card, Vector2 goToPos) {
      texture = VW.CardSheet;
      currentPosition = new Vector2(VW.GameWindow.X / 2 - 32, VW.GameWindow.Y + 40);
      GoToPos = goToPos;
      Type = card;
    }
    public void Update(GameTime gameTime) {
      bool leftClick = Mouse.GetState().LeftButton == ButtonState.Pressed;
      if (!leftClick) down = false;

      Rectangle collisionBox = new Rectangle((int)currentPosition.X, (int)currentPosition.Y, 64, 64);
      Rectangle mouseBox = new Rectangle(VW.MousePos, new Point(1, 1));

      if ((collisionBox.Intersects(mouseBox) && leftClick)) {
        if (!down) {
          Selected = !Selected;
          if (Selected)
            GoToPos.Y = VW.GameWindow.Y / 2 + 80;
          else
            GoToPos.Y = VW.GameWindow.Y / 2 + 90;
          down = true;
        }
      }

      currentPosition.X = MathHelper.Lerp(currentPosition.X, GoToPos.X, 0.1f);
      currentPosition.Y = MathHelper.Lerp(currentPosition.Y, GoToPos.Y, 0.1f);
    }
    public void Draw(SpriteBatch spriteBatch) {
      spriteBatch.Draw(texture, currentPosition, new Rectangle(16 * (int)Type, 0, 16, 16), Color.White, 0, Vector2.Zero, 3, SpriteEffects.None, 0);
    }
  }
  public enum CardType {
    Magic,
    Attack,
    Heal,
    Shield_Magic,
    Shield_Attack
  }
}
