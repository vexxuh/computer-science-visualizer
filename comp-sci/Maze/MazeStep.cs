using Microsoft.Xna.Framework;

namespace comp_sci.Maze;

public enum MazeStepType {
    Visit,
    Backtrack,
    Path,
    Current
}

public readonly struct MazeStep {
    public MazeStepType Type { get; }
    public Point Position { get; }
    public Point FromPosition { get; }

    private MazeStep(MazeStepType type, Point position, Point fromPosition) {
        Type = type;
        Position = position;
        FromPosition = fromPosition;
    }

    public static MazeStep Visit(Point pos, Point from) => new(MazeStepType.Visit, pos, from);
    public static MazeStep Backtrack(Point pos) => new(MazeStepType.Backtrack, pos, pos);
    public static MazeStep PathCell(Point pos) => new(MazeStepType.Path, pos, pos);
    public static MazeStep SetCurrent(Point pos) => new(MazeStepType.Current, pos, pos);
}
