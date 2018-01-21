using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fAdmin : Form
    {
        BindingSource foodlist = new BindingSource();
        public fAdmin()
        {
            InitializeComponent();
            LoadInitial();
        }
        #region methods
        void LoadInitial()
        {
            dtgvFood.DataSource = foodlist;
            LoadListFood();
            LoadDateTimePickerBill();
            LoadBillListByDate(dtpkFromDate.Value, dtpkToDate.Value);
            AddFoodBinding();
            LoadCategoryIntoComboBox(cbCategory_food);
        }

        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }

        void LoadBillListByDate(DateTime from,DateTime to)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDate(from,to);
        }

        void LoadListFood()
        {
            foodlist.DataSource = FoodDAO.Instance.GetListFood();
        }

        void AddFoodBinding()
        {
            txtbFoodName.DataBindings.Add(new Binding("Text",dtgvFood.DataSource,"name"));
            txbFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "id"));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "price"));
        }

        void LoadCategoryIntoComboBox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "name";
        }
        #endregion

        #region events
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadBillListByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }

        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }
        #endregion

        private void txbFoodID_TextChanged(object sender, EventArgs e)
        {
            int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["IDCategory"].Value;

            Category category = CategoryDAO.Instance.GetCategoryByID(id);

            cbCategory_food.SelectedIndex = category.ID - 1; 
        }
    }
}
