namespace comp_sci.Search;

public enum SearchStepType {
    Check,
    Eliminate,
    Found,
    BoundsUpdate
}

public readonly struct SearchStep {
    public SearchStepType Type { get; }
    public int Index { get; }
    public int SecondIndex { get; }

    private SearchStep(SearchStepType type, int index, int secondIndex) {
        Type = type;
        Index = index;
        SecondIndex = secondIndex;
    }

    public static SearchStep Check(int index) => new(SearchStepType.Check, index, -1);
    public static SearchStep Eliminate(int index) => new(SearchStepType.Eliminate, index, -1);
    public static SearchStep Found(int index) => new(SearchStepType.Found, index, -1);
    public static SearchStep Bounds(int low, int high) => new(SearchStepType.BoundsUpdate, low, high);
}
