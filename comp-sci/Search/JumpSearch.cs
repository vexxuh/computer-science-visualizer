using System;
using System.Collections.Generic;

namespace comp_sci.Search;

public class JumpSearch : ISearchAlgorithm {
    public string Name => "Jump Search";

    public IEnumerable<SearchStep> Search(int[] array, int target) {
        int n = array.Length;
        int step = (int)Math.Floor(Math.Sqrt(n));
        int prev = 0;

        int blockEnd = Math.Min(step, n) - 1;
        while (array[blockEnd] < target) {
            yield return SearchStep.Check(blockEnd);
            yield return SearchStep.Eliminate(blockEnd);

            for (int i = prev; i < blockEnd; i++)
                yield return SearchStep.Eliminate(i);

            prev = blockEnd + 1;
            blockEnd = Math.Min(blockEnd + step, n - 1);

            if (prev >= n)
                yield break;
        }

        yield return SearchStep.Check(blockEnd);
        yield return SearchStep.Bounds(prev, blockEnd);

        for (int i = prev; i <= blockEnd; i++) {
            yield return SearchStep.Check(i);

            if (array[i] == target) {
                yield return SearchStep.Found(i);
                yield break;
            }

            yield return SearchStep.Eliminate(i);
        }
    }
}
