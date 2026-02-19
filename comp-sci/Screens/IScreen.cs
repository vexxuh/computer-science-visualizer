using comp_sci.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace comp_sci.Screens;

public enum ScreenType {
    MainMenu,
    SortingVisualizer,
    MazeVisualizer,
    SearchVisualizer
}

public interface IScreen {
    void Update(GameTime gameTime, InputManager input);
    void Draw(SpriteBatch spriteBatch);
    ScreenType? RequestedTransition { get; }
}
