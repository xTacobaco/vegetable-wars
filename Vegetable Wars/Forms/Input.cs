using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;
using System.Linq;
using System;

namespace Vegetable_Wars.Forms {
  public class Input : Form, IForm {
    Label lbl;
    private Keys[] lastKeys = new Keys[0];
    private List<IForm> list;

    public Point Pos { get; set; } = new Point(0, 0);
    public Point Size { get; set; } = new Point(200, 20);

    public bool Spacable { get; set; } = false;
    public bool Privacy { get; set; } = false;
    public bool Selected { get; set; } = false;
    public int MaxLength { get { return Size.X / 13; } }

    private string text = "";
    public string Text {
      get { return (Privacy ? new String('*', text.Length) : text); }
      set { text = (value.Length < MaxLength ? value : value.Substring(0, MaxLength)); }
    }
    public string Content { get { return text; } }

    public Input(List<IForm> index, Point pos) {
      Pos = pos;
      lbl = new Label(Pos, Text);

      index.Add(this);
      list = index;
    }

    public void Update(GameTime gameTime) {
      lbl.Pos = Pos;
      lbl.Text = Text;

      bool leftClick = Mouse.GetState().LeftButton == ButtonState.Pressed;
      Rectangle collisionBox = new Rectangle(Pos.X, Pos.Y, Size.X, Size.Y);
      Rectangle mouseBox = new Rectangle(VW.MousePos, new Point(1, 1));
      if (collisionBox.Intersects(mouseBox) && leftClick) {
        foreach (var index in list)
          index.Selected = false;
        Selected = true;
      }

      if (!Selected) return;
      lbl.Text = Text + (VW.FrameCount % 60 < 30 ? "|" : "");

      //Gets keys, then filters out non-used keys.
      Keys[] pressedKeys = Keyboard.GetState().GetPressedKeys();
      pressedKeys = removeKeys(pressedKeys,
      Keys.RightShift, Keys.LeftShift,
      Keys.RightControl, Keys.LeftControl,
      Keys.LeftWindows, Keys.RightWindows);

      //Removing keys that already was handled.
      if (Enumerable.SequenceEqual(pressedKeys, lastKeys)) return;
      lastKeys = pressedKeys;
      if (pressedKeys.Length != 1) return;

      /* Text handling */
      Keys currentKey = pressedKeys[0];
      string stringKey = currentKey.ToString().ToLower();

      Text = text + (currentKey >= Keys.A && currentKey <= Keys.Z ? stringKey : "");                    //Add current letter.
      Text = text + (currentKey >= Keys.D0 && currentKey <= Keys.D9 ? stringKey.Substring(1, 1) : "");  //Add current number.
      Text = text + (Spacable && currentKey == Keys.Space ? " " : "");                                  //Add space.
      if (currentKey == Keys.Back && Text.Length > 0) Text = Text.Substring(0, Text.Length - 1);        //Remove last character. 
      Selected = (currentKey == Keys.Enter || currentKey == Keys.Escape ? false : Selected);            //Deseleting on common break-keys.
    }

    public void Draw(SpriteBatch spriteBatch) {
      var rect = textureRect(new Vector2(Size.X, 3), Color.White);
      spriteBatch.Draw(rect, Pos.ToVector2() + new Vector2(0, Size.Y), Color.White);
      lbl.Draw(spriteBatch);
    }

    private Keys[] removeKeys(Keys[] keys, params Keys[] removables) {
      foreach (var key in removables)
        if (keys.Contains(key))
          keys = keys.Where(x => x != key).ToArray();
      return keys;
    }
  }
}
