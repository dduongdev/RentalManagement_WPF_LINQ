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
using System.Data.Linq;
using RentalManagement_WPF_LINQ.Utils;

namespace RentalManagement_WPF_LINQ
{
    /// <summary>
    /// Interaction logic for HomeManagement.xaml
    /// </summary>
    public partial class HomeManagement : Window
    {
        RentalManagementDataContext dc = new RentalManagementDataContext();
        Table<NHA> homes;
        Table<HOPDONG> contracts;

        public HomeManagement()
        {
            InitializeComponent();
            this.Loaded += HomeManagement_Loaded;
        }

        private void HomeManagement_Loaded(object sender, RoutedEventArgs e)
        {
            homes = dc.GetTable<NHA>();
            contracts = dc.GetTable<HOPDONG>();


            loadHomes();

            btnSaveHome.Click += BtnSaveHome_Click;
            btnDeleteHome.Click += BtnDeleteHome_Click;
            btnRefreshForm.Click += BtnRefreshForm_Click;
            dgHomes.SelectionChanged += DgHomes_SelectionChanged;
            btnUpdateHome.Click += BtnUpdateHome_Click;
        }

        private void BtnUpdateHome_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtHomeId.Text))
            {
                MessageBox.Show("Mã nhà không được trống.", "Quản lý nhà");
                return;
            }

            var targetHome = homes.FirstOrDefault(s => s.MaNha == txtHomeId.Text);
            if(targetHome == null)
            {
                MessageBox.Show("Mã nhà không tồn tại.", "Quản lý nhà");
                return;
            }

            if (string.IsNullOrEmpty(txtHomeOwnerName.Text))
            {
                MessageBox.Show("Tên chủ nhà không được trống.", "Quản lý nhà");
                return;
            }

            if (!Validator.IsNumeric(txtHomeRentalPrice.Text))
            {
                MessageBox.Show("Giá thuê phải là giá trị số.", "Quản lý nhà");
                return;
            }

            try
            {
                targetHome.TenChuNha = txtHomeOwnerName.Text;
                targetHome.GiaThue = double.Parse(txtHomeRentalPrice.Text);
                targetHome.DaCHoThue = chkHomeIsRenting.IsChecked == true;

                dc.SubmitChanges();

                MessageBox.Show("Sửa thông tin nhà thành công!", "Quản lý nhà");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Quản lý nhà");
            }
        }

        private void DgHomes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedHome = dgHomes.SelectedItem as NHA;
            if(selectedHome != null)
            {
                txtHomeId.Text = selectedHome.MaNha;
                txtHomeOwnerName.Text = selectedHome.TenChuNha;
                txtHomeRentalPrice.Text = selectedHome.GiaThue.ToString();
                chkHomeIsRenting.IsChecked = selectedHome.DaCHoThue;
            }
        }

        private void BtnRefreshForm_Click(object sender, RoutedEventArgs e)
        {
            txtHomeId.Clear();
            txtHomeId.Focus();
            txtHomeOwnerName.Clear();
            txtHomeRentalPrice.Clear();
            chkHomeIsRenting.IsChecked = false;

            loadHomes();
        }

        private void BtnDeleteHome_Click(object sender, RoutedEventArgs e)
        {
            // selectedItem sẽ trả về đối tượng có kiểu dữ liệu khi ta gán vào ItemsSource
            var selectedHome = dgHomes.SelectedItem as NHA;
            try
            {
                if (selectedHome != null)
                {
                    homes.DeleteOnSubmit(selectedHome);
                    dc.SubmitChanges();
                    MessageBox.Show("Xoá thông tin nhà thành công!", "Quản lý nhà");
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn nhà cần xoá!", "Quản lý nhà");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Quản lý nhà");
            }
        }

        private void BtnSaveHome_Click(object sender, RoutedEventArgs e)
        {
            var isExist = homes.FirstOrDefault(s => s.MaNha == txtHomeId.Text);
            if(isExist != null)
            {
                MessageBox.Show("Mã nhà đã tồn tại!", "Quản lý nhà");
                return;
            }

            if (string.IsNullOrEmpty(txtHomeOwnerName.Text))
            {
                MessageBox.Show("Tên chủ nhà không được trống.", "Quản lý nhà");
                return;
            }

            if (!Validator.IsNumeric(txtHomeRentalPrice.Text))
            {
                MessageBox.Show("Giá thuê phải là giá trị số.", "Quản lý nhà");
                return;
            }

            try
            {
                NHA home = new NHA();
                home.MaNha = txtHomeId.Text.Trim();
                home.TenChuNha = txtHomeOwnerName.Text;
                home.GiaThue = double.Parse(txtHomeRentalPrice.Text);
                home.DaCHoThue = chkHomeIsRenting.IsChecked == true;

                homes.InsertOnSubmit(home);
                dc.SubmitChanges();

                MessageBox.Show("Thêm nhà thành công!", "Quản lý nhà");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Quản lý nhà");
            }
        }

        private void loadHomes()
        {
            dgHomes.ItemsSource = from home in homes
                                  select home;
        }
    }
}
