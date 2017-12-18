using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;
using System.Linq;
using System;

namespace Vegetable_Wars.Forms {
  class Input : Form, IForm {
    public Vector2 Pos { get; set; } = new Vector2(0, 0);
    public Vector2 Size { get; set; } = new Vector2(200, 20);
    public bool Privacy { get; set; } = false;
    public bool Selected { get; set; } = false;
    public int MaxLength { get; set; } = 14;
    public int Depth { get; set; } = 0;

    Keys[] lastKeys = new Keys[0];
    Label lbl;
    string text = "";
    public string Text {
      get { return (Privacy ? new String('*', text.Length) : text); }
      set { text = value; }
    }

    public Input(List<IForm> index, Vector2 pos) {
      index.Add(this);
      Pos = pos;
      lbl = new Label(Pos, Text);
    }

    public void Update(GameTime gameTime) {
      lbl.Pos = Pos;
      lbl.Text = Text;
      if (!Selected) return;
      lbl.Text = Text + (VW.frameCount % 60 < 30 ? "|" : "");
      
      //Gets keys, then filters out non-used keys.
      Keys[] pressedKeys = Keyboard.GetState().GetPressedKeys();
      pressedKeys = removeKeys(pressedKeys,
      Keys.RightShift,    Keys.LeftShift,
      Keys.RightControl,  Keys.LeftControl,
      Keys.LeftWindows,   Keys.RightWindows);

      //Removing keys that already was handled.
      Keys[] uniqueKeys = removeKeys(pressedKeys, lastKeys);
      if (uniqueKeys.Length == 0) return;
      lastKeys = pressedKeys;
      if (pressedKeys.Length != 1) return;

      Keys currentKey = pressedKeys[0];
      string stringKey = currentKey.ToString().ToLower();

      //Text handeling
      Text += (currentKey >= Keys.A && currentKey <= Keys.Z ? stringKey : "");                          //Add current letter.
      Text += (currentKey >= Keys.D0 && currentKey <= Keys.D9 ? stringKey.Substring(1, 1) : "");        //Add current number.
      Text =  (currentKey == Keys.Back && Text.Length > 0 ? Text.Substring(0, Text.Length - 1) : Text); //Remove last character. 
      Text += (currentKey == Keys.Space ? " " : "");                                                    //Add space.
      Selected = (currentKey == Keys.Enter || currentKey == Keys.Escape ? false : Selected);            //Deseleting on common break-keys.
    }

    public void Draw(SpriteBatch spriteBatch) {
      var rect = textureRect(new Vector2(Size.X, 3), Color.Black);
      spriteBatch.Draw(rect, Pos + new Vector2(0, Size.Y), Color.White);

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
