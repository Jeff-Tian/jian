using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Jian
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
            LoadSetting();
        }

        private void LoadSetting()
        {
            dataGridView1.DataSource = JianWrapper.LoadFromFile().ToDataTable(true);
            dataGridView1.Refresh();
        }

        private void 保存SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            JianWrapper.GetJian().SaveToFile();
        }

        private void FormSettings_FormClosed(object sender, FormClosedEventArgs e)
        {
            Dispose();
        }

        private void 加一列AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var name = Microsoft.VisualBasic.Interaction.InputBox("请输入列标题", "请输入");

            dataGridView1.Columns.Add(name, name);
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 2) return;

            var newName = Microsoft.VisualBasic.Interaction.InputBox("请输入新的标题名", "请输入");

            dataGridView1.Columns[e.ColumnIndex].Name = newName;
            dataGridView1.Columns[e.ColumnIndex].HeaderText = newName;
        }
    }
}
