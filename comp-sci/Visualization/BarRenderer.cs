using comp_sci.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace comp_sci.Visualization;

public class BarRenderer {
    private readonly PrimitiveRenderer _primitives;
    private readonly SpriteFont _font;
    private readonly int _screenWidth;
    private readonly int _screenHeight;
    private const int TopMargin = 60;
    private const int BottomMargin = 10;

    public BarRenderer(PrimitiveRenderer primitives, SpriteFont font, int screenWidth, int screenHeight) {
        _primitives = primitives;
        _font = font;
        _screenWidth = screenWidth;
        _screenHeight = screenHeight;
    }

    public void Draw(SpriteBatch spriteBatch, int[] array, Color[] barColors, string algorithmName, bool paused, int stepsPerFrame) {
        int n = array.Length;
        float barWidth = (float)_screenWidth / n;
        int drawableHeight = _screenHeight - TopMargin - BottomMargin;

        int maxVal = n;

        for (int i = 0; i < n; i++) {
            float height = (float)array[i] / maxVal * drawableHeight;
            var rect = new Rectangle(
                (int)(i * barWidth),
                _screenHeight - BottomMargin - (int)height,
                System.Math.Max((int)barWidth - 1, 1),
                (int)height
            );
            _primitives.FillRect(spriteBatch, rect, barColors[i]);
        }

        string info = $"{algorithmName}   Speed: {stepsPerFrame}x   {(paused ? "[PAUSED]" : "")}";
        spriteBatch.DrawString(_font, info, new Vector2(10, 10), Color.White);

        string controls = "Space:Pause  Arrows:Algo/Speed  R:Reshuffle  Backspace:Menu";
        spriteBatch.DrawString(_font, controls, new Vector2(10, 35), Color.Gray);
    }
}
