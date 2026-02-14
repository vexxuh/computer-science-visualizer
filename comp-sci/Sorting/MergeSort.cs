using System.Collections.Generic;

namespace comp_sci.Sorting;

public class MergeSort : ISortAlgorithm {
    public string Name => "Merge Sort";

    public IEnumerable<SortStep> Sort(int[] array) {
        int n = array.Length;
        for (int width = 1; width < n; width *= 2) {
            for (int left = 0; left < n; left += 2 * width) {
                int mid = System.Math.Min(left + width, n);
                int right = System.Math.Min(left + 2 * width, n);

                foreach (var step in Merge(array, left, mid, right))
                    yield return step;
            }
        }
        for (int i = 0; i < n; i++)
            yield return SortStep.Done(i);
    }

    private static IEnumerable<SortStep> Merge(int[] array, int left, int mid, int right) {
        int[] temp = new int[right - left];
        int i = left, j = mid, k = 0;

        while (i < mid && j < right) {
            yield return SortStep.Compare(i, j);
            if (array[i] <= array[j])
                temp[k++] = array[i++];
            else
                temp[k++] = array[j++];
        }
        while (i < mid) temp[k++] = array[i++];
        while (j < right) temp[k++] = array[j++];

        for (int t = 0; t < temp.Length; t++) {
            array[left + t] = temp[t];
            yield return SortStep.Set(left + t, temp[t]);
        }
    }
}
