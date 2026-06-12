using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp01
{
    public partial class UC_QLLH : UserControl
    {
        QLsinhvienDataContext db = new QLsinhvienDataContext();

        private int pageSize = 10;
        private int currentPage = 1;
        private int totalPages = 1;
        private int totalRecords = 0;
        private string searchKeyword = "";

        private string sortColumn = "ClassId";
        private bool isAscending = true;

        public UC_QLLH()
        {
            InitializeComponent();
            this.Load += UC_QLLH_Load;
        }

        private void UC_QLLH_Load(object sender, EventArgs e)
        {
            try
            {
                FormatUI();

                dgvLopHoc.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvLopHoc.ReadOnly = true;
                dgvLopHoc.AutoGenerateColumns = true;

                dgvLopHoc.DataBindingComplete += (s, ev) => dgvLopHoc.ClearSelection();

                // ĐÃ BỔ SUNG: Kết nối sự kiện cho nút Thêm
                btn_add.Click += btn_add_Click;
                btn_edit.Click += btn_edit_Click;
                btn_delete.Click += btn_delete_Click;
                btn_clear.Click += btn_clear_Click;
                btn_search.Click += btn_search_Click;
                btn_view_list.Click += btn_view_list_Click;

                button6.Click += btn_head_Click;
                button7.Click += button7_Click;
                button8.Click += button8_Click;
                button9.Click += btn_tail_Click;

                dgvLopHoc.CellClick += dgvLopHoc_CellClick;
                dgvLopHoc.ColumnHeaderMouseClick += dgvLopHoc_ColumnHeaderMouseClick;

                labelTimKiem.Text = "Tìm kiếm (Mã lớp / Tên lớp):";

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải giao diện: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatUI()
        {
            dgvLopHoc.BorderStyle = BorderStyle.None;
            dgvLopHoc.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 250);
            dgvLopHoc.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvLopHoc.DefaultCellStyle.SelectionBackColor = Color.FromArgb(41, 128, 185);
            dgvLopHoc.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvLopHoc.BackgroundColor = Color.White;
            dgvLopHoc.RowTemplate.Height = 35;

            dgvLopHoc.DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Regular);

            dgvLopHoc.EnableHeadersVisualStyles = false;
            dgvLopHoc.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvLopHoc.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            dgvLopHoc.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvLopHoc.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvLopHoc.ColumnHeadersHeight = 40;

            FormatButton(btn_add, Color.FromArgb(46, 204, 113));
            FormatButton(btn_edit, Color.FromArgb(243, 156, 18));
            FormatButton(btn_delete, Color.FromArgb(231, 76, 60));
            FormatButton(btn_clear, Color.FromArgb(149, 165, 166));
            FormatButton(btn_search, Color.FromArgb(52, 73, 94));
            FormatButton(btn_view_list, Color.FromArgb(41, 128, 185));
        }

        private void FormatButton(Button btn, Color bgColor)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = bgColor;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
        }

        private void btn_view_list_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaLop.Text))
            {
                MessageBox.Show("Vui lòng click chọn một lớp học trong bảng để xem danh sách sinh viên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maLop = txtMaLop.Text;
            string tenLop = txtTenLop.Text;

            var danhSachSV = db.Students.Where(s => s.ClassId == maLop).ToList();

            if (danhSachSV.Count == 0)
            {
                MessageBox.Show($"[{tenLop}] hiện tại chưa có sinh viên nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Form popupForm = new Form();
            popupForm.Text = $"Danh sách sinh viên - {tenLop} ({maLop})";
            popupForm.Size = new Size(800, 450);
            popupForm.StartPosition = FormStartPosition.CenterParent;
            popupForm.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            popupForm.BackColor = Color.White;
            popupForm.ShowIcon = false;

            DataGridView dgvPopUp = new DataGridView();
            dgvPopUp.Dock = DockStyle.Fill;

            dgvPopUp.DataSource = danhSachSV;
            dgvPopUp.ReadOnly = true;
            dgvPopUp.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPopUp.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPopUp.BackgroundColor = Color.White;
            dgvPopUp.BorderStyle = BorderStyle.None;

            dgvPopUp.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 250);
            dgvPopUp.EnableHeadersVisualStyles = false;
            dgvPopUp.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvPopUp.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            dgvPopUp.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPopUp.ColumnHeadersHeight = 40;
            dgvPopUp.RowTemplate.Height = 35;

            if (dgvPopUp.Columns["MSSV"] != null) dgvPopUp.Columns["MSSV"].HeaderText = "Mã SV";
            if (dgvPopUp.Columns["FullName"] != null) dgvPopUp.Columns["FullName"].HeaderText = "Họ và Tên";
            if (dgvPopUp.Columns["DateOfBirth"] != null) dgvPopUp.Columns["DateOfBirth"].HeaderText = "Ngày Sinh";
            if (dgvPopUp.Columns["Gender"] != null) dgvPopUp.Columns["Gender"].HeaderText = "Giới Tính";
            if (dgvPopUp.Columns["ClassId"] != null) dgvPopUp.Columns["ClassId"].Visible = false;
            if (dgvPopUp.Columns["Class"] != null) dgvPopUp.Columns["Class"].Visible = false;

            dgvPopUp.DataBindingComplete += (s, ev) => dgvPopUp.ClearSelection();

            popupForm.Controls.Add(dgvPopUp);
            popupForm.ShowDialog();
        }

        public void LoadData()
        {
            var query = db.Classes.AsQueryable();

            if (!string.IsNullOrEmpty(searchKeyword))
            {
                query = query.Where(c => c.ClassId.Contains(searchKeyword) ||
                                         c.ClassName.Contains(searchKeyword));
            }

            switch (sortColumn)
            {
                case "ClassId":
                    query = isAscending ? query.OrderBy(c => c.ClassId) : query.OrderByDescending(c => c.ClassId);
                    break;
                case "ClassName":
                    query = isAscending ? query.OrderBy(c => c.ClassName) : query.OrderByDescending(c => c.ClassName);
                    break;
                case "Note":
                    query = isAscending ? query.OrderBy(c => c.Note) : query.OrderByDescending(c => c.Note);
                    break;
                default:
                    query = query.OrderBy(c => c.ClassId);
                    break;
            }

            totalRecords = query.Count();
            totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (totalPages == 0) totalPages = 1;
            if (currentPage > totalPages) currentPage = totalPages;
            if (currentPage < 1) currentPage = 1;

            var dsLopHoc = query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            dgvLopHoc.DataSource = null;
            dgvLopHoc.DataSource = dsLopHoc;

            if (dgvLopHoc.Columns["ClassId"] != null) dgvLopHoc.Columns["ClassId"].HeaderText = "Mã Lớp";
            if (dgvLopHoc.Columns["ClassName"] != null) dgvLopHoc.Columns["ClassName"].HeaderText = "Tên Lớp";
            if (dgvLopHoc.Columns["Note"] != null) dgvLopHoc.Columns["Note"].HeaderText = "Ghi Chú";

            if (dgvLopHoc.Columns["Students"] != null) dgvLopHoc.Columns["Students"].Visible = false;

            label5.Text = $"Trang {currentPage}/{totalPages} | {totalRecords} bản ghi";
        }

        private void dgvLopHoc_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string clickedColumn = dgvLopHoc.Columns[e.ColumnIndex].Name;

            if (sortColumn == clickedColumn)
            {
                isAscending = !isAscending;
            }
            else
            {
                sortColumn = clickedColumn;
                isAscending = true;
            }

            currentPage = 1;
            LoadData();
        }

        private void dgvLopHoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvLopHoc.Rows[e.RowIndex];

                txtMaLop.Text = row.Cells["ClassId"].Value?.ToString();
                txtTenLop.Text = row.Cells["ClassName"].Value?.ToString();
                txtGhiChu.Text = row.Cells["Note"].Value?.ToString();

                txtMaLop.Enabled = false;
            }
        }

        private void dgvLopHoc_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void btn_add_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaLop.Text) || string.IsNullOrEmpty(txtTenLop.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã lớp và Tên lớp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (db.Classes.Any(c => c.ClassId == txtMaLop.Text))
            {
                MessageBox.Show("Mã lớp này đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Class lop = new Class
            {
                ClassId = txtMaLop.Text,
                ClassName = txtTenLop.Text,
                Note = txtGhiChu.Text
            };

            try
            {
                db.Classes.InsertOnSubmit(lop);
                db.SubmitChanges();
                MessageBox.Show("Thêm lớp học thành công!", "Thành công");
                btn_clear_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaLop.Text))
            {
                MessageBox.Show("Vui lòng chọn một lớp học trong bảng để sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var lop = db.Classes.SingleOrDefault(c => c.ClassId == txtMaLop.Text);

            if (lop != null)
            {
                lop.ClassName = txtTenLop.Text;
                lop.Note = txtGhiChu.Text;

                try
                {
                    db.SubmitChanges();
                    MessageBox.Show("Cập nhật thông tin lớp học thành công!", "Thành công");
                    btn_clear_Click(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi cập nhật: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaLop.Text))
            {
                MessageBox.Show("Vui lòng chọn một lớp học trong bảng để xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var lop = db.Classes.SingleOrDefault(c => c.ClassId == txtMaLop.Text);

            if (lop != null)
            {
                DialogResult dr = MessageBox.Show($"Bạn có chắc chắn muốn xóa lớp {lop.ClassName} không? \nLƯU Ý: Xóa lớp học sẽ xóa TẤT CẢ sinh viên đang thuộc lớp này!", "Cảnh báo nguy hiểm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        db.Classes.DeleteOnSubmit(lop);
                        db.SubmitChanges();
                        MessageBox.Show("Xóa lớp học thành công!", "Thành công");
                        btn_clear_Click(sender, e);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            txtMaLop.Enabled = true;
            txtMaLop.Clear();
            txtTenLop.Clear();
            txtGhiChu.Clear();
            txtTimKiem.Clear();

            searchKeyword = "";
            currentPage = 1;
            sortColumn = "ClassId";
            isAscending = true;

            LoadData();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            searchKeyword = txtTimKiem.Text.Trim();
            currentPage = 1;
            LoadData();
        }

        private void btn_head_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadData();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadData();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadData();
            }
        }

        private void btn_tail_Click(object sender, EventArgs e)
        {
            currentPage = totalPages;
            LoadData();
        }
    }
}