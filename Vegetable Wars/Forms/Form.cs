using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vegetable_Wars.Forms {
  public class Form {
    public Texture2D textureRect(Vector2 size, Color color) {
      Texture2D rectangle = new Texture2D(VW.Graphics.GraphicsDevice, (int)size.X, (int)size.Y);
      Color[] colorData = new Color[(int)size.X * (int)size.Y];

      for (int i = 0; i < colorData.Length; i++)
        colorData[i] = color;
      rectangle.SetData(colorData);
      return rectangle;
    }
  }
  public interface IForm {
    string Content { get; }
    bool Selected { get; set; }
    Point Pos { get; set; }

    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
  }
  public enum SndEffect {
    enter,
    leave
  }
}
