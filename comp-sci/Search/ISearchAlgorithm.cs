using System.Collections.Generic;

namespace comp_sci.Search;

public interface ISearchAlgorithm {
    string Name { get; }
    IEnumerable<SearchStep> Search(int[] array, int target);
}
