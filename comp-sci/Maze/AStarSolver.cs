using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace comp_sci.Maze;

public class AStarSolver : IMazeSolver {
    public string Name => "A* Solver";

    public IEnumerable<MazeStep> Solve(MazeGrid grid) {
        var openSet = new PriorityQueue<Point, (int f, int id)>();
        var gScore = new Dictionary<Point, int>();
        var parent = new Dictionary<Point, Point>();
        var closed = new HashSet<Point>();
        int idCounter = 0;

        gScore[grid.Start] = 0;
        int h = Heuristic(grid.Start, grid.End);
        openSet.Enqueue(grid.Start, (h, idCounter++));
        yield return MazeStep.Visit(grid.Start, grid.Start);

        while (openSet.Count > 0) {
            var current = openSet.Dequeue();

            if (!closed.Add(current))
                continue;

            int g = gScore[current];
            yield return MazeStep.SetCurrent(current);

            if (current == grid.End) {
                foreach (var step in TracePath(parent, grid.Start, grid.End))
                    yield return step;
                yield break;
            }

            foreach (var next in grid.GetPassableNeighbors(current)) {
                if (closed.Contains(next))
                    continue;

                int tentativeG = g + 1;
                if (!gScore.ContainsKey(next) || tentativeG < gScore[next]) {
                    gScore[next] = tentativeG;
                    parent[next] = current;
                    int f = tentativeG + Heuristic(next, grid.End);
                    openSet.Enqueue(next, (f, idCounter++));
                    yield return MazeStep.Visit(next, current);
                }
            }
        }
    }

    private static int Heuristic(Point a, Point b) =>
        Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

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
