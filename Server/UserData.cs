using System;
using Lidgren.Network;

namespace Server {
  class UserData {
    public NetConnection Connection = null;
    public VegetableId id;

    public string Username {
      set {
        username = value.Trim().ToLower();
      }
      get { return username; }
    }
    string username;
    
    public DateTime ConnectionTime;

    public UserData(NetConnection con) {
      ConnectionTime = DateTime.Now;
      Connection = con;
    }
  }

  public enum VegetableId {
    Eggplant,
    Tomato,
    Carrot
  }
}
