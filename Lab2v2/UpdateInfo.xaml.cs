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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab2v2
{
    /// <summary>
    /// Логика взаимодействия для UpdateInfo.xaml
    /// </summary>
    public partial class UpdateInfo 
    {
        public UpdateInfo()
        {
            InitializeComponent();
            UpdatedThreats.Text = ChangeLogObhect.LogCountChng.ToString();
            UpdateStatus.Text = ChangeLogObhect.UpdateStatus.ToString();
            NewThreats.Text = ChangeLogObhect.LogCountNew.ToString();
        }

        private void UpdateStatus_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
