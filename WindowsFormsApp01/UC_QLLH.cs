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

        // Biến Phân trang & Tìm kiếm
        private int pageSize = 10;
        private int currentPage = 1;
        private int totalPages = 1;
        private int totalRecords = 0;
        private string searchKeyword = "";

        // Biến Sắp xếp (Sort)
        private string sortColumn = "ClassId";
        private bool isAscending = true;

        public UC_QLLH()
        {
            InitializeComponent();
        }

        private void UC_QLLH_Load(object sender, EventArgs e)
        {
            try
            {
                // Gọi hàm làm đẹp giao diện
                FormatUI();

                dgvLopHoc.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvLopHoc.ReadOnly = true;

                // Tự động kết nối các sự kiện cho nút bấm để không phải chỉnh trong Designer
                btn_edit.Click += btn_edit_Click;
                btn_delete.Click += btn_delete_Click;
                btn_clear.Click += btn_clear_Click;
                btn_search.Click += btn_search_Click;

                // Nối sự kiện phân trang
                button6.Click += btn_head_Click;
                button7.Click += button7_Click;
                button8.Click += button8_Click;
                button9.Click += btn_tail_Click;

                // Nối sự kiện click bảng và sắp xếp
                dgvLopHoc.CellClick += dgvLopHoc_CellClick;
                dgvLopHoc.ColumnHeaderMouseClick += dgvLopHoc_ColumnHeaderMouseClick;

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải giao diện: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =========================================================
        // CODE LÀM ĐẸP GIAO DIỆN (UI/UX)
        // =========================================================
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

        // =========================================================
        // TẢI DỮ LIỆU, TÌM KIẾM, SẮP XẾP, PHÂN TRANG
        // =========================================================
        public void LoadData()
        {
            var query = db.Classes.AsQueryable();

            // 1. LỌC: Tìm kiếm theo Mã lớp hoặc Tên lớp
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                query = query.Where(c => c.ClassId.Contains(searchKeyword) ||
                                         c.ClassName.Contains(searchKeyword));
            }

            // 2. SẮP XẾP
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

            // 3. PHÂN TRANG
            totalRecords = query.Count();
            totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (totalPages == 0) totalPages = 1;
            if (currentPage > totalPages) currentPage = totalPages;
            if (currentPage < 1) currentPage = 1;

            var dsLopHoc = query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            dgvLopHoc.DataSource = dsLopHoc;

            label5.Text = $"Trang {currentPage}/{totalPages} | {totalRecords} bản ghi";
        }

        // =========================================================
        // SỰ KIỆN CLICK SẮP XẾP VÀ CHỌN LỚP
        // =========================================================
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

                txtMaLop.Enabled = false; // Khóa mã lớp khi đang chọn để sửa/xóa
            }
        }

        private void dgvLopHoc_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        // =========================================================
        // CRUD: THÊM, SỬA, XÓA, LÀM MỚI
        // =========================================================
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

        // =========================================================
        // TÌM KIẾM & CHUYỂN TRANG
        // =========================================================
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