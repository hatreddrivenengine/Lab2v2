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

namespace Lab2v2
{
    /// <summary>
    /// Логика взаимодействия для ThreatInfo.xaml
    /// </summary>
    public partial class ThreatInfo : Window
    {
        public ThreatInfo(ISThreat threat)
        {
            InitializeComponent();
            ThreatID.Text = threat.ThreatId.ToString();
            ThreatCnf.Text = (threat.ConfidentialityViolation ? "Да" : "Нет");
            ThreatAvb.Text = (threat.AvailabilityViolation ? "Да" : "Нет");
            ThreatIng.Text = (threat.IntegrityViolation ? "Да" : "Нет");
            ThreatObj.Text = threat.ThreaObj.ToString();
            ThreatInf.Text = threat.ThreatInfo.ToString();
            ThreatName.Text = threat.ThreatName.ToString();
            ThreatSource.Text = threat.ThreatSource.ToString();
        }


    }
}
