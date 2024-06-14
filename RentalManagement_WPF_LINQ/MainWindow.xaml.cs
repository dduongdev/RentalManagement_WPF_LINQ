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

namespace RentalManagement_WPF_LINQ
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            btnShowHomes.Click += BtnShowHomes_Click;
            btnShowContracts.Click += BtnShowContracts_Click;
        }

        private void BtnShowContracts_Click(object sender, RoutedEventArgs e)
        {
            ContractManagement frmContractManagement = new ContractManagement();
            frmContractManagement.Closed += OtherForm_Closed;
            this.Visibility = Visibility.Hidden;
            frmContractManagement.Show();
        }

        private void OtherForm_Closed(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Visible;
        }

        private void BtnShowHomes_Click(object sender, RoutedEventArgs e)
        {
            HomeManagement frmHomeManagement = new HomeManagement();
            frmHomeManagement.Closed += new EventHandler(OtherForm_Closed);
            this.Visibility = Visibility.Hidden;
            frmHomeManagement.Show();
        }
    }
}
