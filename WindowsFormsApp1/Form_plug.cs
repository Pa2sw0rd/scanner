using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form_plug : Form
    {
        delegate void MyDelegage_SetLog(String data, int type = 0);
        MyDelegage_SetLog myDeleg_setlog;


        Form1 f1 = null;
        private String exec_path = null;
        public Form_plug()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;//该值确定是否可以选择多个文件
            dialog.Title = "请选择可执行文件路径";
            dialog.Filter = "所有文件|*";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.exec_path = dialog.FileName;
            }
            else
            {
                MessageBox.Show("未选中文件");
                return;
            }
            this.textBox1.Text = dialog.FileName;
            this.exec_path = dialog.FileName;
        }

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            this.textBox4.ReadOnly = !this.checkBox1.Checked;
            /*
            if (this.checkBox1.Enabled)
            {
                this.textBox4.dis
            }*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text == "" || this.textBox2.Text == "" || this.textBox3.Text == "" || ((this.checkBox1.Checked && this.textBox4.Text == "")))
            {
                MessageBox.Show("内容不能为空！");
                return;
            }


            this.f1 = (Form1)this.Owner;//将本窗体的拥有者强制设为Form1类的实例f1
            System.Windows.Forms.ToolStripMenuItem tempItem = new System.Windows.Forms.ToolStripMenuItem();
            tempItem.Text = this.textBox2.Text;
            //blueMenuItem.Size = new System.Drawing.Size(100, 22);
            if (this.checkBox1.Checked) tempItem.Name = "plug|" + this.textBox4.Text;
            else tempItem.Name = "plug";
            String arg = this.textBox3.Text;
            Console.WriteLine("这边："+arg);
            tempItem.Click += (a, b) => this.f1.attacks(this.exec_path,arg);
            this.f1.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tempItem });
            f1.setLOG("【" + DateTime.Now.ToLocalTime().ToString() + (this.checkBox1.Checked ? "】 添加菜单：" + this.textBox2.Text + "(" + this.textBox3.Text + "):" + this.textBox4.Text : "】 添加菜单：" + this.textBox2.Text + "(" + this.textBox3.Text + ")"));
            this.Close();

        }

    }

        
}
