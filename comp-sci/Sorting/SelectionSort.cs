using System.Collections.Generic;

namespace comp_sci.Sorting;

public class SelectionSort : ISortAlgorithm {
    public string Name => "Selection Sort";

    public IEnumerable<SortStep> Sort(int[] array) {
        int n = array.Length;
        for (int i = 0; i < n - 1; i++) {
            int minIdx = i;
            for (int j = i + 1; j < n; j++) {
                yield return SortStep.Compare(minIdx, j);
                if (array[j] < array[minIdx])
                    minIdx = j;
            }
            if (minIdx != i) {
                (array[i], array[minIdx]) = (array[minIdx], array[i]);
                yield return SortStep.Swap(i, minIdx);
            }
            yield return SortStep.Done(i);
        }
        yield return SortStep.Done(n - 1);
    }
}
