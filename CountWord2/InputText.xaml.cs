using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace CountWord2
{
    /// <summary>
    /// Логика взаимодействия для InputText.xaml
    /// </summary>
    public partial class InputText : Window
    {
        public InputText()
        {
            InitializeComponent();
        }

        private void SaveText_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(InputTextBox.Text))
            {
                MainWindow.originalText = InputTextBox.Text;
                this.Close();
            }
            else
                MessageBox.Show("Поле для вставки текста не заполнено!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
