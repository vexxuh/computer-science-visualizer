using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace comp_sci.Maze;

public class DFSSolver : IMazeSolver {
    public string Name => "DFS Solver";

    public IEnumerable<MazeStep> Solve(MazeGrid grid) {
        var stack = new Stack<Point>();
        var parent = new Dictionary<Point, Point>();
        var visited = new HashSet<Point>();

        stack.Push(grid.Start);
        visited.Add(grid.Start);
        yield return MazeStep.Visit(grid.Start, grid.Start);

        while (stack.Count > 0) {
            var current = stack.Pop();
            yield return MazeStep.SetCurrent(current);

            if (current == grid.End) {
                foreach (var step in TracePath(parent, grid.Start, grid.End))
                    yield return step;
                yield break;
            }

            var neighbors = grid.GetPassableNeighbors(current);
            foreach (var next in neighbors) {
                if (visited.Add(next)) {
                    parent[next] = current;
                    stack.Push(next);
                    yield return MazeStep.Visit(next, current);
                }
            }
        }
    }

    private static IEnumerable<MazeStep> TracePath(Dictionary<Point, Point> parent, Point start, Point end) {
        var path = new List<Point>();
        var current = end;
        while (current != start) {
            path.Add(current);
            current = parent[current];
        }
        path.Add(start);
        path.Reverse();
        foreach (var p in path)
            yield return MazeStep.PathCell(p);
    }
}
