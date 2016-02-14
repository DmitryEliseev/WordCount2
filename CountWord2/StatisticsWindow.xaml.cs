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
    /// Interaction logic for StatisticsWindow.xaml
    /// </summary>
    public partial class StatisticsWindow : Window
    {
        double screeWidth = SystemParameters.FullPrimaryScreenWidth;
        double screeHeight = SystemParameters.FullPrimaryScreenHeight;

        public StatisticsWindow()
        {
            InitializeComponent();
            this.Left = screeWidth - (screeWidth - 656) / 2;
            this.Top = screeHeight - (screeWidth - 350) / 2;
        }
    }
}
