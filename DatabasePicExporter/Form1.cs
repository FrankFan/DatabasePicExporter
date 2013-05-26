using DatabasePicExporter.DPE.Entity;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DatabasePicExporter
{
    public partial class Form1 : Form
    {
        private string imagePath = string.Empty;

        public Form1()
        {
            InitializeComponent();
            this.MaximizeBox = false;
        }

        private void btbExe_Click(object sender, EventArgs e)
        {
            this.btnExe.Enabled = false;
            this.txtStatus.Text = string.Empty;
            this.txtDetail.Text = string.Empty;
            txtStatus.Text = string.Format("正在对 {0} 进行操作...",txtDb.Text);
            txtStatus.Refresh();

            imagePath = Application.StartupPath + "\\" + this.txtDb.Text;

            //get connection string
            Dbinfo.ConnString = GetConnStr();

            //get tables and columns
            List<Table> tables = OperateDatabase();
            
            //generate pic from db
            GeneratePicFromTable(tables);

        }

        /// <summary>
        /// open file path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenIt_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Application.StartupPath);
        }

        /// <summary>
        /// 根据界面参数拼connStr
        /// </summary>
        /// <returns></returns>
        private string GetConnStr()
        {
            string connStr = string.Empty;

            connStr = "server="+txtServer.Text.Trim();
            connStr += ";uid=" + txtUid.Text.Trim();
            connStr += ";pwd=" + txtPwd.Text.Trim();
            connStr += ";database=" + txtDb.Text.Trim();

            return connStr;
        }

        private List<Table> OperateDatabase()
        {
            List<Table> tables;
            try
            {
                tables = Dbinfo.GetTables();
            }
            catch (Exception ex)
            {
                txtStatus.Text = "";
                txtDetail.Text = ex.Message;
                btnExe.Enabled = true;
                return null;
            }

            txtStatus.Text = string.Format("执行结束。完成对 {0} 中的 {1} 个表的操作。", txtDb.Text, tables.Count);
            btnExe.Enabled = true;

            return tables;
        }

        private void GeneratePicFromTable(List<Table> tables)
        {
            if (tables != null && tables.Count > 0)
            {
                for (int i = 0; i < tables.Count; i++)
                {
                    //draw pic
                    Image.DrawPhoto(txtDb.Text.Trim(), tables[i], imagePath, i + 1);

                    txtDetail.Text += string.Format("------{0}({1})\r\n", tables[i].TableName, tables[i].Description);

                    foreach (Column column in tables[i].Columns)
                        txtDetail.Text += string.Format("{0}\t{1}\t{2}\r\n", column.ColumnName, column.Type, column.Description);

                    txtDetail.Text += "\r\n";
                }
            }
            else
            {
                MessageBox.Show("导出数据出错.");
                return;
            }
        }
    }
}
