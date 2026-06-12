using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp01
{
    public partial class Main : Form
    {
        // Khởi tạo sẵn 2 UserControl
        private UC_QLLH uc_qllh = new UC_QLLH();
        private UC_QLSV uc_qlsv = new UC_QLSV();

        // Biến cục bộ dùng để lưu trữ Form Login đang ẩn
        private Form loginForm;
        private bool isLoggingOut = false; // Cờ kiểm soát việc văng phần mềm

        // 1. Hàm khởi tạo mặc định (BẮT BUỘC phải giữ lại để file Designer.cs không bị lỗi Build)
        public Main()
        {
            InitializeComponent();
        }

        // 2. Hàm khởi tạo mở rộng: Nhận Form Login truyền vào để xử lý Đăng xuất mượt mà
        public Main(Form login) : this()
        {
            this.loginForm = login;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            // Vừa mở lên thì nạp ngay màn hình sinh viên
            ShowUserControl(uc_qlsv);
        }

        private void ShowUserControl(UserControl uc)
        {
            panelContainer.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            panelContainer.Controls.Add(uc);
        }

        private void btnQLLH_Click(object sender, EventArgs e)
        {
            ShowUserControl(uc_qllh);
            DoiTrangThaiNut(btnQLLH);
        }

        private void btnQLSV_Click(object sender, EventArgs e)
        {
            ShowUserControl(uc_qlsv);
            DoiTrangThaiNut(btnQLSV);
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                isLoggingOut = true; // Báo cho hệ thống biết là đang đăng xuất một cách chủ động
                if (loginForm != null)
                {
                    loginForm.Show(); // Gọi Form Login đang ẩn hiện lên lại
                }
                this.Close(); // Đóng form Main
            }
        }

        // FIX LỖI ỨNG DỤNG BỊ TREO NGẦM: Nếu tắt app bằng dấu X thì ép giải phóng toàn bộ!
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            if (!isLoggingOut)
            {
                Application.Exit(); // Dọn dẹp sạch sẽ toàn bộ tiến trình ngầm
            }
        }

        // Đổi Font chữ bôi đậm thanh menu điều hướng
        private void DoiTrangThaiNut(Button activeBtn)
        {
            btnQLSV.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            btnQLLH.Font = new Font("Segoe UI", 10F, FontStyle.Regular);

            activeBtn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        }
    }
}