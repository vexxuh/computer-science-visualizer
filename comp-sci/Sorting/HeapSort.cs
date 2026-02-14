using System.Collections.Generic;

namespace comp_sci.Sorting;

public class HeapSort : ISortAlgorithm {
    public string Name => "Heap Sort";

    public IEnumerable<SortStep> Sort(int[] array) {
        int n = array.Length;

        for (int i = n / 2 - 1; i >= 0; i--)
            foreach (var step in Heapify(array, n, i))
                yield return step;

        for (int i = n - 1; i > 0; i--) {
            (array[0], array[i]) = (array[i], array[0]);
            yield return SortStep.Swap(0, i);
            yield return SortStep.Done(i);

            foreach (var step in Heapify(array, i, 0))
                yield return step;
        }
        yield return SortStep.Done(0);
    }

    private static IEnumerable<SortStep> Heapify(int[] array, int n, int i) {
        int largest = i;
        int left = 2 * i + 1;
        int right = 2 * i + 2;

        if (left < n) {
            yield return SortStep.Compare(left, largest);
            if (array[left] > array[largest])
                largest = left;
        }

        if (right < n) {
            yield return SortStep.Compare(right, largest);
            if (array[right] > array[largest])
                largest = right;
        }

        if (largest != i) {
            (array[i], array[largest]) = (array[largest], array[i]);
            yield return SortStep.Swap(i, largest);

            foreach (var step in Heapify(array, n, largest))
                yield return step;
        }
    }
}
