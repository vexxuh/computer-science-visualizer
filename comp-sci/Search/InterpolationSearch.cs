using System.Collections.Generic;

namespace comp_sci.Search;

public class InterpolationSearch : ISearchAlgorithm {
    public string Name => "Interpolation Search";

    public IEnumerable<SearchStep> Search(int[] array, int target) {
        int low = 0;
        int high = array.Length - 1;

        while (low <= high && target >= array[low] && target <= array[high]) {
            yield return SearchStep.Bounds(low, high);

            if (low == high) {
                yield return SearchStep.Check(low);
                if (array[low] == target)
                    yield return SearchStep.Found(low);
                else
                    yield return SearchStep.Eliminate(low);
                yield break;
            }

            int range = array[high] - array[low];
            int pos = range == 0
                ? low
                : low + (target - array[low]) * (high - low) / range;

            yield return SearchStep.Check(pos);

            if (array[pos] == target) {
                yield return SearchStep.Found(pos);
                yield break;
            }

            if (array[pos] < target) {
                for (int i = low; i <= pos; i++)
                    yield return SearchStep.Eliminate(i);
                low = pos + 1;
            } else {
                for (int i = pos; i <= high; i++)
                    yield return SearchStep.Eliminate(i);
                high = pos - 1;
            }
        }
    }
}
