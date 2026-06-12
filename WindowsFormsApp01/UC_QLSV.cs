using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp01
{
    public partial class UC_QLSV : UserControl
    {
        QLsinhvienDataContext db = new QLsinhvienDataContext();

        private int pageSize = 10;
        private int currentPage = 1;
        private int totalPages = 1;
        private int totalRecords = 0;
        private string searchKeyword = "";

        public UC_QLSV()
        {
            InitializeComponent();
        }

        private void UC_QLSV_Load(object sender, EventArgs e)
        {
            try
            {
                dgvSinhVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvSinhVien.ReadOnly = true;

                LoadLopHoc();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải giao diện: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadData()
        {
            var query = db.Students.AsQueryable();

            if (!string.IsNullOrEmpty(searchKeyword))
            {
                query = query.Where(s => s.MSSV.Contains(searchKeyword) ||
                                         s.FullName.Contains(searchKeyword) ||
                                         s.ClassId.Contains(searchKeyword));
            }

            totalRecords = query.Count();
            totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (totalPages == 0) totalPages = 1;
            if (currentPage > totalPages) currentPage = totalPages;
            if (currentPage < 1) currentPage = 1;

            var dsSinhVien = query.OrderBy(s => s.MSSV).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            dgvSinhVien.DataSource = dsSinhVien;

            if (dgvSinhVien.Columns["Class"] != null)
            {
                dgvSinhVien.Columns["Class"].Visible = false;
            }

            label4.Text = $"Trang {currentPage}/{totalPages} | {totalRecords} bản ghi";
        }

        public void LoadLopHoc()
        {
            cbxLopHoc.DataSource = db.Classes.ToList();
            cbxLopHoc.DisplayMember = "ClassName";
            cbxLopHoc.ValueMember = "ClassId";
        }

        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSinhVien.Rows[e.RowIndex];

                txt_mssv.Text = row.Cells["MSSV"].Value?.ToString();
                txt_name.Text = row.Cells["FullName"].Value?.ToString();
                cboGioiTinh.Text = row.Cells["Gender"].Value?.ToString();

                if (row.Cells["DateOfBirth"].Value != null)
                {
                    dtpNgaySinh.Value = Convert.ToDateTime(row.Cells["DateOfBirth"].Value);
                }

                cbxLopHoc.SelectedValue = row.Cells["ClassId"].Value?.ToString();
                txt_mssv.Enabled = false;
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_mssv.Text))
            {
                MessageBox.Show("Vui lòng nhập MSSV!", "Cảnh báo");
                return;
            }

            if (db.Students.Any(s => s.MSSV == txt_mssv.Text))
            {
                MessageBox.Show("Mã số sinh viên này đã tồn tại!", "Lỗi");
                return;
            }

            Student sv = new Student
            {
                MSSV = txt_mssv.Text,
                FullName = txt_name.Text,
                Gender = cboGioiTinh.Text,
                DateOfBirth = dtpNgaySinh.Value,
                ClassId = cbxLopHoc.SelectedValue?.ToString()
            };

            try
            {
                db.Students.InsertOnSubmit(sv);
                db.SubmitChanges();
                MessageBox.Show("Thêm thành công!");
                btn_clear_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        // =========================================================
        // ĐỂ TRỐNG HÀM SỬA VÀ XÓA (Giữ nguyên tên hàm để không bị lỗi Form Design)
        // =========================================================
        private void btn_edit_Click(object sender, EventArgs e)
        {
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
        }
        // =========================================================

        private void btn_clear_Click(object sender, EventArgs e)
        {
            txt_mssv.Enabled = true;
            txt_mssv.Clear();
            txt_name.Clear();
            cboGioiTinh.SelectedIndex = -1;
            textBox1.Clear();

            searchKeyword = "";
            currentPage = 1;
            LoadData();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            searchKeyword = textBox1.Text.Trim();
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

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}