using comp_sci.Utilities;
using comp_sci.Visualization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace comp_sci.Screens;

public class SearchScreen : IScreen {
    private readonly SearchVisualizer _visualizer;
    private readonly SearchRenderer _renderer;
    private ScreenType? _transition;

    public ScreenType? RequestedTransition {
        get {
            var t = _transition;
            _transition = null;
            return t;
        }
    }

    public SearchScreen(SearchVisualizer visualizer, SearchRenderer renderer) {
        _visualizer = visualizer;
        _renderer = renderer;
    }

    public void Update(GameTime gameTime, InputManager input) {
        if (input.IsKeyPressed(Keys.Back))
            _transition = ScreenType.MainMenu;
        if (input.IsKeyPressed(Keys.Space))
            _visualizer.TogglePause();
        if (input.IsKeyPressed(Keys.R))
            _visualizer.Restart();
        if (input.IsKeyPressed(Keys.Right))
            _visualizer.NextAlgorithm();
        if (input.IsKeyPressed(Keys.Left))
            _visualizer.PreviousAlgorithm();
        if (input.IsKeyPressed(Keys.Up))
            _visualizer.SpeedUp();
        if (input.IsKeyPressed(Keys.Down))
            _visualizer.SlowDown();

        _visualizer.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch) {
        _renderer.Draw(spriteBatch, _visualizer);
    }
}
