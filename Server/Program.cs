using System;
using System.Linq;
using System.Collections.Generic;
using Lidgren.Network;
using System.Threading;

namespace Server {
  class Program {
    public static NetServer server;
    public static List<Room> Rooms = new List<Room>();
    static void Main(string[] args) {
      var connections = new Dictionary<NetConnection, UserData>();
      var config = new NetPeerConfiguration("QWERTY");
      config.Port = 2220;
      config.PingInterval = 0.01f;
      config.ConnectionTimeout = 1f;
      server = new NetServer(config);
      server.Start();

      DateTime time = DateTime.Now;
      while (true) {
        var imsg = server.ReadMessage();
        if (imsg != null) {
          switch (imsg.MessageType) {
            case NetIncomingMessageType.StatusChanged:
              var status = imsg.SenderConnection.Status;
              if (status == NetConnectionStatus.Connected) {
                UserData user = new UserData(imsg.SenderConnection);
                connections.Add(imsg.SenderConnection, user);
                Console.WriteLine($"User({imsg.SenderEndPoint}) Connected, current Connections: {connections.Count}.");
              }
              if (status == NetConnectionStatus.Disconnected) {
                var conn = connections[imsg.SenderConnection];
                foreach (var room in Rooms.Where(x => x.FindPlayer(conn))) {
                  Console.WriteLine($"User({imsg.SenderEndPoint}, {conn.Username}) left the room.");
                  room.Winners();
                }
                Rooms = Rooms.Where(x => !x.FindPlayer(conn)).ToList();
                connections.Remove(imsg.SenderConnection);
                Console.WriteLine($"User({imsg.SenderEndPoint}) Disconnected, current Connections: {connections.Count}.");
              }
              break;
            case NetIncomingMessageType.Data:
              var state = (MessageId)imsg.ReadInt32();
              var sender = connections[imsg.SenderConnection];
              switch (state) {
                case MessageId.setup:
                  var username = imsg.ReadString();
                  if (connections.Select(x => x.Value).Where(y => y.Username == username).Count() == 0) {
                    sender.Username = username;
                    Console.WriteLine($"User({imsg.SenderEndPoint}) Logged In with the Username: {sender.Username}.");
                    NetOutgoingMessage sendMsg = server.CreateMessage();
                    sendMsg.Write("clean");
                    imsg.SenderConnection.SendMessage(sendMsg, NetDeliveryMethod.ReliableOrdered, 0);
                  } else {
                    Console.WriteLine($"User({imsg.SenderEndPoint}) Tried to Log In with the Username: {username}.");
                  }
                  break;
                case MessageId.search:
                  sender.id = (VegetableId)imsg.ReadInt32();
                  Room free = null;
                  foreach (var room in Rooms) {
                    if (room.Free == true) {
                      free = room;
                      break;
                    }
                  }
                  if (free == null)
                    free = new Room();
                  free.AddPlayer(sender);
                  break;
                case MessageId.leave:
                  foreach (var room in Rooms.Where(x => x.FindPlayer(sender))) {
                    Console.WriteLine($"User({imsg.SenderEndPoint}, {sender.Username}) left the match, resulting in a win for the other player.");
                    room.Winners();
                  }
                  Rooms = Rooms.Where(x => !x.FindPlayer(sender)).ToList();
                  break;
                case MessageId.placed:
                  foreach (var room in Rooms.Where(x => x.FindPlayer(sender))) {
                    room.SendInfo(imsg);
                  }
                  break;
              }
              break;
          }

          foreach (var room in Rooms)
            room.Update();
        }
        var delta = DateTime.Now - time;
        Thread.Sleep((int)Math.Max(66.6 - delta.TotalMilliseconds, 0)); //15hz clock
        time = DateTime.Now;
      }
    }
  }
  public enum MessageId {
    setup,
    search,
    leave,
    placed
  }
}
