using System.Threading;
using Lidgren.Network;
using System.Collections.Generic;
using System.Diagnostics;

namespace Vegetable_Wars.Funcs {
  public class NetworkHandler {
    public NetClient peer;
    public Dictionary<string,NetIncomingMessage> Unhandled = new Dictionary<string,NetIncomingMessage>();

    public NetworkHandler(NetClient p) {
      peer = p;
    }

    public NetIncomingMessage getMessage(string key) {
      NetIncomingMessage imsg = null;
      var index = VW.Index;

      while (VW.Index == index) {
        foreach (var msg in Unhandled) {
          if (msg.Key == key) {
            imsg = msg.Value;
            Unhandled.Remove(msg.Key);
            return imsg;
          }
        }
        imsg = peer.ReadMessage();
        if (imsg != null) {
          string currentKey = imsg.ReadString();
          if (currentKey == key)
            return imsg;
          else if (!Unhandled.ContainsKey(currentKey))
            Unhandled.Add(currentKey, imsg);
        }
        Thread.Sleep(1);
      }
      return null;
    }

    public NetIncomingMessage getMessage(string key, int timeout) {
      NetIncomingMessage imsg = null;
      int num = 0;

      while (num < timeout) {
        foreach (var msg in Unhandled) {
          if (msg.Key == key) {
            imsg = msg.Value;
            Unhandled.Remove(msg.Key);
            return imsg;
          }
        }
        imsg = peer.ReadMessage();
        if (imsg != null) {
          string currentKey = imsg.ReadString();
          if (currentKey == key)
            return imsg;
          else if (!Unhandled.ContainsKey(currentKey))
            Unhandled.Add(currentKey, imsg);
        }
        num++;
        Thread.Sleep(1);
      }
      return null;
    }
  }
}
