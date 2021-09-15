using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lidgren.Network;

using Vegetable_Wars.Funcs;
using Vegetable_Wars.Forms;
using Vegetable_Wars.Visual;

namespace Vegetable_Wars.Rooms {
  class Lobby : Room {
    Loading loading;
    Label searchLabel;
    Label gameInfo;
    Button buttonPlay;

    Vegetable player;
    Vegetable enemy;
    public List<Card> Hand = new List<Card>();

    bool searching = true;
    bool waiting = false;
    bool won = false;
    bool lose = false;
    public Lobby() {
      Index = new List<IForm>();
      searchLabel = new Label(new Point((int)VW.GameWindow.X/2, 260), "Looking for opponent!") { Centered = true };
      buttonPlay = new Button(new List<IForm>(), new Point((int)VW.GameWindow.X / 2 - 60, (int)VW.GameWindow.Y / 2 + 155), "Play", SndEffect.enter, play);
      gameInfo = new Label(new Point((int)VW.GameWindow.X / 2, (int)VW.GameWindow.Y / 2 + 50), "") { Centered = true };
      loading = new Loading(new Vector2(400, 235));
      player = new Vegetable(new Vector2(256, VW.island.pos.Y), VW.VeggieSheet);
      enemy = new Vegetable(new Vector2(VW.GameWindow.X - 260, VW.island.pos.Y), VW.VeggieSheet);

      buttonList.Add(new Button(new List<IForm>(), new Point(15, (int)VW.GameWindow.Y - 50), "Leave", SndEffect.leave, leave));
      buttonList.Add(buttonPlay);
      labelList.Add(searchLabel);
    }

    public override void Update(GameTime gameTime) {
      keyboardNavigation();

      findOpponent();
      getContent();
      getWinner();

      if (enemy.Health == 0) {
        won = true;
      }
      lose = player.Health == 0 && !won;

      if (searching)
        loading.Update(gameTime);
      else
        VW.EnemyDisplay.onScreen = true;

      searchLabel.Visible = searching;
      gameInfo.Visible = !searching;
      player.Update(gameTime);
      enemy.Update(gameTime);

      foreach (var button in buttonList) button.Update(gameTime);
      foreach (var label in labelList) label.Update(gameTime);
      foreach (var input in inputList) input.Update(gameTime);
      foreach (var card in Hand)
        if (card != null) card.Update(gameTime);

      for (int i = 0; i < Hand.Count; i++)
        if (!Hand[i].Selected)
          Hand[i].GoToPos = new Vector2(VW.GameWindow.X / 2 + i * 70 - Hand.Count * 70 / 2, VW.GameWindow.Y / 2 + 90);

      getGameInfo();
      gameInfo.Update(gameTime);

      if (won) {
        searchLabel.Visible = true;
        searchLabel.Pos = new Point((int)VW.GameWindow.X/2, 230);
        searchLabel.Text = "Congratulations " + VW.Username + ",\nyou won against " + VW.Enemy;
        buttonPlay.Visible = false;
        gameInfo.Visible = false;
      } else if (lose) {
        searchLabel.Visible = true;
        searchLabel.Pos = new Point((int)VW.GameWindow.X/2, 230);
        searchLabel.Text = "Sorry " + VW.Username + ",\nyou lost against " + VW.Enemy;
        buttonPlay.Visible = false;
        gameInfo.Visible = false;
      }

    }

    public override void Draw(SpriteBatch spriteBatch) {
      player.Draw(spriteBatch);

      if (searching) {
        spriteBatch.Draw(Shadow, new Vector2(0, 150), Color.White);
        loading.Draw(spriteBatch);
      } else {
        enemy.Draw(spriteBatch);
        foreach (var card in Hand)
          if (card != null) card.Draw(spriteBatch);
      }
      if (won || lose)
        spriteBatch.Draw(Shadow, new Vector2(0, 150), Color.White);

      foreach (var button in buttonList) button.Draw(spriteBatch);
      foreach (var label in labelList) label.Draw(spriteBatch);
      foreach (var input in inputList) input.Draw(spriteBatch);

      gameInfo.Draw(spriteBatch);
    }

