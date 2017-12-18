using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace Vegetable_Wars.Forms {
  public class Button : Form, IForm {
    Label lbl;
    Color bgColor;
    public bool Visible { get; set; } = true;
    public string Text { get; set; }
    public Point Pos { get; set; }
    public string Content { get { return Text; } }

    Point size = new Point(9 * 14, 21);
    public Point Size { get { return size; } set { size = value; } }

    public bool Selected { get; set; }
    private SoundEffect click;

    public delegate void Action();
    public Action Pressed;
    public bool Down = false;

    private Color[] Green = new Color[] { new Color(81, 164, 82), new Color(60, 130, 63), new Color(42, 111, 55) };

    public Button(List<IForm> index, Point pos, string text, SndEffect sound, Action action) {
      Pos = pos;
      Text = text;
      lbl = new Label(pos, Text);
      Pressed = action;
      click = VW.soundEffects[(int)sound];
      index.Add(this);
    }

    public void Update(GameTime gameTime) {
      lbl.Text = Text;
      bool enterDown = Keyboard.GetState().IsKeyDown(Keys.Enter);
      bool leftClick = Mouse.GetState().LeftButton == ButtonState.Pressed;
      if (!leftClick && !enterDown) Down = false;

      Rectangle collisionBox = new Rectangle(Pos.X, Pos.Y - 2, Size.X, Size.Y);
      Rectangle mouseBox = new Rectangle(VW.MousePos, new Point(1, 1));

      bgColor = Green[0];
      if (Selected || collisionBox.Intersects(mouseBox)) bgColor = Green[1];
      if (Visible && ((collisionBox.Intersects(mouseBox) && leftClick) || (Selected && enterDown))) {
        bgColor = Green[2];
        if (!Down) {
          Pressed();
          click.Play();
          Down = true;
        }
      }
    }

    public void Draw(SpriteBatch spriteBatch) {
      if (!Visible) return;
      var rect = textureRect(Size.ToVector2(), bgColor);
      var brect = textureRect(Size.ToVector2() + new Vector2(6, 6), Color.Black);
      var clear = textureRect(new Vector2(4, 4), bgColor);
      var box = textureRect(new Vector2(8, 8), Color.White);

      spriteBatch.Draw(brect, Pos.ToVector2() - new Vector2(6, 5), Color.White);
      spriteBatch.Draw(rect, Pos.ToVector2() - new Vector2(3, 2), Color.White);
      if (Selected) {
        spriteBatch.Draw(box, Pos.ToVector2() + new Vector2(Size.X - 15, (Size.Y + 10) / 2 - 10), Color.White);
        spriteBatch.Draw(clear, Pos.ToVector2() + new Vector2(Size.X - 15 + 2, (Size.Y + 10) / 2 - 10 + 2), Color.White);
      }

      lbl.Draw(spriteBatch);
    }
  }
}