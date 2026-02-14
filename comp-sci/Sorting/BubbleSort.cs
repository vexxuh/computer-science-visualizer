using System.Collections.Generic;

namespace comp_sci.Sorting;

public class BubbleSort : ISortAlgorithm {
    public string Name => "Bubble Sort";

    public IEnumerable<SortStep> Sort(int[] array) {
        int n = array.Length;
        for (int i = 0; i < n - 1; i++) {
            for (int j = 0; j < n - 1 - i; j++) {
                yield return SortStep.Compare(j, j + 1);
                if (array[j] > array[j + 1]) {
                    (array[j], array[j + 1]) = (array[j + 1], array[j]);
                    yield return SortStep.Swap(j, j + 1);
                }
            }
            yield return SortStep.Done(n - 1 - i);
        }
        yield return SortStep.Done(0);
    }
}
