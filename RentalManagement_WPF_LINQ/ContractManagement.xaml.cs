using System.Linq;
using System.Data.Linq;
using System.Windows;

namespace RentalManagement_WPF_LINQ
{
    /// <summary>
    /// Interaction logic for ContractManagement.xaml
    /// </summary>
    public partial class ContractManagement : Window
    {
        RentalManagementDataContext dc = new RentalManagementDataContext();
        Table<NHA> homes;
        Table<HOPDONG> contracts;
        Table<KHACHTHUENHA> customers;

        public ContractManagement()
        {
            InitializeComponent();
            this.Loaded += ContractManagement_Loaded;
        }

        private void ContractManagement_Loaded(object sender, RoutedEventArgs e)
        {
            homes = dc.GetTable<NHA>();
            contracts = dc.GetTable<HOPDONG>();
            customers = dc.GetTable<KHACHTHUENHA>();

            loadData();
        }

        private void loadData()
        {
            var contractsInfo = from contract in contracts
                                join home in homes on contract.MaNha equals home.MaNha
                                join customer in customers on contract.MaKhach equals customer.MaKhach
                                select new
                                {
                                    contractId = contract.SoHD,
                                    homeOwnerName = home.TenChuNha,
                                    customerName = customer.TenKhach,
                                    startDate = contract.NgayBatDau,
                                    endDate = contract.NgayKetThuc,
                                    total = (contract.NgayKetThuc - contract.NgayBatDau).Days / 30 * home.GiaThue
                                };
            dgHighestRevenueContracts.ItemsSource = contractsInfo.Where(s => s.total == contractsInfo.Max(u => u.total));
            dgContracts.ItemsSource = contractsInfo;
        }
    }
}
