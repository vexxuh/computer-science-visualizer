using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace comp_sci.Maze;

public class WallFollowerSolver : IMazeSolver {
    public string Name => "Wall Follower (Right Hand)";

    // Directions: 0=North, 1=East, 2=South, 3=West
    private static readonly (int dx, int dy)[] DirVectors = { (0, -1), (1, 0), (0, 1), (-1, 0) };

    public IEnumerable<MazeStep> Solve(MazeGrid grid) {
        var current = grid.Start;
        int facing = 1; // Start facing East
        var visited = new HashSet<Point>();
        var path = new List<Point> { current };

        yield return MazeStep.Visit(current, current);

        int maxSteps = grid.Width * grid.Height * 4;
        int steps = 0;

        while (current != grid.End && steps < maxSteps) {
            steps++;
            // Right-hand rule: try right, forward, left, back
            bool moved = false;
            for (int turn = 0; turn < 4; turn++) {
                int tryDir = (facing + 1 - turn + 4) % 4; // right, forward, left, back
                if (CanMove(grid, current, tryDir)) {
                    var (dx, dy) = DirVectors[tryDir];
                    var next = new Point(current.X + dx, current.Y + dy);
                    facing = tryDir;
                    current = next;
                    moved = true;

                    if (visited.Add(current))
                        yield return MazeStep.Visit(current, path[^1]);
                    else
                        yield return MazeStep.Backtrack(current);

                    yield return MazeStep.SetCurrent(current);
                    path.Add(current);
                    break;
                }
            }

            if (!moved) break;
        }

        // Trace the actual shortest path from the path taken (remove loops)
        var cleanPath = RemoveLoops(path);
        foreach (var p in cleanPath)
            yield return MazeStep.PathCell(p);
    }

    private static bool CanMove(MazeGrid grid, Point pos, int dir) {
        var cell = grid.Cells[pos.X, pos.Y];
        return dir switch {
            0 => !cell.WallNorth && pos.Y > 0,
            1 => !cell.WallEast && pos.X < grid.Width - 1,
            2 => !cell.WallSouth && pos.Y < grid.Height - 1,
            3 => !cell.WallWest && pos.X > 0,
            _ => false
        };
    }

    private static List<Point> RemoveLoops(List<Point> path) {
        // Keep last occurrence of each point to remove loops
        var lastIndex = new Dictionary<Point, int>();
        for (int i = 0; i < path.Count; i++)
            lastIndex[path[i]] = i;

        var clean = new List<Point>();
        int idx = 0;
        while (idx < path.Count) {
            clean.Add(path[idx]);
            if (lastIndex[path[idx]] > idx)
                idx = lastIndex[path[idx]];
            else
                idx++;
        }
        return clean;
    }
}
