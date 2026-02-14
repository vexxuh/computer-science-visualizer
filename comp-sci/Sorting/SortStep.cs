namespace comp_sci.Sorting;

public enum SortStepType {
    Compare,
    Swap,
    Set,
    Done
}

public readonly struct SortStep {
    public SortStepType Type { get; }
    public int IndexA { get; }
    public int IndexB { get; }
    public int Value { get; }

    private SortStep(SortStepType type, int indexA, int indexB, int value) {
        Type = type;
        IndexA = indexA;
        IndexB = indexB;
        Value = value;
    }

    public static SortStep Compare(int a, int b) => new(SortStepType.Compare, a, b, 0);
    public static SortStep Swap(int a, int b) => new(SortStepType.Swap, a, b, 0);
    public static SortStep Set(int index, int value) => new(SortStepType.Set, index, -1, value);
    public static SortStep Done(int index) => new(SortStepType.Done, index, -1, 0);
}
