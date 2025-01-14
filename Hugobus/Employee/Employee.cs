﻿using BUS;
using DTO;
using GUI.Employee.TaiXe;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using ClosedXML.Excel;




namespace GUI.Employee
{
    public partial class Employee : Form
    {
        EmployeeDriverBus service = new EmployeeDriverBus();
        EmployeeNannyBUS service2 = new EmployeeNannyBUS();
        public Employee()
        {
            InitializeComponent();
            dataGridViewTaiXe.DataSource = service.getDuLieu();
            dataGridViewBaoMau.DataSource = service2.getDuLieu();
        }

        private void content(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewTaiXe.Columns[e.ColumnIndex] is DataGridViewImageColumn && dataGridViewTaiXe.Columns[e.ColumnIndex].Name == "ColumnDelete")
            {
                int row = e.RowIndex;
                int idtaixe = int.Parse(dataGridViewTaiXe.Rows[row].Cells["ColumnId"].Value.ToString());
                if (service.xoaTaiXeById(idtaixe) == true)
                {
                    MessageBox.Show("Dữ liệu đã được xoá thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dataGridViewTaiXe.DataSource = service.getDuLieu();
                }
                else
                {
                    MessageBox.Show("Dữ liệu xoá không thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }

            if (dataGridViewTaiXe.Columns[e.ColumnIndex] is DataGridViewImageColumn && dataGridViewTaiXe.Columns[e.ColumnIndex].Name == "ColumnChiTiet")
            {
                int row = e.RowIndex;
                int idtaixe = int.Parse(dataGridViewTaiXe.Rows[row].Cells["ColumnId"].Value.ToString());
                Driver d = service.getTextfield(idtaixe);
                if (d != null)
                {
                    Detail new_form = new Detail(d);
                    new_form.ShowDialog();
                }
                dataGridViewTaiXe.DataSource = service.getDuLieu();
            }
            if (dataGridViewTaiXe.Columns[e.ColumnIndex] is DataGridViewImageColumn && dataGridViewTaiXe.Columns[e.ColumnIndex].Name == "ColumnIn")
            {
                int row = e.RowIndex;
                int idtaixe = int.Parse(dataGridViewTaiXe.Rows[row].Cells["ColumnId"].Value.ToString());
                DateTime currentDate = DateTime.Now;
                string formattedDate = currentDate.ToString("yyyy-MM-dd");
                DataTable dt = service.getLich(idtaixe, formattedDate);
                string folderpath = String.Format("D:\\Detainhom18\\Driver\\driver{0}.xlsx", idtaixe);

                Microsoft.Office.Interop.Excel.Application objexcelapp = new Microsoft.Office.Interop.Excel.Application();
                objexcelapp.Application.Workbooks.Add(Type.Missing);

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    objexcelapp.Cells[1, i + 1] = dt.Columns[i].ColumnName; // Điều chỉnh chỉ số và truy cập cột
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Rows[i][j] != null)
                        {
                            objexcelapp.Cells[i + 2, j + 1] = dt.Rows[i][j].ToString(); // Điều chỉnh chỉ số và truy cập hàng, cột
                        }
                    }
                }

                objexcelapp.Columns.AutoFit();

                objexcelapp.ActiveWorkbook.SaveCopyAs(folderpath);
                objexcelapp.ActiveWorkbook.Saved = true;
                objexcelapp.Quit();

                // Giải phóng tài nguyên
                System.Runtime.InteropServices.Marshal.ReleaseComObject(objexcelapp);



            }


        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            ADD new_form = new ADD();
            new_form.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridViewTaiXe.DataSource = service.getDuLieu();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            BaoMau.ADD new_form = new BaoMau.ADD();
            new_form.ShowDialog();
        }

        private void contentclick2(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewBaoMau.Columns[e.ColumnIndex] is DataGridViewImageColumn && dataGridViewBaoMau.Columns[e.ColumnIndex].Name == "cotxoa")
            {
                int row = e.RowIndex;
                int idbaomau = int.Parse(dataGridViewBaoMau.Rows[row].Cells["cotid"].Value.ToString());
                if (service2.xoaNannyById(idbaomau) == true)
                {
                    MessageBox.Show("Dữ liệu đã được xoá thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dataGridViewBaoMau.DataSource = service2.getDuLieu();
                }
                else
                {
                    MessageBox.Show("Dữ liệu xoá không thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }

            if (dataGridViewBaoMau.Columns[e.ColumnIndex] is DataGridViewImageColumn && dataGridViewBaoMau.Columns[e.ColumnIndex].Name == "cotsua")
            {
                int row = e.RowIndex;
                int idbaomau = int.Parse(dataGridViewBaoMau.Rows[row].Cells["cotid"].Value.ToString());
                Nanny d = service2.getTextfield(idbaomau);
                if (d != null)
                {
                    BaoMau.anh.Detail new_form = new BaoMau.anh.Detail(d);
                    new_form.ShowDialog();
                }
                dataGridViewBaoMau.DataSource = service2.getDuLieu();
            }
            if (dataGridViewBaoMau.Columns[e.ColumnIndex] is DataGridViewImageColumn && dataGridViewBaoMau.Columns[e.ColumnIndex].Name == "cotin")
            {
                int row = e.RowIndex;
                int idbaomau = int.Parse(dataGridViewBaoMau.Rows[row].Cells["cotid"].Value.ToString());
                DateTime currentDate = DateTime.Now;
                string formattedDate = currentDate.ToString("yyyy-MM-dd");
                DataTable dt = service2.getLich(idbaomau, formattedDate);
                string folderpath = String.Format("D:\\Detainhom18\\Nanny\\driver{0}.xlsx", idbaomau);

                Microsoft.Office.Interop.Excel.Application objexcelapp = new Microsoft.Office.Interop.Excel.Application();
                objexcelapp.Application.Workbooks.Add(Type.Missing);

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    objexcelapp.Cells[1, i + 1] = dt.Columns[i].ColumnName; // Điều chỉnh chỉ số và truy cập cột
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Rows[i][j] != null)
                        {
                            objexcelapp.Cells[i + 2, j + 1] = dt.Rows[i][j].ToString(); // Điều chỉnh chỉ số và truy cập hàng, cột
                        }
                    }
                }

                objexcelapp.Columns.AutoFit();

                objexcelapp.ActiveWorkbook.SaveCopyAs(folderpath);
                objexcelapp.ActiveWorkbook.Saved = true;
                objexcelapp.Quit();

                // Giải phóng tài nguyên
                System.Runtime.InteropServices.Marshal.ReleaseComObject(objexcelapp);
            }

        }

        private void buttonTimkiem_Click(object sender, EventArgs e)
        {
            string select = comboBoxTimKiem.SelectedItem.ToString();
            string textbox = textBoxTimKiem.Text;
            if (!string.IsNullOrEmpty(select) && !string.IsNullOrEmpty(textbox))
            {
                dataGridViewTaiXe.DataSource = service.getSearch(select, textbox);
            }
        }

        private void buttonTimKiemBM_Click(object sender, EventArgs e)
        {
            string select = comboBoxTimKiemBaoMau.SelectedItem.ToString();
            string textbox = textBoxTimKiemBM.Text;
            if (!string.IsNullOrEmpty(select) && !string.IsNullOrEmpty(textbox))
            {
                dataGridViewBaoMau.DataSource = service2.getSearch(select, textbox);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridViewBaoMau.DataSource = service2.getDuLieu();

        }
    }
}
