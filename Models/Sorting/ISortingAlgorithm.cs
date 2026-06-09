using SortVisualizer.Models;

namespace SortVisualizer.Sorting
{
    public interface ISortingAlgorithm
    {
        string Name { get; }

        List<SortStep> Sort(int[] array);
    }
}