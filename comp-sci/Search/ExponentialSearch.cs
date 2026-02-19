using System;
using System.Collections.Generic;

namespace comp_sci.Search;

public class ExponentialSearch : ISearchAlgorithm {
    public string Name => "Exponential Search";

    public IEnumerable<SearchStep> Search(int[] array, int target) {
        int n = array.Length;

        if (n == 0) yield break;

        yield return SearchStep.Check(0);
        if (array[0] == target) {
            yield return SearchStep.Found(0);
            yield break;
        }

        int bound = 1;
        while (bound < n && array[bound] <= target) {
            yield return SearchStep.Check(bound);
            if (array[bound] == target) {
                yield return SearchStep.Found(bound);
                yield break;
            }
            bound *= 2;
        }

        int low = bound / 2;
        int high = Math.Min(bound, n - 1);

        for (int i = 0; i < low; i++)
            yield return SearchStep.Eliminate(i);

        if (high < n - 1) {
            for (int i = high + 1; i < n; i++)
                yield return SearchStep.Eliminate(i);
        }

        yield return SearchStep.Bounds(low, high);

        while (low <= high) {
            int mid = low + (high - low) / 2;
            yield return SearchStep.Check(mid);

            if (array[mid] == target) {
                yield return SearchStep.Found(mid);
                yield break;
            }

            if (array[mid] < target) {
                for (int i = low; i <= mid; i++)
                    yield return SearchStep.Eliminate(i);
                low = mid + 1;
            } else {
                for (int i = mid; i <= high; i++)
                    yield return SearchStep.Eliminate(i);
                high = mid - 1;
            }
        }
    }
}
