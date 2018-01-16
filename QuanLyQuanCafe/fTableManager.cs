﻿using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fTableManager : Form
    {
        public fTableManager()
        {
            InitializeComponent();
            LoadTable();
            LoadCategory();
            //lsvBill.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            //lsvBill.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
        #region Methods
        void LoadCategory()
        {
            List<Category> lstCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = lstCategory;
            cbCategory.DisplayMember = "name";
        }

        void LoadFoodListByCategoryID(int id)
        {
            List<Food> lstFood = FoodDAO.Instance.GetListFoodByIDCategory(id);
            cbFood.DataSource = lstFood;
            cbFood.DisplayMember = "name";
        }

        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<Table> tableList = TableDAO.Instance.LoadTableList();
            foreach(Table item in tableList)
            {
                Button button = new Button() { Width = TableDAO.tableWidth, Height = TableDAO.tableHeight };
                button.Text = item.Name + Environment.NewLine + item.Status;
                button.Click += Button_Click1;
                button.Tag = item;
                switch (item.Status)
                {
                    case "Trống":
                        button.BackColor = Color.Aqua;
                        break;
                    default:
                        button.BackColor = Color.YellowGreen;
                        break;
                }
                flpTable.Controls.Add(button);
            }
        }
        void ShowBill(int tableID)
        {
            lsvBill.Items.Clear();
            List<DTO.Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(tableID);
            float totalPrice = 0;

            foreach(DTO.Menu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;
                lsvBill.Items.Add(lsvItem);
            }
            CultureInfo culture = new CultureInfo("vi-VN");
            Thread.CurrentThread.CurrentCulture = culture;
            txbTotalPrice.Text = totalPrice.ToString("c");
            
        }


        #endregion

        #region Events
        private void Button_Click1(object sender, EventArgs e)
        {
            int tableID = (((sender as Button).Tag) as Table).Id;
            // Lấy dữ liệu bàn gán vào lsvFood
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableID);
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile();
            f.ShowDialog();
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.ShowDialog();
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;
            ComboBox cb = sender as ComboBox;

            Category selected = cb.SelectedItem as Category;
            id = selected.ID;
            LoadFoodListByCategoryID(id);
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            int idBill = BillDAO.Instance.GetUnCheckBillGetByID(table.Id);
            int idFood = (cbFood.SelectedItem as Food).ID;
            int count = (int)nmCountFood.Value;

            // nếu bàn không có bill thì tạo bill mới cho bàn đó
            if(idBill == -1)
            {
                BillDAO.Instance.InsertBill(table.Id);
                // Thêm BillInfo
                BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(), idFood, count);

            }

            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, idFood, count);
            }
            ShowBill(table.Id);
            LoadTable();
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            int idBill = BillDAO.Instance.GetUnCheckBillGetByID(table.Id);
            int discount = (int)nmDiscount.Value;
            double totalPrice = double.Parse(txbTotalPrice.Text, NumberStyles.Currency);
            double finalTotalPrice = totalPrice - (totalPrice/100) * discount;
            if (idBill != -1)
            {
                if(MessageBox.Show(string.Format("Bạn có chắc muốn thanh toán hóa đơn cho {0} ?\n Tổng tiền - (Tổng tiền /100) * discount = {1} - ({1}/100)*{2} = {3}",table.Name,totalPrice,discount,finalTotalPrice),"Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill,discount);
                    // Sau khi thanh toán thì Show lại bill --> đã chuyển status về 1 nên không hiện chi tiết bill
                    ShowBill(table.Id);
                    LoadTable();
                }
            }
        }


        #endregion


    }
}
