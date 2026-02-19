using System.Collections.Generic;

namespace comp_sci.Search;

public class BinarySearch : ISearchAlgorithm {
    public string Name => "Binary Search";

    public IEnumerable<SearchStep> Search(int[] array, int target) {
        int low = 0;
        int high = array.Length - 1;

        while (low <= high) {
            yield return SearchStep.Bounds(low, high);

            int mid = low + (high - low) / 2;
            yield return SearchStep.Check(mid);

            if (array[mid] == target) {
                yield return SearchStep.Found(mid);
                yield break;
            }

            if (array[mid] < target) {
                for (int i = low; i < mid; i++)
                    yield return SearchStep.Eliminate(i);
                low = mid + 1;
            } else {
                for (int i = mid + 1; i <= high; i++)
                    yield return SearchStep.Eliminate(i);
                high = mid - 1;
            }

            yield return SearchStep.Eliminate(mid);
        }
    }
}
