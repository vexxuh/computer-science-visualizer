using comp_sci.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace comp_sci.Screens;

public class MenuScreen : IScreen {
    private readonly SpriteFont _font;
    private readonly PrimitiveRenderer _primitives;
    private readonly int _screenWidth;
    private readonly int _screenHeight;
    private readonly string[] _items = { "Sorting Algorithms", "Maze Solvers" };
    private readonly ScreenType[] _targets = { ScreenType.SortingVisualizer, ScreenType.MazeVisualizer };
    private int _selectedIndex;
    private ScreenType? _transition;

    public ScreenType? RequestedTransition {
        get {
            var t = _transition;
            _transition = null;
            return t;
        }
    }

    public MenuScreen(SpriteFont font, PrimitiveRenderer primitives, int screenWidth, int screenHeight) {
        _font = font;
        _primitives = primitives;
        _screenWidth = screenWidth;
        _screenHeight = screenHeight;
    }

    public void Update(GameTime gameTime, InputManager input) {
        if (input.IsKeyPressed(Keys.Up))
            _selectedIndex = (_selectedIndex - 1 + _items.Length) % _items.Length;
        if (input.IsKeyPressed(Keys.Down))
            _selectedIndex = (_selectedIndex + 1) % _items.Length;
        if (input.IsKeyPressed(Keys.Enter))
            _transition = _targets[_selectedIndex];
    }

    public void Draw(SpriteBatch spriteBatch) {
        string title = "Computer Science Visualizer";
        var titleSize = _font.MeasureString(title);
        float titleX = (_screenWidth - titleSize.X) / 2f;
        spriteBatch.DrawString(_font, title, new Vector2(titleX, 150), Color.White);

        for (int i = 0; i < _items.Length; i++) {
            var text = _items[i];
            var size = _font.MeasureString(text);
            float x = (_screenWidth - size.X) / 2f;
            float y = 300 + i * 60;

            bool selected = i == _selectedIndex;

            if (selected) {
                var highlightRect = new Rectangle(
                    (int)(x - 20), (int)(y - 8),
                    (int)(size.X + 40), (int)(size.Y + 16));
                _primitives.FillRect(spriteBatch, highlightRect, new Color(60, 60, 80));
            }

            var color = selected ? Color.Yellow : Color.Gray;
            spriteBatch.DrawString(_font, text, new Vector2(x, y), color);
        }

        string hint = "Up/Down: Select   Enter: Launch   Escape: Quit";
        var hintSize = _font.MeasureString(hint);
        float hintX = (_screenWidth - hintSize.X) / 2f;
        spriteBatch.DrawString(_font, hint, new Vector2(hintX, _screenHeight - 80), Color.DarkGray);
    }
}
