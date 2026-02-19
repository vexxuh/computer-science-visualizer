using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace comp_sci.Utilities;

public class PrimitiveRenderer {
    public Texture2D Pixel { get; }

    public PrimitiveRenderer(GraphicsDevice graphicsDevice) {
        Pixel = new Texture2D(graphicsDevice, 1, 1);
        Pixel.SetData(new[] { Color.White });
    }

    public void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, int thickness = 1) {
        var edge = end - start;
        float length = edge.Length();
        if (length < 0.001f) return;

        float angle = (float)Math.Atan2(edge.Y, edge.X);
        spriteBatch.Draw(Pixel,
            new Rectangle((int)start.X, (int)start.Y, (int)length, thickness),
            null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
    }

    public void FillRect(SpriteBatch spriteBatch, Rectangle rect, Color color) {
        spriteBatch.Draw(Pixel, rect, color);
    }
}
