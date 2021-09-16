using System;
using System.Collections.Generic;
using System.Linq;
using Lidgren.Network;

namespace Server {
  class Room {
    List<UserData> players = new List<UserData>();

    public bool Free {
      get { return players.Count < 2; }
    }
    bool started = false;

    public Room() { 
      Program.Rooms.Add(this);
      Console.WriteLine("Room initiated...");
    }

    public void Winner(UserData player) {
      var msg = Program.server.CreateMessage();
      msg.Write("won");
      Program.server.SendMessage(msg, player.Connection, NetDeliveryMethod.ReliableOrdered);
    }
    public void Winners() {
      foreach (var player in players)
        Winner(player);
    }

    public void AddPlayer(UserData player) {
      if (Free)
        players.Add(player);
    }

    public void SendInfo(NetIncomingMessage imsg) {
      var omsg = Program.server.CreateMessage();
      omsg.Write("place");
      omsg.Write(imsg.ReadString());
      NetConnection recipent = players.Where(x => x.Connection != imsg.SenderConnection).Select(x => x.Connection).First();
      recipent.SendMessage(omsg, NetDeliveryMethod.ReliableOrdered, 0);
      Console.WriteLine("Data passed on. (place)");
    }

    public bool FindPlayer(UserData player) {
      return players.Contains(player);
    }

    public void Update() {
      if (!Free && !started) {
        var msg = Program.server.CreateMessage();
        msg.Write("found");
        msg.Write(players[0].Username);
        msg.Write((int)players[0].id);
        msg.Write(players[1].Username);
        msg.Write((int)players[1].id);
        List<NetConnection> connections = players.Select(x => x.Connection).Where(x => x != null).ToList();
        Program.server.SendMessage(msg, connections, NetDeliveryMethod.ReliableOrdered, 0);
        Console.WriteLine($"Room started... {players[0].Username} vs. {players[1].Username}");
        started = true;
      }
    }
  }
}
