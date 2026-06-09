using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RGZ_TIMP.Views
{
    public partial class MainWindow : Window
    {
        private readonly Random random = new();

        private int[] currentArray = Array.Empty<int>();

        private bool isPaused = false;

        private int comparisons = 0;

        private int swaps = 0;

        private readonly Stopwatch stopwatch = new();

        public MainWindow()
        {
            InitializeComponent();

            GenerateButton.Click += GenerateButton_Click;

            StartButton.Click += StartButton_Click;

            PauseButton.Click += PauseButton_Click;

            ResetButton.Click += ResetButton_Click;

            LoadButton.Click += LoadButton_Click;

            SaveButton.Click += SaveButton_Click;

            HelpButton.Click += HelpButton_Click;

            AlgorithmComboBox.SelectedIndex = 0;
        }

        private void GenerateButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            List<int> numbers = new();

            for (int i = 0; i < 20; i++)
            {
                numbers.Add(random.Next(10, 100));
            }

            currentArray = numbers.ToArray();

            ArrayTextBox.Text =
                string.Join(" ", currentArray);

            DrawArray(currentArray, -1, -1);
        }

        private async void StartButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                currentArray = ArrayTextBox.Text
                    .Split(' ',
                    StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();

                comparisons = 0;

                swaps = 0;

                stopwatch.Restart();

                string algorithm =
                    (AlgorithmComboBox.SelectedItem
                    as ComboBoxItem)?.Content?.ToString()
                    ?? "";

                switch (algorithm)
                {
                    case "Пузырьковая":
                        await BubbleSort(currentArray);
                        break;

                    case "Выбором":
                        await SelectionSort(currentArray);
                        break;

                    case "Вставками":
                        await InsertionSort(currentArray);
                        break;

                    case "Быстрая":
                        await QuickSort(
                            currentArray,
                            0,
                            currentArray.Length - 1);
                        break;
                }

                stopwatch.Stop();

                DrawArray(currentArray, -1, -1);

                UpdateStats();

                MessageBox.Show(
                    "Сортировка завершена");
            }
            catch
            {
                MessageBox.Show(
                    "Ошибка ввода массива");
            }
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "XML Файлы (*.xml)|*.xml|Все файлы (*.*)|*.*",
                Title = "Загрузить массив"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    currentArray = Services.XmlService.LoadArray(dialog.FileName);
                    ArrayTextBox.Text = string.Join(" ", currentArray);
                    DrawArray(currentArray, -1, -1);
                    MessageBox.Show("Массив успешно загружен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch
                {
                    MessageBox.Show("Ошибка при загрузке файла. Убедитесь, что это корректный XML-файл.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentArray == null || currentArray.Length == 0)
            {
                MessageBox.Show("Нет данных для сохранения. Сначала сгенерируйте или введите массив.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "XML Файлы (*.xml)|*.xml",
                Title = "Сохранить массив",
                FileName = "ArrayData.xml"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    Services.XmlService.SaveArray(currentArray, dialog.FileName);
                    MessageBox.Show("Массив успешно сохранен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch
                {
                    MessageBox.Show("Ошибка при сохранении файла.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            Views.HelpWindow helpWindow = new Views.HelpWindow();
            helpWindow.Owner = this; // Открывать по центру главного окна
            helpWindow.ShowDialog();
        }

        private void PauseButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            isPaused = !isPaused;

            PauseButton.Content =
                isPaused ? "ПРОДОЛЖИТЬ" : "ПАУЗА";
        }

        private void ResetButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            ChartCanvas.Children.Clear();

            ArrayTextBox.Clear();

            StatsTextBlock.Text = "";

            currentArray = Array.Empty<int>();
        }

        private void UpdateStats()
        {
            StatsTextBlock.Text =
                $"Сравнений: {comparisons}     " +
                $"Перестановок: {swaps}     " +
                $"Время: {stopwatch.ElapsedMilliseconds} мс";
        }

        private async Task PauseCheck()
        {
            while (isPaused)
            {
                await Task.Delay(100);
            }
        }

        private async Task BubbleSort(int[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = 0;
                     j < array.Length - i - 1;
                     j++)
                {
                    await PauseCheck();

                    comparisons++;

                    DrawArray(array, j, j + 1);

                    UpdateStats();

                    await Delay();

                    if (array[j] > array[j + 1])
                    {
                        swaps++;

                        (array[j], array[j + 1]) =
                            (array[j + 1], array[j]);

                        DrawArray(array, j, j + 1);

                        UpdateStats();

                        await Delay();
                    }
                }
            }
        }

        private async Task SelectionSort(int[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                int min = i;

                for (int j = i + 1;
                     j < array.Length;
                     j++)
                {
                    await PauseCheck();

                    comparisons++;

                    DrawArray(array, min, j);

                    UpdateStats();

                    await Delay();

                    if (array[j] < array[min])
                    {
                        min = j;
                    }
                }

                swaps++;

                (array[i], array[min]) =
                    (array[min], array[i]);

                DrawArray(array, i, min);

                UpdateStats();

                await Delay();
            }
        }

        private async Task InsertionSort(int[] array)
        {
            for (int i = 1; i < array.Length; i++)
            {
                int key = array[i];

                int j = i - 1;

                while (j >= 0 &&
                       array[j] > key)
                {
                    await PauseCheck();

                    comparisons++;

                    swaps++;

                    array[j + 1] = array[j];

                    DrawArray(array, j, j + 1);

                    UpdateStats();

                    await Delay();

                    j--;
                }

                array[j + 1] = key;
            }
        }

        private async Task QuickSort(
            int[] array,
            int left,
            int right)
        {
            int i = left;

            int j = right;

            int pivot =
                array[(left + right) / 2];

            while (i <= j)
            {
                while (array[i] < pivot)
                {
                    i++;
                }

                while (array[j] > pivot)
                {
                    j--;
                }

                if (i <= j)
                {
                    await PauseCheck();

                    comparisons++;

                    swaps++;

                    (array[i], array[j]) =
                        (array[j], array[i]);

                    DrawArray(array, i, j);

                    UpdateStats();

                    await Delay();

                    i++;

                    j--;
                }
            }

            if (left < j)
            {
                await QuickSort(array, left, j);
            }

            if (i < right)
            {
                await QuickSort(array, i, right);
            }
        }

        private async Task Delay()
        {
            await Task.Delay(
                (int)SpeedSlider.Value);
        }

        private void DrawArray(
            int[] array,
            int first,
            int second)
        {
            ChartCanvas.Children.Clear();

            if (array.Length == 0)
            {
                return;
            }

            double width =
                ChartCanvas.ActualWidth /
                array.Length;

            for (int i = 0; i < array.Length; i++)
            {
                Rectangle rectangle = new();

                rectangle.Width = width - 5;

                rectangle.Height = array[i] * 4;

                rectangle.Fill =
                    Brushes.SteelBlue;

                if (i == first ||
                    i == second)
                {
                    rectangle.Fill =
                        Brushes.OrangeRed;
                }

                Canvas.SetLeft(
                    rectangle,
                    i * width);

                Canvas.SetTop(
                    rectangle,
                    ChartCanvas.ActualHeight -
                    rectangle.Height);

                ChartCanvas.Children.Add(
                    rectangle);
            }
        }
    }
}