namespace SortVisualizer.Models
{
    public class SortStep
    {
        public int[] Array { get; set; } = [];

        public int FirstIndex { get; set; }

        public int SecondIndex { get; set; }

        public bool IsSwap { get; set; }

        public SortStep Clone()
        {
            return new SortStep
            {
                Array = (int[])Array.Clone(),
                FirstIndex = FirstIndex,
                SecondIndex = SecondIndex,
                IsSwap = IsSwap
            };
        }
    }
}