using System.Threading;
using Lidgren.Network;
using System.Collections.Generic;
using System.Diagnostics;

namespace Vegetable_Wars.Funcs {
  public class NetworkHandler {
    public NetClient Peer;
    public Dictionary<string, NetIncomingMessage> Unhandled;

    public NetworkHandler(NetClient netPeer) {
      Peer = netPeer;
      Unhandled = new Dictionary<string, NetIncomingMessage>();
    }

    public void Update() {
      var imsg = Peer.ReadMessage();
      if (imsg != null) {
        string currentKey = imsg.ReadString();
        Unhandled.Add(currentKey, imsg);
      }
    }

    private NetIncomingMessage findMessage(string key) {
      NetIncomingMessage imsg = null;
      foreach (var msg in Unhandled) {
        if (msg.Key == key) {
          imsg = msg.Value;
          Unhandled.Remove(msg.Key);
          return imsg;
        }
      }
      return null;
    }

    public NetIncomingMessage getMessage(string key) {
      var index = VW.Index;

      while (VW.Index == index) {
        var msg = findMessage(key);
        if (msg != null) return msg;
        Thread.Sleep(1);
      }
      return null;
    }

    public NetIncomingMessage getMessage(string key, int timeout) {
      int num = 0;

      while (num < timeout) {
        var msg = findMessage(key);
        if (msg != null) return msg;
        num++;
        Thread.Sleep(1);
      }
      return null;
    }
  }
}
