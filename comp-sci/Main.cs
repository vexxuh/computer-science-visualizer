using comp_sci.Visualization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace comp_sci;

public class Main : Game {
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SortVisualizer _visualizer;
    private BarRenderer _renderer;
    private KeyboardState _prevKeyState;

    public Main() {
        _graphics = new GraphicsDeviceManager(this) {
            PreferredBackBufferWidth = 1280,
            PreferredBackBufferHeight = 720
        };
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize() {
        _visualizer = new SortVisualizer();
        base.Initialize();
    }

    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        var font = Content.Load<SpriteFont>("DefaultFont");
        _renderer = new BarRenderer(GraphicsDevice, font, 1280, 720);
    }

    protected override void Update(GameTime gameTime) {
        var keyState = Keyboard.GetState();

        if (keyState.IsKeyDown(Keys.Escape))
            Exit();

        if (IsKeyPressed(keyState, Keys.Space))
            _visualizer.TogglePause();

        if (IsKeyPressed(keyState, Keys.R))
            _visualizer.Restart();

        if (IsKeyPressed(keyState, Keys.Right))
            _visualizer.NextAlgorithm();

        if (IsKeyPressed(keyState, Keys.Left))
            _visualizer.PreviousAlgorithm();

        if (IsKeyPressed(keyState, Keys.Up))
            _visualizer.SpeedUp();

        if (IsKeyPressed(keyState, Keys.Down))
            _visualizer.SlowDown();

        _prevKeyState = keyState;
        _visualizer.Update(gameTime);

        base.Update(gameTime);
    }

    private bool IsKeyPressed(KeyboardState current, Keys key) =>
        current.IsKeyDown(key) && _prevKeyState.IsKeyUp(key);

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(new Color(30, 30, 30));

        _spriteBatch.Begin();
        _renderer.Draw(_spriteBatch, _visualizer.Array, _visualizer.BarColors,
            _visualizer.CurrentAlgorithmName, _visualizer.Paused, _visualizer.StepsPerFrame);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
