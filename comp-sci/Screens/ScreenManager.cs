using System.Collections.Generic;
using comp_sci.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace comp_sci.Screens;

public class ScreenManager {
    private readonly Dictionary<ScreenType, IScreen> _screens = new();
    private IScreen _activeScreen;

    public void Register(ScreenType type, IScreen screen) {
        _screens[type] = screen;
    }

    public void SetActive(ScreenType type) {
        _activeScreen = _screens[type];
    }

    public void Update(GameTime gameTime, InputManager input) {
        _activeScreen.Update(gameTime, input);

        if (_activeScreen.RequestedTransition is { } next) {
            SetActive(next);
        }
    }

    public void Draw(SpriteBatch spriteBatch) {
        _activeScreen.Draw(spriteBatch);
    }
}
