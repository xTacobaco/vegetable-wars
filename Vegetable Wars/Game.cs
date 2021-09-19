using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using System.Collections.Generic;

using Vegetable_Wars.Rooms;
using Vegetable_Wars.Visual;
using Vegetable_Wars.Funcs;
using Vegetable_Wars.Forms;

namespace Vegetable_Wars {
  public class VW : Game {
    public static uint FrameCount = 0;
    public static GraphicsDeviceManager Graphics;
    public static NetworkHandler netHandler;
    public static Vector2 GameWindow;
    public static List<SoundEffect> soundEffects = new List<SoundEffect>();
    static bool exitGame = false;

    public static string Enemy = "";
    public static VegetableId EnemyType = 0;

    static string username = null;
    public static string Username {
      get { return username; }
      set { if (username == null) username = value; }
    }
    public static VegetableId Type = 0;

    SpriteBatch spriteBatch;
    public static SpriteFont GlobalFont;

    Texture2D mouseTexture;
    public static Point MousePos;
    public static Texture2D LoadTexture, VeggieSheet, CardSheet, Shield;

    public static Texture2D DisplaySprite;
    public static VegetableDisplay PlayerDisplay;
    public static VegetableDisplay EnemyDisplay;

    public static Title title;
    public static Island island;

    static RoomType index;
    public static RoomType Index {
      get { return index; }
      set {
        index = value;
        foreach (var button in Rooms[(int)Index].buttonList)
          button.Down = true;
      }
    }
    public static List<IRoom> Rooms;

    public VW(NetworkHandler nh) {
      Graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
      netHandler = nh;
      GameWindow = new Vector2(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);
    }

    protected override void Initialize() {
      base.Initialize();
    }

    protected override void LoadContent() {
      spriteBatch = new SpriteBatch(GraphicsDevice);
      
      VeggieSheet = Content.Load<Texture2D>("spriteSheet");
      Shield = Content.Load<Texture2D>("shield");
      CardSheet = Content.Load<Texture2D>("cardSheet");
      mouseTexture = Content.Load<Texture2D>("mouse");
      LoadTexture = Content.Load<Texture2D>("load");
      GlobalFont = Content.Load<SpriteFont>("8bit");

      DisplaySprite = Content.Load<Texture2D>("displaySprite");
      PlayerDisplay = new VegetableDisplay(new Vector2(-200, 0));
      EnemyDisplay = new VegetableDisplay(new Vector2(GameWindow.X, 0));

      title = new Title(new Vector2(GameWindow.X / 2, 90), Content.Load<Texture2D>("VW"));
      island = new Island(new Vector2(GameWindow.X / 2, 360), Content.Load<Texture2D>("bg"), Content.Load<Texture2D>("water"));

      soundEffects.Add(Content.Load<SoundEffect>("click"));
      soundEffects.Add(Content.Load<SoundEffect>("clack"));

      Rooms = new List<IRoom>() {
        new Login(),
        new Menu(),
        new Options(),
        new Lobby()
      };
    }

    protected override void UnloadContent() { }

    protected override void Update(GameTime gameTime) {
      FrameCount += 1;
      MousePos = new Point(Mouse.GetState().X, Mouse.GetState().Y);
      netHandler.Update();

      PlayerDisplay.Update(gameTime);
      EnemyDisplay.Update(gameTime);

      title.Update(gameTime);
      island.Update(gameTime);

      Rooms[(int)Index].Update(gameTime);
      if (exitGame) {
        Exit();
        System.Environment.Exit(0);
      }
      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
      GraphicsDevice.Clear(new Color(92, 169, 224));
      spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
      PlayerDisplay.Draw(spriteBatch);
      EnemyDisplay.Draw(spriteBatch);

      title.Draw(spriteBatch);
      island.Draw(spriteBatch);

      Rooms[(int)Index].Draw(spriteBatch);
      spriteBatch.Draw(mouseTexture, MousePos.ToVector2(), null, Color.White, 0, new Vector2(0, 0), 3, SpriteEffects.None, 0);
      spriteBatch.End();
      base.Draw(gameTime);
    }

    public static void ExitGame() {
      exitGame = true;
    }
  }
}
