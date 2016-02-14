using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CountWord2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string originalText;
        public static string processedText;
        w1 firstW;
        w2 secondW;
        StatisticsWindow sw;

        public MainWindow()
        {
            InitializeComponent();
            Title = "CountWordsApp";
            comboBoxTypeViewText.Items.Add("Считанный текст");
            comboBoxTypeViewText.Items.Add("Обработанный текст");
            comboBoxTypeViewText.SelectedIndex = 1;

            comboBoxTypeOpenFile.Items.Add("Cкопировать текст");
            comboBoxTypeOpenFile.Items.Add("Выбрать txt файл");
            comboBoxTypeOpenFile.SelectedIndex = 0;

            ChooseContent.IsEnabled = false;
            CountWords.IsEnabled = false;
            comboBoxTypeViewText.IsEnabled = false;
        }

        private void ChooseFile_Click(object sender, RoutedEventArgs e)
        {
            originalText = null;
            processedText = null;

            //Выбор типа открытия файла
            if (comboBoxTypeOpenFile.SelectedIndex == 1)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Текстовые файлы|*.txt";
                openFileDialog.Title = "Выбирете текстовый файл";

                if (openFileDialog.ShowDialog() == true)
                    originalText = File.ReadAllText(openFileDialog.FileName, Encoding.Default);
            }
            else
            {
                InputText it = new InputText();
                it.ShowDialog();
            }

            //обработка originalText, разблокировка элементов интерфейса для дальнейшей работы
            if (!String.IsNullOrEmpty(originalText))
            {
                this.Title = "CountWordsApp: preparing for displaying read file...";
                Task.Factory.StartNew(() => Dispatcher.Invoke(() => MainTextBox.Text = originalText))
                    .ContinueWith(prevTask => Dispatcher.Invoke(() => this.Title = "CountWordsApp: done"));

                processedText = originalText.ToLower();

                string[] mas = new string[] { "®", "“", "\"", "«", "»", "’", " - ", " — ", "—", "–", ":", ";", "(", ")", "[", "]", "%", "*", "...", "…", ",", ".", "!", "?", "#" };

                for (int i = 0; i < mas.Length; i++)
                    processedText = processedText.Replace(mas[i], "").Replace(i.ToString(), "");

                processedText = processedText.Replace("/", " ").Replace("\r", " ").Replace("\n", " ");

                comboBoxTypeViewText.IsEnabled = true;
                ChooseContent.IsEnabled = true;
                CountWords.IsEnabled = true;
            }
            else
                MessageBox.Show("Текст для анализа не выбран!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void CountWords_Click(object sender, RoutedEventArgs e)
        {
           

            var words = processedText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            Dictionary<string, int> wordCount = new Dictionary<string, int>();

            MainTextBox.Text = "Количество                   Слово\r\n";
            MainTextBox.AppendText("--------------                  --------\r\n");

            this.Title = "CountWordsApp: counting...";

            Task.Factory.StartNew(() =>
                {
                    foreach (var word in words)
                    {
                        if (wordCount.ContainsKey(word))
                            wordCount[word]++;
                        else
                            wordCount[word] = 1;
                    }
                }).ContinueWith(prevTask => Dispatcher.Invoke(()=>
                {
                    this.Title = "CountWordsApp: preparing for displaying calculating results...";

                    foreach (var item in wordCount.OrderByDescending(k => k.Value))
                    {
                        string space = new string(' ', 29 - item.Value.ToString().Length);
                        string output = String.Format("        {0}{1}{2}\r\n", item.Value, space, item.Key);
                        MainTextBox.AppendText(output);
                    }

                    
                    firstW = new w1();
                    secondW = new w2();

                    firstW.w1TextBox.Text = "Cлова\r\n";
                    secondW.w2TextBox.Text = "Количество\r\n";

                    foreach (var item in wordCount.OrderByDescending(k => k.Value))
                    {
                        firstW.w1TextBox.AppendText(item.Key.ToString()+"\r\n");
                        secondW.w2TextBox.AppendText(item.Value.ToString() + "\r\n");

                    }

                    //firstW.Show();
                    //secondW.Show();
                    
                })).ContinueWith(prevTask => Dispatcher.Invoke(() => 
                {
                    int words_length = words.Length;
                    
                    int[] NumberOfWords = new int[11];
                    for (int i = 0; i <= 10; i++)
                        NumberOfWords[i] = wordCount.Where(item => item.Key.Length == i+1).Select(v => v.Value).Sum();
                    NumberOfWords[10] = wordCount.Where(item => item.Key.Length >= 11).Select(v => v.Value).Sum();
                    
                    this.Title = "CountWordsApp: done";

                    string str = String.Format("Обработано {0} слов(а/о)\r\n", words_length);
                    str += String.Format("Уникальных слов: {0}\r\n\r\n", wordCount.Keys.Count);
                    str += String.Format("Cлов длиной 1: {0} ({1:#.##}%)\r\n", NumberOfWords[0], (double)NumberOfWords[0] / words_length * 100);
                    str += String.Format("Cлов длиной 2: {0} ({1:#.##}%)\r\n", NumberOfWords[1], (double)NumberOfWords[1] / words_length * 100);
                    str += String.Format("Cлов длиной 3: {0} ({1:#.##}%)\r\n", NumberOfWords[2], (double)NumberOfWords[2] / words_length * 100);
                    str += String.Format("Cлов длиной 4: {0} ({1:#.##}%)\r\n", NumberOfWords[3], (double)NumberOfWords[3] / words_length * 100);
                    str += String.Format("Cлов длиной 5: {0} ({1:#.##}%)\r\n", NumberOfWords[4], (double)NumberOfWords[4] / words_length * 100);
                    str += String.Format("Cлов длиной 6: {0} ({1:#.##}%)\r\n", NumberOfWords[5], (double)NumberOfWords[5] / words_length * 100);
                    str += String.Format("Cлов длиной 7: {0} ({1:#.##}%)\r\n", NumberOfWords[6], (double)NumberOfWords[6] / words_length * 100);
                    str += String.Format("Cлов длиной 8: {0} ({1:#.##}%)\r\n", NumberOfWords[7], (double)NumberOfWords[7] / words_length * 100);
                    str += String.Format("Cлов длиной 9: {0} ({1:#.##}%)\r\n", NumberOfWords[8], (double)NumberOfWords[8] / words_length * 100);
                    str += String.Format("Cлов длиной 10: {0} ({1:#.##}%)\r\n", NumberOfWords[9], (double)NumberOfWords[9] / words_length * 100);
                    str += String.Format("Остальные слова: {0} ({1:#.##}%)\r\n", NumberOfWords[10], (double)NumberOfWords[10] / words_length * 100);

                    sw = new StatisticsWindow();
                    sw.Show();
                    sw.swTextBox.Text = str;

                }));
        }

        private void ChooseContent_Click(object sender, RoutedEventArgs e)
        {
            this.Title = "CountWordsApp: preparing for displaying text...";

            Task.Factory.StartNew(()=> Dispatcher.Invoke(()=>
                {
                    if (comboBoxTypeViewText.SelectedIndex == 0)
                        MainTextBox.Text = originalText;
                    else
                        MainTextBox.Text = processedText;
                })).ContinueWith(prevTask => Dispatcher.Invoke(() => this.Title = "CountWordsApp: done"));
        }
    }
}
