using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Linq;

using Vegetable_Wars.Forms;
using System;

namespace Vegetable_Wars.Rooms {
  public interface IRoom { 
    List<Button> buttonList { get; set; }
    List<Label> labelList { get; set; }
    List<Input> inputList { get; set; }
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
  }

  abstract class Room : IRoom, IDisposable {
    public List<Button> buttonList { get; set; }
    public List<Label> labelList { get; set; }
    public List<Input> inputList { get; set; }

    public List<IForm> Index { get; set; }
    public int selectedIndex = 0;

    Keys[] lastKeys = new Keys[0];
    public static readonly HttpClient loginValid = new HttpClient();

    public Texture2D Shadow = new Texture2D(VW.Graphics.GraphicsDevice, (int)VW.GameWindow.X, 200);
    Color[] shadowColor;

    public Room() {
      shadowColor = new Color[Shadow.Width*Shadow.Height];
      for (int i = 0; i < shadowColor.Length; i++)
        shadowColor[i] = new Color(0, 0, 0, 0.6f);
      Shadow.SetData(shadowColor);
      Index = new List<IForm>();
      buttonList = new List<Button>();
      labelList = new List<Label>();
      inputList = new List<Input>();
    }

    public virtual void Update(GameTime gameTime) { }
    public virtual void Draw(SpriteBatch spriteBatch) { }

    public void keyboardNavigation() {
      var keys = Keyboard.GetState().GetPressedKeys();
      if (Enumerable.SequenceEqual(keys, lastKeys)) return;
      lastKeys = keys;

      if (keys.Length <= 0) return;
      if (keys[0] == Keys.Tab) {
        selectedIndex++;
        selectedIndex = (selectedIndex >= Index.Count ? 0 : selectedIndex);
        foreach (var index in Index) index.Selected = false;
        if (Index.Count > 0) Index[selectedIndex].Selected = true;
      }
    }

    protected async Task<int> request(Dictionary<string, string> values) {
      var content = new FormUrlEncodedContent(values);
      HttpResponseMessage request;
      int result;
      try {
        request = await loginValid.PostAsync("http://127.0.0.1/", content);
        result = int.Parse(await request.Content.ReadAsStringAsync());
      } catch {
        result = 4;
      }
      return result;
    }

    protected async Task<bool> impostor(string username) {
      bool result = true;
      var client = VW.netHandler.Peer;
      var msg = client.CreateMessage();
      msg.Write(0);
      msg.Write(username);
      client.SendMessage(msg, Lidgren.Network.NetDeliveryMethod.ReliableOrdered);
      result = await Task.Run(() => VW.netHandler.getMessage("clean", 500)) != null;
      return result;
    }

    public void Dispose() {
      Shadow.Dispose();
    }
  }

  public enum RoomType {
    Login,
    Menu,
    Options,
    Lobby
  }
}
