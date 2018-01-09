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
      //A list of unhandled messages.
      Unhandled = new Dictionary<string, NetIncomingMessage>();
    }

    public void Update() {
      var imsg = Peer.ReadMessage();
      if (imsg != null) {
        //Adding key of message to an unhandled list. 
        string currentKey = imsg.ReadString();
        Unhandled.Add(currentKey, imsg);
      }
    }

    //Find Message by key from the unhandled list.
    private NetIncomingMessage findMessage(string key) {
      NetIncomingMessage imsg = null;
      foreach (var msg in Unhandled) {
        //Found key
        if (msg.Key == key) {
          imsg = msg.Value;
          //Returning message and removing it from unhandled.
          Unhandled.Remove(msg.Key);
          return imsg;
        }
      }
      return null;
    }
    
    //Keep on searching for Message by key from the unhandled list till it's found or room switches.
    public NetIncomingMessage getMessage(string key) {
      var index = VW.Index;

      while (VW.Index == index) {
        var msg = findMessage(key);
        //Validating that key was found before retrieving.
        if (msg != null) return msg;
        Thread.Sleep(1);
      }
      return null;
    }

    //Keep on searching for Message by key from unhandles list till it's found, room switches or the timeout is hit.
    public NetIncomingMessage getMessage(string key, int timeout) {
      int num = 0;

      while (num < timeout) {
        var msg = findMessage(key);
        //Validating that key was found before retrieving.
        if (msg != null) return msg;
        num++;
        Thread.Sleep(1);
      }
      return null;
    }
  }
}
