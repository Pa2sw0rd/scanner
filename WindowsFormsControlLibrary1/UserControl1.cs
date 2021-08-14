using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsControlLibrary1
{
    public partial class UserControl1: UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
            //this.initSize();
        }

        private void initSize()//初始化位置坐标信息
        {
            Size box_size = new Size(this.textBox1.Width, this.textBox1.Height);
            Size point_size = new Size(this.label1.Width,this.label1.Height);
            Point pos = new Point(1, 1);
            this.textBox1.Location = pos;
            this.textBox1.Size = box_size;

            this.label1.Location = new Point(pos.X+box_size.Width+1,pos.Y);

            this.textBox2.Location = new Point(pos.X + box_size.Width + point_size.Width, pos.Y);

            this.label2.Location = new Point(pos.X + 2*box_size.Width+10, pos.Y);

            this.textBox3.Location = new Point(pos.X + 2*box_size.Width + 2*point_size.Width, pos.Y);

            this.label3.Location = new Point(pos.X + 3 * box_size.Width+20, 2*pos.Y);

            this.textBox4.Location = new Point(pos.X + 3 * box_size.Width + 3 * point_size.Width, pos.Y);


        }
        public override string Text { 
            get {
                return this.textBox1.Text.ToString() + "." + this.textBox2.Text.ToString() + "." + this.textBox3.Text.ToString() + "." + this.textBox4.Text.ToString();
            }
            set { 
            } }
        private void winResize(object sender,EventArgs e)
        {
            
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
