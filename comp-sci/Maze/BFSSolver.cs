using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace comp_sci.Maze;

public class BFSSolver : IMazeSolver {
    public string Name => "BFS Solver (Shortest Path)";

    public IEnumerable<MazeStep> Solve(MazeGrid grid) {
        var queue = new Queue<Point>();
        var parent = new Dictionary<Point, Point>();
        var visited = new HashSet<Point>();

        queue.Enqueue(grid.Start);
        visited.Add(grid.Start);
        yield return MazeStep.Visit(grid.Start, grid.Start);

        while (queue.Count > 0) {
            var current = queue.Dequeue();
            yield return MazeStep.SetCurrent(current);

            if (current == grid.End) {
                foreach (var step in TracePath(parent, grid.Start, grid.End))
                    yield return step;
                yield break;
            }

            foreach (var next in grid.GetPassableNeighbors(current)) {
                if (visited.Add(next)) {
                    parent[next] = current;
                    queue.Enqueue(next);
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
