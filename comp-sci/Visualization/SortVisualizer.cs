using System;
using System.Collections.Generic;
using comp_sci.Sorting;
using Microsoft.Xna.Framework;

namespace comp_sci.Visualization;

public class SortVisualizer {
    private readonly ISortAlgorithm[] _algorithms;
    private int _currentAlgorithmIndex;
    private int[] _array;
    private Color[] _barColors;
    private IEnumerator<SortStep> _stepper;
    private bool _paused;
    private bool _completed;
    private int _stepsPerFrame = 5;
    private double _completedTimer;
    private const double AutoAdvanceDelay = 2.0;
    private const int BarCount = 100;
    private readonly Random _rng = new();

    public int[] Array => _array;
    public Color[] BarColors => _barColors;
    public string CurrentAlgorithmName => _algorithms[_currentAlgorithmIndex].Name;
    public bool Paused => _paused;
    public int StepsPerFrame => _stepsPerFrame;

    public SortVisualizer() {
        _algorithms = new ISortAlgorithm[] {
            new BubbleSort(),
            new SelectionSort(),
            new InsertionSort(),
            new QuickSort(),
            new MergeSort(),
            new HeapSort(),
            new ShellSort()
        };

        _array = new int[BarCount];
        _barColors = new Color[BarCount];
        Restart();
    }

    public void Restart() {
        for (int i = 0; i < BarCount; i++)
            _array[i] = i + 1;
        Shuffle();
        ResetColors();
        _stepper = _algorithms[_currentAlgorithmIndex].Sort(_array).GetEnumerator();
        _completed = false;
        _completedTimer = 0;
    }

    private void Shuffle() {
        for (int i = _array.Length - 1; i > 0; i--) {
            int j = _rng.Next(i + 1);
            (_array[i], _array[j]) = (_array[j], _array[i]);
        }
    }

    private void ResetColors() {
        for (int i = 0; i < _barColors.Length; i++)
            _barColors[i] = Color.White;
    }

    public void TogglePause() => _paused = !_paused;

    public void SpeedUp() => _stepsPerFrame = Math.Min(_stepsPerFrame + 1, 50);
    public void SlowDown() => _stepsPerFrame = Math.Max(_stepsPerFrame - 1, 1);

    public void NextAlgorithm() {
        _currentAlgorithmIndex = (_currentAlgorithmIndex + 1) % _algorithms.Length;
        Restart();
    }

    public void PreviousAlgorithm() {
        _currentAlgorithmIndex = (_currentAlgorithmIndex - 1 + _algorithms.Length) % _algorithms.Length;
        Restart();
    }

    public void Update(GameTime gameTime) {
        if (_completed) {
            _completedTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (_completedTimer >= AutoAdvanceDelay)
                NextAlgorithm();
            return;
        }

        if (_paused) return;

        ResetHighlights();

        for (int s = 0; s < _stepsPerFrame; s++) {
            if (!_stepper.MoveNext()) {
                _completed = true;
                for (int i = 0; i < _barColors.Length; i++)
                    _barColors[i] = Color.Green;
                return;
            }

            var step = _stepper.Current;
            switch (step.Type) {
                case SortStepType.Compare:
                    _barColors[step.IndexA] = Color.Yellow;
                    _barColors[step.IndexB] = Color.Yellow;
                    break;
                case SortStepType.Swap:
                    _barColors[step.IndexA] = Color.Red;
                    _barColors[step.IndexB] = Color.Red;
                    break;
                case SortStepType.Set:
                    _barColors[step.IndexA] = Color.Red;
                    break;
                case SortStepType.Done:
                    _barColors[step.IndexA] = Color.Green;
                    break;
            }
        }
    }

    private void ResetHighlights() {
        for (int i = 0; i < _barColors.Length; i++) {
            if (_barColors[i] == Color.Yellow || _barColors[i] == Color.Red)
                _barColors[i] = Color.White;
        }
    }
}
