using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp01
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btn_login_Click(object sender, EventArgs e)
        {

            string username = txt_username.Text.Trim();
            string password = txt_password.Text.Trim();

            try
            {
                using (var db = new QLsinhvienDataContext())
                {
                    // Find student by MSSV (password)
                    var student = db.Students.SingleOrDefault(s => s.MSSV == password);
                    if (student != null)
                    {
                        // Expected email for a student is MSSV@st.huce.edu.vn
                        string expectedEmail = student.MSSV + "@st.huce.edu.vn";
                        if (string.Equals(username, expectedEmail, StringComparison.OrdinalIgnoreCase))
                        {
                            MessageBox.Show("Đăng nhập thành công");
                            Form QLSinhVien = new QLSinhVien();
                            QLSinhVien.Show();
                            this.Hide();
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle exception as needed; show a generic error for now
                MessageBox.Show("Lỗi khi kết nối cơ sở dữ liệu: " + ex.Message);
                return;
            }

            MessageBox.Show("Đăng nhập thất bại");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txt_username_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
