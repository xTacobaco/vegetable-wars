using System;
using Vegetable_Wars.Funcs;
using Lidgren.Network;

namespace Vegetable_Wars {
#if WINDOWS || LINUX
  public static class Program {
    [STAThread]
    static void Main() {
      NetPeerConfiguration config = new NetPeerConfiguration("QWERTY");
      config.PingInterval = 0.01f;
      config.ConnectionTimeout = 1f;

      NetClient client = new NetClient(config);
      client.Start();
      client.Connect("94.254.74.192", 2220);

      NetworkHandler netHandler = new NetworkHandler(client);
      using (var game = new VW(netHandler))
        game.Run();
    }
  }
#endif
}
