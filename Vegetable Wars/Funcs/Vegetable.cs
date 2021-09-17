using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vegetable_Wars.Funcs {
  public class Vegetable {
    Vector2 position;
    SpriteManager sprite;
    
    int health = 100;
    public int Health {
      get { return (health < 0) ? 0 : (health > 100 ? 100 : health);   }
      set { health = (value < 0) ? 0 : (value > 100 ? 100 : value); }
    }

    int magicBlock = 0;
    public int MagicBlock {
      get { return (magicBlock < 0) ? 0 : magicBlock;   }
      set { magicBlock = (value < 0) ? 0 : value; }
    }

    int attackBlock = 0;
    public int AttackBlock {
      get { return (attackBlock < 0) ? 0 : attackBlock;   }
      set { attackBlock = (value < 0) ? 0 : value; }
    }

    Texture2D healthBarOutline = new Texture2D(VW.Graphics.GraphicsDevice, 104, 9);
    Texture2D healthBar;
    Color[] hboColor, hbColor;

    public Vegetable(Vector2 pos, Texture2D spriteSheet) {
      position = pos;
      sprite = new SpriteManager() {
        SpriteSheet = spriteSheet,
        Speed = 7.5,
        ScaleSize = 3,
        Pos = position,
        TileSize = new Point(17, 18),
        Orgin = new Point(17 / 2, 18),
        SpriteEffect = (pos.X < VW.GameWindow.X / 2 ? SpriteEffects.FlipHorizontally : SpriteEffects.None)
      };
      sprite.Inject(0, 0, 0, 0, 1, 2);
      
      hboColor = new Color[healthBarOutline.Width*healthBarOutline.Height];
 
      for (int i = 0; i < hboColor.Length; i++)
        hboColor[i] = new Color(0, 0, 0, 1f);
      healthBarOutline.SetData(hboColor);

      healthBar = new Texture2D(VW.Graphics.GraphicsDevice, 1, 1);
      healthBar.SetData(new[] { new Color(81, 164, 82) });
    }

    public void Update(GameTime gameTime) {
      position.Y = VW.island.pos.Y;
      sprite.Pos = position;
      sprite.HOffset = (int)(position.X < VW.GameWindow.X / 2 ? VW.Type : VW.EnemyType);
      sprite.Update(gameTime);

      if (health > 0) {
        //healthBar.Width = Health;
        //healthBar = new Texture2D(VW.Graphics.GraphicsDevice, Health, 5);

        hbColor = new Color[healthBar.Width*healthBar.Height];
        for (int i = 0; i < hbColor.Length; i++)
          hbColor[i] = new Color(81, 164, 82);

        healthBar.SetData(hbColor);
      }
    }

    public void Draw(SpriteBatch spriteBatch) {
      spriteBatch.Draw(healthBarOutline, position - new Vector2(52,72), Color.White);
      if (health > 0) spriteBatch.Draw(healthBar, new Rectangle((position - new Vector2(50,70)).ToPoint(), new Point(health, 5)), Color.White);
      sprite.Draw(spriteBatch);
    }
  }
  public enum VegetableId {
    Eggplant,
    Tomato,
    Carrot
  }
}
