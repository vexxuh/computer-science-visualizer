using System;
using System.Collections.Generic;
using comp_sci.Search;
using Microsoft.Xna.Framework;

namespace comp_sci.Visualization;

public class SearchVisualizer {
    private readonly ISearchAlgorithm[] _algorithms;
    private int _currentAlgorithmIndex;
    private int[] _array;
    private Color[] _barColors;
    private float[] _eliminateProgress;
    private IEnumerator<SearchStep> _stepper;
    private bool _paused;
    private bool _completed;
    private int _speed = 1;
    private double _stepTimer;
    private double _completedTimer;
    private const double BaseStepDelay = 0.45;
    private const double AutoAdvanceDelay = 3.0;
    private const int ElementCount = 32;
    private readonly Random _rng = new();

    private int _target;
    private int _foundIndex = -1;
    private int _lowBound = -1;
    private int _highBound = -1;

    public int[] Array => _array;
    public Color[] BarColors => _barColors;
    public float[] EliminateProgress => _eliminateProgress;
    public string CurrentAlgorithmName => _algorithms[_currentAlgorithmIndex].Name;
    public bool Paused => _paused;
    public int Speed => _speed;
    public int Target => _target;
    public int FoundIndex => _foundIndex;
    public int LowBound => _lowBound;
    public int HighBound => _highBound;
    public bool Completed => _completed;

    public SearchVisualizer() {
        _algorithms = new ISearchAlgorithm[] {
            new LinearSearch(),
            new BinarySearch(),
            new JumpSearch(),
            new ExponentialSearch(),
            new InterpolationSearch(),
            new SqrtSearch()
        };

        _array = new int[ElementCount];
        _barColors = new Color[ElementCount];
        _eliminateProgress = new float[ElementCount];
        Restart();
    }

    public void Restart() {
        for (int i = 0; i < ElementCount; i++)
            _array[i] = (i + 1) * 3;

        _target = _array[_rng.Next(ElementCount)];
        _foundIndex = -1;
        _lowBound = -1;
        _highBound = -1;
        ResetColors();
        _stepper = _algorithms[_currentAlgorithmIndex].Search(_array, _target).GetEnumerator();
        _completed = false;
        _completedTimer = 0;
        _stepTimer = 0;
    }

    private void ResetColors() {
        for (int i = 0; i < _barColors.Length; i++) {
            _barColors[i] = Color.White;
            _eliminateProgress[i] = 0f;
        }
    }

    public void TogglePause() => _paused = !_paused;

    public void SpeedUp() => _speed = Math.Min(_speed + 1, 10);
    public void SlowDown() => _speed = Math.Max(_speed - 1, 1);

    public void NextAlgorithm() {
        _currentAlgorithmIndex = (_currentAlgorithmIndex + 1) % _algorithms.Length;
        Restart();
    }

    public void PreviousAlgorithm() {
        _currentAlgorithmIndex = (_currentAlgorithmIndex - 1 + _algorithms.Length) % _algorithms.Length;
        Restart();
    }

    public void Update(GameTime gameTime) {
        double elapsed = gameTime.ElapsedGameTime.TotalSeconds;

        AnimateEliminations(elapsed);

        if (_completed) {
            _completedTimer += elapsed;
            if (_completedTimer >= AutoAdvanceDelay)
                NextAlgorithm();
            return;
        }

        if (_paused) return;

        _stepTimer += elapsed;
        double stepDelay = BaseStepDelay / _speed;

        while (_stepTimer >= stepDelay) {
            _stepTimer -= stepDelay;

            if (!_stepper.MoveNext()) {
                _completed = true;
                _completedTimer = 0;
                return;
            }

            var step = _stepper.Current;
            switch (step.Type) {
                case SearchStepType.Check:
                    _barColors[step.Index] = Color.Yellow;
                    break;
                case SearchStepType.Eliminate:
                    _barColors[step.Index] = new Color(120, 40, 40);
                    _eliminateProgress[step.Index] = 0.01f;
                    break;
                case SearchStepType.Found:
                    _foundIndex = step.Index;
                    _barColors[step.Index] = Color.Green;
                    _completed = true;
                    _completedTimer = 0;
                    break;
                case SearchStepType.BoundsUpdate:
                    _lowBound = step.Index;
                    _highBound = step.SecondIndex;
                    break;
            }
        }
    }

    private void AnimateEliminations(double elapsed) {
        float animSpeed = 3.0f;
        for (int i = 0; i < ElementCount; i++) {
            if (_eliminateProgress[i] > 0f && _eliminateProgress[i] < 1f) {
                _eliminateProgress[i] = Math.Min(1f, _eliminateProgress[i] + (float)(elapsed * animSpeed));
            }
        }
    }
}
