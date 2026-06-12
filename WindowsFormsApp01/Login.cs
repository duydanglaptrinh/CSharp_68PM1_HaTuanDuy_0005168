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

            // Tự động điền sẵn tài khoản admin để bạn bấm đăng nhập cho nhanh
            txt_username.Text = "admin";
            txt_password.Text = "admin";
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            string username = txt_username.Text.Trim();
            string password = txt_password.Text.Trim();

            // CHỈ GIỮ LẠI TÀI KHOẢN ADMIN VẠN NĂNG
            if (username == "admin" && password == "admin")
            {
                Main mainForm = new Main(this); // Truyền chính Form Login này vào Form Main
                mainForm.Show();
                this.Hide(); // Ẩn form Login đi
                return;
            }

            // Nếu nhập bất kỳ chữ nào khác ngoài admin/admin thì báo lỗi
            MessageBox.Show("Sai tài khoản hoặc mật khẩu! Vui lòng kiểm tra lại.", "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void txt_username_TextChanged(object sender, EventArgs e) { }
    }
}