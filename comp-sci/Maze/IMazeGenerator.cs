using System.Collections.Generic;

namespace comp_sci.Maze;

public interface IMazeGenerator {
    IEnumerable<MazeStep> Generate(MazeGrid grid);
}
