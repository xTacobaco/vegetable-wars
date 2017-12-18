using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

using Vegetable_Wars.Forms;
using Vegetable_Wars.Visual;

namespace Vegetable_Wars.Rooms {
  class Login : Room {
    Loading load;
    bool loading = false;

    string[] requestCode = new string[6] {
      "Error(0): no matching account.",
      "Registered successfully.",
      "Error(2): bad input. (4-16)",
      "Error(3): user already exists.",
      "Error(4): couldn't connect.",
      "Error(5): user already logged in."
    };

    Dictionary<string, string> values = new Dictionary<string, string> {
      { "seq", "QWERTY" }, { "status", "" }, { "user", "" }, { "pass", "" }
    };

    public Login() {
      labelList.Add(new Label(new Point(240, 170), "Username:"));
      inputList.Add(new Input(Index, new Point(350, 170)));
      labelList.Add(new Label(new Point(240, 205), "Password:"));
      inputList.Add(new Input(Index, new Point(350, 205)) { Privacy = true });
      labelList.Add(new Label(new Point(240, 243), "") { Col = new Color(197, 63, 59) });
      buttonList.Add(new Button(Index, new Point(340, 280), "Login", SndEffect.enter, login));
      buttonList.Add(new Button(Index, new Point(340, 310), "Register", SndEffect.enter, register));
      load = new Loading(new Vector2(397, 250));
      Index[selectedIndex].Selected = true;
    }

    public override void Update(GameTime gameTime) {
      keyboardNavigation();
      if (loading) load.Update(gameTime);

      foreach (var button in buttonList) button.Update(gameTime);
      foreach (var label in labelList) label.Update(gameTime);
      foreach (var input in inputList) input.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch) {
      spriteBatch.Draw(Shadow, new Vector2(0, 150), Color.White);
      if (loading) load.Draw(spriteBatch);

      foreach (var button in buttonList) button.Draw(spriteBatch);
      foreach (var label in labelList) label.Draw(spriteBatch);
      foreach (var input in inputList) input.Draw(spriteBatch);
    }

    private void valuesFill() {
      values["user"] = Index[0].Content;
      values["pass"] = Index[1].Content;
    }

    private async void login() {
      cleanUp();
      values["status"] = "log";

      labelList[2].Col = new Color(197, 63, 59);
      var result = await request(values);
      if (result == 1) {
        var username = Index[0].Content;
        if (await impostor(username)) {
          VW.Username = username;
          VW.Index = RoomType.Menu;
        } else
          labelList[2].Text = requestCode[5];
      } else labelList[2].Text = requestCode[result];
      loading = false;
    }

    private async void register() {
      cleanUp();
      values["status"] = "reg";

      var result = await request(values);
      labelList[2].Col = (result == 1 ? new Color(81, 164, 82) : new Color(197, 63, 59));
      labelList[2].Text = requestCode[result];
      loading = false;
    }
    private void cleanUp() {
      loading = true;
      labelList[2].Text = "";
      valuesFill();
    }
  }
}
