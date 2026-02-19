using System.Collections.Generic;

namespace comp_sci.Maze;

public interface IMazeSolver {
    string Name { get; }
    IEnumerable<MazeStep> Solve(MazeGrid grid);
}
