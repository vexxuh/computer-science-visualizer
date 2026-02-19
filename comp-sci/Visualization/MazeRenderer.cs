using comp_sci.Maze;
using comp_sci.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace comp_sci.Visualization;

public class MazeRenderer {
    private readonly PrimitiveRenderer _primitives;
    private readonly SpriteFont _font;
    private readonly int _screenWidth;
    private readonly int _screenHeight;
    private const int CellSize = 26;
    private const int WallThickness = 2;

    public MazeRenderer(PrimitiveRenderer primitives, SpriteFont font, int screenWidth, int screenHeight) {
        _primitives = primitives;
        _font = font;
        _screenWidth = screenWidth;
        _screenHeight = screenHeight;
    }

    public void Draw(SpriteBatch spriteBatch, MazeVisualizer visualizer) {
        var grid = visualizer.Grid;
        int gridPixelW = grid.Width * CellSize;
        int gridPixelH = grid.Height * CellSize;
        int offsetX = (_screenWidth - gridPixelW) / 2;
        int offsetY = (_screenHeight - gridPixelH) / 2 + 20;

        // Draw cell fills
        for (int x = 0; x < grid.Width; x++) {
            for (int y = 0; y < grid.Height; y++) {
                var color = visualizer.CellColors[x, y];
                if (color != Color.Transparent) {
                    var rect = new Rectangle(
                        offsetX + x * CellSize + WallThickness,
                        offsetY + y * CellSize + WallThickness,
                        CellSize - WallThickness * 2,
                        CellSize - WallThickness * 2);
                    _primitives.FillRect(spriteBatch, rect, color);
                }
            }
        }

        // Draw start and end markers
        if (visualizer.State != MazeState.Generating) {
            DrawMarker(spriteBatch, grid.Start, offsetX, offsetY, Color.Lime);
            DrawMarker(spriteBatch, grid.End, offsetX, offsetY, Color.Red);
        }

        // Draw solution path line
        var path = visualizer.SolutionPath;
        if (path.Count > 1) {
            for (int i = 0; i < path.Count - 1; i++) {
                var from = CellCenter(path[i], offsetX, offsetY);
                var to = CellCenter(path[i + 1], offsetX, offsetY);
                _primitives.DrawLine(spriteBatch, from, to, new Color(80, 255, 120), 3);
            }
        }

        // Draw walls
        var wallColor = new Color(80, 80, 80);
        for (int x = 0; x < grid.Width; x++) {
            for (int y = 0; y < grid.Height; y++) {
                int cx = offsetX + x * CellSize;
                int cy = offsetY + y * CellSize;
                var cell = grid.Cells[x, y];

                if (cell.WallNorth)
                    _primitives.FillRect(spriteBatch, new Rectangle(cx, cy, CellSize, WallThickness), wallColor);
                if (cell.WallWest)
                    _primitives.FillRect(spriteBatch, new Rectangle(cx, cy, WallThickness, CellSize), wallColor);
                if (x == grid.Width - 1 && cell.WallEast)
                    _primitives.FillRect(spriteBatch, new Rectangle(cx + CellSize - WallThickness, cy, WallThickness, CellSize), wallColor);
                if (y == grid.Height - 1 && cell.WallSouth)
                    _primitives.FillRect(spriteBatch, new Rectangle(cx, cy + CellSize - WallThickness, CellSize, WallThickness), wallColor);
            }
        }

        // Draw UI text
        string info = $"{visualizer.CurrentSolverName}   Speed: {visualizer.StepsPerFrame}x   {(visualizer.Paused ? "[PAUSED]" : "")}";
        spriteBatch.DrawString(_font, info, new Vector2(10, 10), Color.White);

        string controls = "Space:Pause  Arrows:Solver/Speed  R:New Maze  Backspace:Menu";
        spriteBatch.DrawString(_font, controls, new Vector2(10, 35), Color.Gray);
    }

    private void DrawMarker(SpriteBatch spriteBatch, Point pos, int offsetX, int offsetY, Color color) {
        var rect = new Rectangle(
            offsetX + pos.X * CellSize + 4,
            offsetY + pos.Y * CellSize + 4,
            CellSize - 8,
            CellSize - 8);
        _primitives.FillRect(spriteBatch, rect, color);
    }

    private static Vector2 CellCenter(Point cell, int offsetX, int offsetY) {
        return new Vector2(
            offsetX + cell.X * CellSize + CellSize / 2f,
            offsetY + cell.Y * CellSize + CellSize / 2f);
    }
}
