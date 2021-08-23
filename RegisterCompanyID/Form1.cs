using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegisterCompanyID
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            if (SQL.Con.State == ConnectionState.Open)
            {
                SQL.Con.Close();
            }
            SQL.Con.Open();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        private void txtId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtId.Text.Length == 0 || txtName.Text.Length == 0)
            {
                MessageBox.Show("Please input company id and name", "Error");
                return;
            }

            int companyID = int.Parse(txtId.Text);
            string companyName = txtName.Text;
            DateTime dtExp = dtExpir.Value;

            if (dtExp <= DateTime.Now)
            {
                MessageBox.Show("Please input expiry date correctly", "Error");
                return;
            }

            if (companyID == 0)
            {
                MessageBox.Show("Please input company id correctly", "Error");
                return;
            }

            int id;
            string name;
            IsExist(out id, out name);
            if (id != 0 && name.Length != 0)
            {
                MessageBox.Show("Company ID already exist", "Info");
                return;
            }

            string sql = $"INSERT INTO Company (PKCompany, CompanyName, IsActive, CreatedDate, ExpiryDate) VALUES ({companyID}, '{companyName}', 1, GetDate(), '{dtExp.ToString("yyyy-MM-dd")}')";
            string result = SQL.ScalarQuery(sql);

            MessageBox.Show("It added successfully!");
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (txtId.Text.Length == 0 || txtName.Text.Length == 0)
            {
                MessageBox.Show("Please input company id and name", "Error");
                return;
            }

            int companyID = int.Parse(txtId.Text);
            string companyName = txtName.Text;

            int id;
            string name;
            IsExist(out id, out name);
            if (id == 0 && name.Length == 0)
            {
                MessageBox.Show("Company ID not exist", "Info");
                return;
            }

            string sql = $"delete FROM Company WHERE PKCompany={companyID}";
            string result = SQL.ScalarQuery(sql);

            txtId.Text = "";
            txtName.Text = "";
            txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            MessageBox.Show("It removed successfully!", "Info");
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (txtId.Text.Length == 0 && txtName.Text.Length == 0)
            {
                MessageBox.Show("Please input company id and name", "Error");
                return;
            }

            if (txtId.Text.Length != 0)
            {
                string sql = $"select * from Company where PKCompany = {txtId.Text}";
                DataTable companyResult = SQL.GetDataTable(sql);
                if (companyResult.Rows.Count == 0)
                {
                    MessageBox.Show("That Company ID does not exist", "Info");
                    return;
                }
                foreach (DataRow row in companyResult.Rows)
                {
                    string company_na = row["CompanyName"].ToString();
                    int company_id = int.Parse(row["PKCompany"].ToString());
                    string date = row["CreatedDate"].ToString();
                    DateTime dtExp = DateTime.Now;
                    if (row["ExpiryDate"].ToString().Length != 0)
                        dtExp = DateTime.Parse(row["ExpiryDate"].ToString());

                    txtId.Text = company_id.ToString();
                    txtName.Text = company_na;
                    if (date != "")
                    { 
                        txtDate.Text = DateTime.Parse(date).ToString("yyyy-MM-dd");
                        dtExpir.Value = dtExp;
                    }
                }
            }
            else if(txtName.Text.Length != 0)
            {
                string sql = $"select * from Company where CompanyName = '{txtName.Text}'";
                DataTable companyResult = SQL.GetDataTable(sql);
                if (companyResult.Rows.Count == 0)
                {
                    MessageBox.Show("That Company Name does not exist", "Info");
                    return;
                }
                foreach (DataRow row in companyResult.Rows)
                {
                    string company_na = row["CompanyName"].ToString();
                    int company_id = int.Parse(row["PKCompany"].ToString());
                    string date = row["CreatedDate"].ToString();

                    txtId.Text = company_id.ToString();
                    txtName.Text = company_na;
                    if (date != "")
                        txtDate.Text = DateTime.Parse(date).ToString("yyyy-MM-dd");
                }
            }

        }

        public void IsExist(out int id, out string name)
        {
            id = 0;
            name = "";

            string sql = $"select * from Company where PKCompany = {txtId.Text}";
            DataTable companyResult = SQL.GetDataTable(sql);
            foreach (DataRow row in companyResult.Rows)
            {
                string company_na = row["CompanyName"].ToString();
                int company_id = int.Parse(row["PKCompany"].ToString());
                string date = row["CreatedDate"].ToString();
                id = company_id;
                name = company_na;

                txtId.Text = id.ToString();
                txtName.Text = name;
                if (date != "")
                    txtDate.Text = DateTime.Parse(date).ToString("yyyy-MM-dd");
            }
        }

        private void dtExpir_ValueChanged(object sender, EventArgs e)
        {
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtId.Text.Length == 0 || txtName.Text.Length == 0)
            {
                MessageBox.Show("Please input company id and name", "Error");
                return;
            }

            int companyID = int.Parse(txtId.Text);
            string companyName = txtName.Text;
            DateTime dtExp = dtExpir.Value;

            int id;
            string name;
            IsExist(out id, out name);
            if (id == 0 && name.Length == 0)
            {
                MessageBox.Show("Company ID not exist", "Info");
                return;
            }

            string sql = $"update Company set ExpiryDate='{dtExp.ToString("yyyy-MM-dd")}' WHERE PKCompany={companyID} and CompanyName='{companyName}'";
            string result = SQL.ScalarQuery(sql);

            if (dtExp > DateTime.Parse(txtDate.Text))
                sql = $"update Company set IsActive='1' WHERE PKCompany={companyID} and CompanyName='{companyName}'";
            else
                sql = $"update Company set IsActive='0' WHERE PKCompany={companyID} and CompanyName='{companyName}'";
            result = SQL.ScalarQuery(sql);

            MessageBox.Show("It updated successfully!", "Info");
        }
    }
}
