using Microsoft.Xna.Framework.Input;

namespace comp_sci.Utilities;

public class InputManager {
    private KeyboardState _previousState;
    private KeyboardState _currentState;

    public void Update() {
        _previousState = _currentState;
        _currentState = Keyboard.GetState();
    }

    public bool IsKeyPressed(Keys key) =>
        _currentState.IsKeyDown(key) && _previousState.IsKeyUp(key);

    public bool IsKeyDown(Keys key) => _currentState.IsKeyDown(key);
}
