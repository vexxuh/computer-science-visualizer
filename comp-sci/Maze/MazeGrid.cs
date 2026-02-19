using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace comp_sci.Maze;

public struct Cell {
    public bool WallNorth;
    public bool WallSouth;
    public bool WallEast;
    public bool WallWest;
    public bool Visited;

    public static Cell Default() => new() {
        WallNorth = true,
        WallSouth = true,
        WallEast = true,
        WallWest = true,
        Visited = false
    };
}

public class MazeGrid {
    public int Width { get; }
    public int Height { get; }
    public Cell[,] Cells { get; }
    public Point Start { get; set; }
    public Point End { get; set; }

    public MazeGrid(int width, int height) {
        Width = width;
        Height = height;
        Cells = new Cell[width, height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                Cells[x, y] = Cell.Default();
    }

    public bool IsInBounds(int x, int y) =>
        x >= 0 && x < Width && y >= 0 && y < Height;

    public void RemoveWall(Point from, Point to) {
        int dx = to.X - from.X;
        int dy = to.Y - from.Y;

        if (dx == 1) {
            Cells[from.X, from.Y].WallEast = false;
            Cells[to.X, to.Y].WallWest = false;
        } else if (dx == -1) {
            Cells[from.X, from.Y].WallWest = false;
            Cells[to.X, to.Y].WallEast = false;
        } else if (dy == 1) {
            Cells[from.X, from.Y].WallSouth = false;
            Cells[to.X, to.Y].WallNorth = false;
        } else if (dy == -1) {
            Cells[from.X, from.Y].WallNorth = false;
            Cells[to.X, to.Y].WallSouth = false;
        }
    }

    public List<Point> GetPassableNeighbors(Point pos) {
        var neighbors = new List<Point>(4);
        var cell = Cells[pos.X, pos.Y];

        if (!cell.WallNorth && pos.Y > 0)
            neighbors.Add(new Point(pos.X, pos.Y - 1));
        if (!cell.WallSouth && pos.Y < Height - 1)
            neighbors.Add(new Point(pos.X, pos.Y + 1));
        if (!cell.WallEast && pos.X < Width - 1)
            neighbors.Add(new Point(pos.X + 1, pos.Y));
        if (!cell.WallWest && pos.X > 0)
            neighbors.Add(new Point(pos.X - 1, pos.Y));

        return neighbors;
    }

    public void ResetVisited() {
        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                Cells[x, y].Visited = false;
    }
}
