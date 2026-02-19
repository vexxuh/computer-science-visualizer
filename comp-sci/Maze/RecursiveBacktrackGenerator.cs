using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace comp_sci.Maze;

public class RecursiveBacktrackGenerator : IMazeGenerator {
    private static readonly (int dx, int dy)[] Directions = { (0, -1), (0, 1), (1, 0), (-1, 0) };
    private readonly Random _rng = new();

    public IEnumerable<MazeStep> Generate(MazeGrid grid) {
        var stack = new Stack<Point>();
        var start = new Point(0, 0);
        grid.Cells[0, 0].Visited = true;
        stack.Push(start);
        yield return MazeStep.Visit(start, start);

        while (stack.Count > 0) {
            var current = stack.Peek();
            var unvisited = GetUnvisitedNeighbors(grid, current);

            if (unvisited.Count > 0) {
                var next = unvisited[_rng.Next(unvisited.Count)];
                grid.RemoveWall(current, next);
                grid.Cells[next.X, next.Y].Visited = true;
                stack.Push(next);
                yield return MazeStep.Visit(next, current);
            } else {
                stack.Pop();
                if (stack.Count > 0)
                    yield return MazeStep.Backtrack(current);
            }
        }

        // Pick random start on left edge, end on right edge
        grid.Start = new Point(0, _rng.Next(grid.Height));
        grid.End = new Point(grid.Width - 1, _rng.Next(grid.Height));
        grid.ResetVisited();
    }

    private List<Point> GetUnvisitedNeighbors(MazeGrid grid, Point pos) {
        var result = new List<Point>(4);
        foreach (var (dx, dy) in Directions) {
            int nx = pos.X + dx, ny = pos.Y + dy;
            if (grid.IsInBounds(nx, ny) && !grid.Cells[nx, ny].Visited)
                result.Add(new Point(nx, ny));
        }
        return result;
    }
}
