using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vegetable_Wars.Forms {
  public class Label : IForm {
    public Point Pos { get; set; }
    public string Text { get; set; } = "";
    public string Content { get; }
    public bool Selected { get; set; }
    public Color Col { get; set; } = Color.White;
    public bool Visible = true;
    public bool Centered = false;
    SpriteFont spriteFont = VW.GlobalFont;

    public Label(Point pos, string text) {
      Pos = pos;
      Text = text;
    }

    public void Update(GameTime gameTime) { }

    public void Draw(SpriteBatch spriteBatch) {
      if (Visible && Text != null)
        spriteBatch.DrawString(spriteFont, Text, Pos.ToVector2() - new Vector2(Centered ? VW.GlobalFont.MeasureString(Text).X / 2 : 0, 0), Col);
    }
  }
}
