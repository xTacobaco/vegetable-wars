using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;
using Vegetable_Wars.Forms;
using Lidgren.Network;

namespace Vegetable_Wars.Rooms {
  class Menu : Room {
    public Menu() {
      Index = new List<IForm>();
      buttonList.Add(new Button(Index, new Point(340, 190), "Play", SndEffect.enter, play));
      buttonList.Add(new Button(Index, new Point(340, 240), "Options", SndEffect.enter, options));
      buttonList.Add(new Button(Index, new Point(340, 290), "Exit", SndEffect.leave, exit));
      Index[selectedIndex].Selected = true;
    }

    public override void Update(GameTime gameTime) {
      keyboardNavigation();

      foreach (var button in buttonList) button.Update(gameTime);
      foreach (var label in labelList) label.Update(gameTime);
      foreach (var input in inputList) input.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch) {
      spriteBatch.Draw(Shadow, new Vector2(0, 150), Color.White);

      foreach (var button in buttonList) button.Draw(spriteBatch);
      foreach (var label in labelList) label.Draw(spriteBatch);
      foreach (var input in inputList) input.Draw(spriteBatch);
    }

    private void play() {
      VW.netHandler.Unhandled = new Dictionary<string,NetIncomingMessage>();
      VW.title.onScreen = false;
      VW.island.Focus = true;
      VW.PlayerDisplay.onScreen = true;
      var client = VW.netHandler.peer;
      var msg = client.CreateMessage();
      msg.Write(1);
      msg.Write((int)VW.Type);
      client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
      VW.Index = RoomType.Lobby;
    }
    private void options() {
      VW.Index = RoomType.Options;
    }
    private void exit() {
      VW.netHandler.peer.Disconnect("");
      VW.ExitGame();
    }
  }
}
