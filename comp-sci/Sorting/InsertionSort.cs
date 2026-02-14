using System.Collections.Generic;

namespace comp_sci.Sorting;

public class InsertionSort : ISortAlgorithm {
    public string Name => "Insertion Sort";

    public IEnumerable<SortStep> Sort(int[] array) {
        int n = array.Length;
        yield return SortStep.Done(0);
        for (int i = 1; i < n; i++) {
            int key = array[i];
            int j = i - 1;
            yield return SortStep.Compare(i, j);
            while (j >= 0 && array[j] > key) {
                array[j + 1] = array[j];
                yield return SortStep.Set(j + 1, array[j + 1]);
                j--;
                if (j >= 0)
                    yield return SortStep.Compare(j + 1, j);
            }
            array[j + 1] = key;
            yield return SortStep.Set(j + 1, key);
            yield return SortStep.Done(j + 1);
        }
        for (int i = 0; i < n; i++)
            yield return SortStep.Done(i);
    }
}
