using SortVisualizer.Models;

namespace SortVisualizer.Sorting
{
    public class BubbleSort : ISortingAlgorithm
    {
        public string Name => "Пузырьковая";

        public List<SortStep> Sort(int[] array)
        {
            List<SortStep> steps = new();

            int[] arr = (int[])array.Clone();

            for (int i = 0; i < arr.Length - 1; i++)
            {
                for (int j = 0; j < arr.Length - i - 1; j++)
                {
                    steps.Add(new SortStep
                    {
                        Array = (int[])arr.Clone(),
                        FirstIndex = j,
                        SecondIndex = j + 1,
                        IsSwap = false
                    });

                    if (arr[j] > arr[j + 1])
                    {
                        (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);

                        steps.Add(new SortStep
                        {
                            Array = (int[])arr.Clone(),
                            FirstIndex = j,
                            SecondIndex = j + 1,
                            IsSwap = true
                        });
                    }
                }
            }

            return steps;
        }
    }
}