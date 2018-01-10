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
            LoadAccountList();
            LoadFoodList();
        }

        void LoadFoodList()
        {
            string query = "SELECT * FROM dbo.Food";

            dtgvFood.DataSource = DataProvider.Instance.ExecuteQuery(query);
        }
        void LoadAccountList()
        {
            //string query = "SELECT UserName as N'Tên tài khoản' FROM dbo.Account";
            string query = "EXEC dbo.USP_GetAccountByUserName @UserName";

            dtgvAccount.DataSource = DataProvider.Instance.ExecuteQuery(query,new object[] { "staff"});
        }

        
    }
}
