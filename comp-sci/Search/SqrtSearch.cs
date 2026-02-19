using System;
using System.Collections.Generic;

namespace comp_sci.Search;

public class SqrtSearch : ISearchAlgorithm {
    public string Name => "Square Root Search";

    public IEnumerable<SearchStep> Search(int[] array, int target) {
        int n = array.Length;
        int block = (int)Math.Ceiling(Math.Sqrt(n));

        int blockStart = 0;
        while (blockStart < n) {
            int blockEnd = Math.Min(blockStart + block - 1, n - 1);
            yield return SearchStep.Check(blockEnd);

            if (array[blockEnd] >= target) {
                yield return SearchStep.Bounds(blockStart, blockEnd);

                for (int i = blockEnd + 1; i < n; i++)
                    yield return SearchStep.Eliminate(i);

                for (int i = 0; i < blockStart; i++)
                    yield return SearchStep.Eliminate(i);

                for (int i = blockStart; i <= blockEnd; i++) {
                    yield return SearchStep.Check(i);

                    if (array[i] == target) {
                        yield return SearchStep.Found(i);
                        yield break;
                    }

                    if (array[i] > target) {
                        yield return SearchStep.Eliminate(i);
                        yield break;
                    }

                    yield return SearchStep.Eliminate(i);
                }

                yield break;
            }

            for (int i = blockStart; i <= blockEnd; i++)
                yield return SearchStep.Eliminate(i);

            blockStart += block;
        }
    }
}
