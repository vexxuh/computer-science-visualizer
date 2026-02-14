using System.Collections.Generic;

namespace comp_sci.Sorting;

public class ShellSort : ISortAlgorithm {
    public string Name => "Shell Sort";

    public IEnumerable<SortStep> Sort(int[] array) {
        int n = array.Length;

        for (int gap = n / 2; gap > 0; gap /= 2) {
            for (int i = gap; i < n; i++) {
                int temp = array[i];
                int j = i;

                yield return SortStep.Compare(j, j - gap);
                while (j >= gap && array[j - gap] > temp) {
                    array[j] = array[j - gap];
                    yield return SortStep.Set(j, array[j]);
                    j -= gap;
                    if (j >= gap)
                        yield return SortStep.Compare(j, j - gap);
                }
                array[j] = temp;
                yield return SortStep.Set(j, temp);
            }
        }
        for (int i = 0; i < n; i++)
            yield return SortStep.Done(i);
    }
}
