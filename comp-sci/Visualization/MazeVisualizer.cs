using System;
using System.Collections.Generic;
using comp_sci.Maze;
using Microsoft.Xna.Framework;

namespace comp_sci.Visualization;

public enum MazeState {
    Generating,
    Solving,
    Complete
}

public class MazeVisualizer {
    private readonly IMazeSolver[] _solvers;
    private readonly IMazeGenerator _generator;
    private int _currentSolverIndex;
    private MazeGrid _grid;
    private IEnumerator<MazeStep> _stepper;
    private MazeState _state;
    private bool _paused;
    private int _stepsPerFrame = 2;
    private double _completeTimer;
    private const double AutoAdvanceDelay = 2.0;
    private const int GridSize = 25;
    private readonly Random _rng = new();

    private Color[,] _cellColors;
    private List<Point> _solutionPath;
    private Point _currentCell;

    public MazeGrid Grid => _grid;
    public Color[,] CellColors => _cellColors;
    public List<Point> SolutionPath => _solutionPath;
    public Point CurrentCell => _currentCell;
    public string CurrentSolverName => _state == MazeState.Generating ? "Generating Maze..." : _solvers[_currentSolverIndex].Name;
    public MazeState State => _state;
    public bool Paused => _paused;
    public int StepsPerFrame => _stepsPerFrame;

    public MazeVisualizer() {
        _generator = new RecursiveBacktrackGenerator();
        _solvers = new IMazeSolver[] {
            new DFSSolver(),
            new BFSSolver(),
            new AStarSolver(),
            new WallFollowerSolver()
        };
        _solutionPath = new List<Point>();
        NewMaze();
    }

    public void NewMaze() {
        _grid = new MazeGrid(GridSize, GridSize);
        _cellColors = new Color[GridSize, GridSize];
        _solutionPath.Clear();
        _currentCell = Point.Zero;
        ResetCellColors();
        _state = MazeState.Generating;
        _stepper = _generator.Generate(_grid).GetEnumerator();
        _completeTimer = 0;
        _paused = false;
    }

    private void StartSolving() {
        _grid.ResetVisited();
        ResetCellColors();
        _solutionPath.Clear();
        _state = MazeState.Solving;
        _stepper = _solvers[_currentSolverIndex].Solve(_grid).GetEnumerator();
    }

    private void ResetCellColors() {
        for (int x = 0; x < GridSize; x++)
            for (int y = 0; y < GridSize; y++)
                _cellColors[x, y] = Color.Transparent;
    }

    public void TogglePause() => _paused = !_paused;

    public void SpeedUp() => _stepsPerFrame = Math.Min(_stepsPerFrame + 1, 50);
    public void SlowDown() => _stepsPerFrame = Math.Max(_stepsPerFrame - 1, 1);

    public void NextSolver() {
        _currentSolverIndex = (_currentSolverIndex + 1) % _solvers.Length;
        StartSolving();
    }

    public void PreviousSolver() {
        _currentSolverIndex = (_currentSolverIndex - 1 + _solvers.Length) % _solvers.Length;
        StartSolving();
    }

    public void Update(GameTime gameTime) {
        if (_state == MazeState.Complete) {
            _completeTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (_completeTimer >= AutoAdvanceDelay) {
                _currentSolverIndex = (_currentSolverIndex + 1) % _solvers.Length;
                StartSolving();
            }
            return;
        }

        if (_paused) return;

        for (int s = 0; s < _stepsPerFrame; s++) {
            if (!_stepper.MoveNext()) {
                if (_state == MazeState.Generating) {
                    StartSolving();
                } else {
                    _state = MazeState.Complete;
                    _completeTimer = 0;
                }
                return;
            }

            var step = _stepper.Current;
            switch (step.Type) {
                case MazeStepType.Visit:
                    _cellColors[step.Position.X, step.Position.Y] = new Color(60, 80, 140);
                    break;
                case MazeStepType.Backtrack:
                    _cellColors[step.Position.X, step.Position.Y] = new Color(140, 100, 50);
                    break;
                case MazeStepType.Current:
                    _currentCell = step.Position;
                    _cellColors[step.Position.X, step.Position.Y] = Color.Yellow;
                    break;
                case MazeStepType.Path:
                    _cellColors[step.Position.X, step.Position.Y] = new Color(50, 200, 80);
                    _solutionPath.Add(step.Position);
                    break;
            }
        }
    }
}
