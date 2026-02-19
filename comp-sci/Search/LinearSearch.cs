using System.Collections.Generic;

namespace comp_sci.Search;

public class LinearSearch : ISearchAlgorithm {
    public string Name => "Linear Search";

    public IEnumerable<SearchStep> Search(int[] array, int target) {
        for (int i = 0; i < array.Length; i++) {
            yield return SearchStep.Check(i);

            if (array[i] == target) {
                yield return SearchStep.Found(i);
                yield break;
            }

            yield return SearchStep.Eliminate(i);
        }
    }
}
