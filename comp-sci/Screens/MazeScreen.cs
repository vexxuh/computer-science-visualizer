using comp_sci.Utilities;
using comp_sci.Visualization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace comp_sci.Screens;

public class MazeScreen : IScreen {
    private readonly MazeVisualizer _visualizer;
    private readonly MazeRenderer _renderer;
    private ScreenType? _transition;

    public ScreenType? RequestedTransition {
        get {
            var t = _transition;
            _transition = null;
            return t;
        }
    }

    public MazeScreen(MazeVisualizer visualizer, MazeRenderer renderer) {
        _visualizer = visualizer;
        _renderer = renderer;
    }

    public void Update(GameTime gameTime, InputManager input) {
        if (input.IsKeyPressed(Keys.Back))
            _transition = ScreenType.MainMenu;
        if (input.IsKeyPressed(Keys.Space))
            _visualizer.TogglePause();
        if (input.IsKeyPressed(Keys.R))
            _visualizer.NewMaze();
        if (input.IsKeyPressed(Keys.Right))
            _visualizer.NextSolver();
        if (input.IsKeyPressed(Keys.Left))
            _visualizer.PreviousSolver();
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
