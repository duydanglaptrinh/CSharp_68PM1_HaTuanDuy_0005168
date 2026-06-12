using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp01
{
    public partial class Main : Form
    {
        private UC_QLLH uc_qllh = new UC_QLLH();
        private UC_QLSV uc_qlsv = new UC_QLSV();

        private Form loginForm;
        private bool isLoggingOut = false;

        public Main()
        {
            InitializeComponent();
        }

        public Main(Form login) : this()
        {
            this.loginForm = login;
        }

        private void Main_Load(object sender, EventArgs e)
        {
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
            // FIX ĐỒNG BỘ: Ép ô chọn lớp học bên tab sinh viên tải lại danh sách mới nhất từ DB
            uc_qlsv.LoadLopHoc();

            ShowUserControl(uc_qlsv);
            DoiTrangThaiNut(btnQLSV);
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                isLoggingOut = true;
                if (loginForm != null)
                {
                    loginForm.Show();
                }
                this.Close();
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            if (!isLoggingOut)
            {
                Application.Exit();
            }
        }

        private void DoiTrangThaiNut(Button activeBtn)
        {
            btnQLSV.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            btnQLLH.Font = new Font("Segoe UI", 10F, FontStyle.Regular);

            activeBtn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        }
    }
}