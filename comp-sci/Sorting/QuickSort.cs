using System.Collections.Generic;

namespace comp_sci.Sorting;

public class QuickSort : ISortAlgorithm {
    public string Name => "Quick Sort";

    public IEnumerable<SortStep> Sort(int[] array) {
        var stack = new Stack<(int low, int high)>();
        stack.Push((0, array.Length - 1));

        while (stack.Count > 0) {
            var (low, high) = stack.Pop();
            if (low >= high) {
                if (low >= 0 && low < array.Length)
                    yield return SortStep.Done(low);
                continue;
            }

            int pivot = array[high];
            int i = low - 1;

            for (int j = low; j < high; j++) {
                yield return SortStep.Compare(j, high);
                if (array[j] <= pivot) {
                    i++;
                    if (i != j) {
                        (array[i], array[j]) = (array[j], array[i]);
                        yield return SortStep.Swap(i, j);
                    }
                }
            }

            i++;
            if (i != high) {
                (array[i], array[high]) = (array[high], array[i]);
                yield return SortStep.Swap(i, high);
            }
            yield return SortStep.Done(i);

            stack.Push((i + 1, high));
            stack.Push((low, i - 1));
        }
    }
}