    private void leave() {
      NetClient client = VW.netHandler.Peer;
      var msg = client.CreateMessage();
      msg.Write(2);
      client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);

      Hand = new List<Card>();
      won = false;
      searching = true;

      VW.island.Focus = false;
      VW.title.onScreen = true;
      VW.PlayerDisplay.onScreen = false;
      VW.EnemyDisplay.onScreen = false;
      VW.Index = RoomType.Menu;
      VW.Rooms.Remove(this);
      VW.Rooms.Add(new Lobby());
    }

    protected void findOpponent() {
      var msg = VW.netHandler.getMessage("found");
      if (msg != null) {
        searching = false;
        string user = msg.ReadString();
        if (user == VW.Username) {
          waiting = false;
          msg.ReadInt32();
          VW.Enemy = msg.ReadString();
          VW.EnemyType = (VegetableId)msg.ReadInt32();
        } else {
          waiting = true;
          VW.Enemy = user;
          VW.EnemyType = (VegetableId)msg.ReadInt32();
        }
        Random random = new Random();
        for (int i = 0; i < 5; i++)
          createCard(random);

        player.Health = 100;
        enemy.Health = 100;
      }
    }
    protected void getContent() {
      var msg = VW.netHandler.getMessage("place");
      if (msg != null) {
        waiting = false;
        cardHandler(enemy, msg.ReadString());
      }
    }
    protected void getWinner() {
      if (VW.netHandler.getMessage("won") != null && lose == false) {
        won = true;
      }
    }
    private void play() {
      List<Card> cards = Hand.Where(o => o != null).Where(x => x.Selected).ToList();
      foreach (var card in cards)
        Hand.Remove(card);
      Random random = new Random();
      createCard(random);
      waiting = true;

      var client = VW.netHandler.Peer;
      var msg = client.CreateMessage();
      msg.Write(3);
      msg.Write(string.Join(",", cards.Select(y => (int)y.Type)));
      client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
      cardHandler(player, cards);
    }
    private void getGameInfo() {
      CardType[] cards = Hand.Where(o => o != null).Where(x => x.Selected).Select(y => y.Type).ToArray();
      getGameInfo(cards);
    }
    private void getGameInfo(CardType[] cards) {
      buttonPlay.Visible = !searching && !waiting;
      buttonPlay.Text = (cards.Length > 0 ? "Play" : "Skip");
      gameInfo.Text = (waiting ? "Enemy players turn, please wait." : "");
      if (gameInfo.Text == "")
        gameInfo.Text = string.Join(" + ", cards);
    }
    private void cardHandler(Vegetable sender, string cards) {
      var other = (sender == player ? enemy : player);
      int magi = cards.ToCharArray().Count(c => c == '0');
      int atta = cards.ToCharArray().Count(c => c == '1');
      int heal = cards.ToCharArray().Count(c => c == '2');
      int smag = cards.ToCharArray().Count(c => c == '3');
      int satt = cards.ToCharArray().Count(c => c == '4');

      other.Health -= 25 * (int)Math.Pow(atta * (other.AttackBlock > 0 ? 0 : 1), 2);
      other.Health -= 25 * (int)Math.Pow(magi * (other.AttackBlock > 0 ? 0 : 1), 2);
      other.AttackBlock--;
      other.MagicBlock--;

      sender.AttackBlock += satt;
      sender.MagicBlock += smag;
      sender.Health += 25 * (int)Math.Pow(heal, 2);
    }
    private void cardHandler(Vegetable veggie, List<Card> cards) { cardHandler(veggie, string.Join(",", cards.Select(y => (int)y.Type))); }

    private void createCard(Random random) {
      Hand.Add(new Card((CardType)random.Next(5), new Vector2(VW.GameWindow.X / 2 + Hand.Count * 70, VW.GameWindow.Y / 2 + 90)));
    }
  }
}
