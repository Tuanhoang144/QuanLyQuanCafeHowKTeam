using QuanLyQuanCafe.DAO;
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
        public fAdmin()
        {
            InitializeComponent();
            LoadDateTimePickerBill();
            LoadBillListByDate(dtpkFromDate.Value, dtpkToDate.Value);
            
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

        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadBillListByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }
    }
}
