using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;
using Vegetable_Wars.Forms;
using Vegetable_Wars.Funcs;

namespace Vegetable_Wars.Rooms {
  class Options : Room {
    SpriteManager sprite;
    int Selected = 0;

    public Options() {
      sprite = new SpriteManager() {
        SpriteSheet = VW.VeggieSheet,
        Speed = 7.5,
        ScaleSize = 3,
        Pos = new Vector2((340 + 450) / 2, 260),
        TileSize = new Point(17, 18),
        Orgin = new Point(17 / 2, 18)
      };
      sprite.Inject(0, 0, 0, 0, 1, 2);
      Index = new List<IForm>();
      labelList.Add(new Label(new Point(400, 180), VW.Type.ToString()) { Centered = true });
      buttonList.Add(new Button(new List<IForm>(), new Point(340, 235), "<", SndEffect.enter, goLeft, "small"));
      buttonList.Add(new Button(new List<IForm>(), new Point(450, 235), ">", SndEffect.enter, goRight, "small"));
      buttonList.Add(new Button(Index, new Point(340, 300), "Back", SndEffect.leave, back));
      labelList.Add(new Label(new Point(VW.Graphics.PreferredBackBufferWidth-250, VW.Graphics.PreferredBackBufferHeight-20), "William \"xTacobaco\" Beino"));
      Index[selectedIndex].Selected = true;
    }

    public override void Update(GameTime gameTime) {
      keyboardNavigation();

      foreach (var button in buttonList) button.Update(gameTime);
      foreach (var label in labelList) label.Update(gameTime);
      foreach (var input in inputList) input.Update(gameTime);
      sprite.HOffset = (int)VW.Type;
      labelList[0].Text = VW.Type.ToString();
    }

    public override void Draw(SpriteBatch spriteBatch) {
      spriteBatch.Draw(Shadow, new Vector2(0, 150), Color.White);

      foreach (var button in buttonList) button.Draw(spriteBatch);
      foreach (var label in labelList) label.Draw(spriteBatch);
      foreach (var input in inputList) input.Draw(spriteBatch);
      sprite.Draw(spriteBatch);
    }

    private void back() {
      VW.Index = RoomType.Menu;
    }
    private void goLeft() {
      Selected--;
      Selected = (Selected >= 0 ? Selected : 2);
      VW.Type = (VegetableId)Selected;
    }
    private void goRight() {
      Selected++;
      Selected = (Selected <= 2 ? Selected : 0);
      VW.Type = (VegetableId)Selected;
    }
  }
}
