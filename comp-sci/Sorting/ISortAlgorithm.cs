using System.Collections.Generic;

namespace comp_sci.Sorting;

public interface ISortAlgorithm {
    string Name { get; }
    IEnumerable<SortStep> Sort(int[] array);
}
