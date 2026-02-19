using comp_sci.Screens;
using comp_sci.Utilities;
using comp_sci.Visualization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace comp_sci;

public class Main : Game {
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private InputManager _input;
    private ScreenManager _screenManager;

    public Main() {
        _graphics = new GraphicsDeviceManager(this) {
            PreferredBackBufferWidth = 1280,
            PreferredBackBufferHeight = 720
        };
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize() {
        _input = new InputManager();
        _screenManager = new ScreenManager();
        base.Initialize();
    }

    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        var font = Content.Load<SpriteFont>("DefaultFont");
        var primitives = new PrimitiveRenderer(GraphicsDevice);

        // Sorting screen
        var sortVisualizer = new SortVisualizer();
        var barRenderer = new BarRenderer(primitives, font, 1280, 720);
        var sortingScreen = new SortingScreen(sortVisualizer, barRenderer);

        // Maze screen
        var mazeVisualizer = new MazeVisualizer();
        var mazeRenderer = new MazeRenderer(primitives, font, 1280, 720);
        var mazeScreen = new MazeScreen(mazeVisualizer, mazeRenderer);

        // Search screen
        var searchVisualizer = new SearchVisualizer();
        var searchRenderer = new SearchRenderer(primitives, font, 1280, 720);
        var searchScreen = new SearchScreen(searchVisualizer, searchRenderer);

        // Menu screen
        var menuScreen = new MenuScreen(font, primitives, 1280, 720);

        _screenManager.Register(ScreenType.MainMenu, menuScreen);
        _screenManager.Register(ScreenType.SortingVisualizer, sortingScreen);
        _screenManager.Register(ScreenType.SearchVisualizer, searchScreen);
        _screenManager.Register(ScreenType.MazeVisualizer, mazeScreen);
        _screenManager.SetActive(ScreenType.MainMenu);
    }

    protected override void Update(GameTime gameTime) {
        _input.Update();

        if (_input.IsKeyPressed(Keys.Escape))
            Exit();

        _screenManager.Update(gameTime, _input);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(new Color(30, 30, 30));

        _spriteBatch.Begin();
        _screenManager.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
