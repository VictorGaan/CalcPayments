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
using System.Windows.Shapes;

namespace ItVolga
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog Path = new SaveFileDialog();
            GridPayments.SelectAllCells();
            GridPayments.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, GridPayments);
            String resultat = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
            GridPayments.UnselectAllCells();
            Path.FileName = "report";
            Path.DefaultExt = ".csv";
            Path.Filter = "Csv documents (.csv)|*.csv";
            Nullable<bool> result = Path.ShowDialog();
            string filename = Path.FileName;
            StreamWriter file1 = new StreamWriter(Path.FileName);
            file1.WriteLine(resultat);
            file1.Close();
        }
    }
}
