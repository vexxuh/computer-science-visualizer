using System;
using comp_sci.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace comp_sci.Visualization;

public class SearchRenderer {
    private readonly PrimitiveRenderer _primitives;
    private readonly SpriteFont _font;
    private readonly int _screenWidth;
    private readonly int _screenHeight;
    private const int TopMargin = 80;
    private const int BottomMargin = 40;
    private const float EliminatedHeightRatio = 0.15f;

    public SearchRenderer(PrimitiveRenderer primitives, SpriteFont font, int screenWidth, int screenHeight) {
        _primitives = primitives;
        _font = font;
        _screenWidth = screenWidth;
        _screenHeight = screenHeight;
    }

    public void Draw(SpriteBatch spriteBatch, SearchVisualizer visualizer) {
        var array = visualizer.Array;
        var barColors = visualizer.BarColors;
        var elimProgress = visualizer.EliminateProgress;
        int n = array.Length;

        float totalBarArea = _screenWidth - 40f;
        float barWidth = totalBarArea / n;
        float startX = 20f;
        int drawableHeight = _screenHeight - TopMargin - BottomMargin;
        int maxVal = array[n - 1];

        for (int i = 0; i < n; i++) {
            float fullHeight = (float)array[i] / maxVal * drawableHeight;
            float x = startX + i * barWidth;
            int actualBarWidth = Math.Max((int)barWidth - 2, 1);

            float t = elimProgress[i];
            float shrunkHeight = MathHelper.Lerp(fullHeight, fullHeight * EliminatedHeightRatio, t);
            int barTop = _screenHeight - BottomMargin - (int)shrunkHeight;

            var color = barColors[i];
            if (t > 0f) {
                float fade = MathHelper.Lerp(1f, 0.35f, t);
                color = new Color(
                    (int)(color.R * fade),
                    (int)(color.G * fade),
                    (int)(color.B * fade));
            }

            var rect = new Rectangle(
                (int)x,
                barTop,
                actualBarWidth,
                (int)shrunkHeight
            );
            _primitives.FillRect(spriteBatch, rect, color);

            if (t >= 1f) {
                float midY = barTop + shrunkHeight / 2f;
                _primitives.DrawLine(spriteBatch,
                    new Vector2(x, midY),
                    new Vector2(x + actualBarWidth, midY),
                    new Color(200, 50, 50), 2);
            }

            if (i == visualizer.FoundIndex) {
                var outlineRect = new Rectangle(
                    (int)x - 2, barTop - 2,
                    actualBarWidth + 4, (int)shrunkHeight + 4);
                DrawOutline(spriteBatch, outlineRect, Color.Lime, 2);
            }

            string label = array[i].ToString();
            var labelSize = _font.MeasureString(label);
            float labelX = x + (actualBarWidth - labelSize.X) / 2f;
            float labelY = _screenHeight - BottomMargin + 4;

            float scale = 1f;
            if (labelSize.X > barWidth - 2) {
                scale = (barWidth - 4) / labelSize.X;
                labelX = x + (actualBarWidth - labelSize.X * scale) / 2f;
            }

            var labelColor = t >= 1f ? new Color(80, 80, 80) : Color.LightGray;

            spriteBatch.DrawString(_font, label,
                new Vector2(labelX, labelY),
                labelColor,
                0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        if (visualizer.LowBound >= 0 && visualizer.HighBound >= 0 && !visualizer.Completed) {
            float lowX = startX + visualizer.LowBound * barWidth;
            float highX = startX + (visualizer.HighBound + 1) * barWidth - 2;
            float bracketY = TopMargin + 10;

            _primitives.DrawLine(spriteBatch,
                new Vector2(lowX, bracketY),
                new Vector2(highX, bracketY),
                Color.Cyan, 2);
            _primitives.DrawLine(spriteBatch,
                new Vector2(lowX, bracketY),
                new Vector2(lowX, bracketY - 8),
                Color.Cyan, 2);
            _primitives.DrawLine(spriteBatch,
                new Vector2(highX, bracketY),
                new Vector2(highX, bracketY - 8),
                Color.Cyan, 2);

            string boundsText = $"[{visualizer.LowBound}..{visualizer.HighBound}]";
            var boundsSize = _font.MeasureString(boundsText);
            float boundsX = (lowX + highX - boundsSize.X) / 2f;
            spriteBatch.DrawString(_font, boundsText,
                new Vector2(boundsX, bracketY + 4),
                Color.Cyan);
        }

        string targetText = $"Target: {visualizer.Target}";
        string algoText = $"{visualizer.CurrentAlgorithmName}   Speed: {visualizer.Speed}x   {(visualizer.Paused ? "[PAUSED]" : "")}";

        if (visualizer.FoundIndex >= 0)
            targetText += $"   FOUND at index {visualizer.FoundIndex}!";

        spriteBatch.DrawString(_font, algoText, new Vector2(10, 10), Color.White);
        spriteBatch.DrawString(_font, targetText, new Vector2(10, 35), Color.Orange);

        string controls = "Space:Pause  Arrows:Algo/Speed  R:New Search  Backspace:Menu";
        spriteBatch.DrawString(_font, controls, new Vector2(10, 57), Color.Gray);
    }

    private void DrawOutline(SpriteBatch spriteBatch, Rectangle rect, Color color, int thickness) {
        _primitives.FillRect(spriteBatch, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);
        _primitives.FillRect(spriteBatch, new Rectangle(rect.X, rect.Bottom - thickness, rect.Width, thickness), color);
        _primitives.FillRect(spriteBatch, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);
        _primitives.FillRect(spriteBatch, new Rectangle(rect.Right - thickness, rect.Y, thickness, rect.Height), color);
    }
}
