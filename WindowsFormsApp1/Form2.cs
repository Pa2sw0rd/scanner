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
using ThreadManager;
using Task = System.Threading.Tasks.Task;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        private String file_user_path = null;
        private String file_pass_path = null;
        private Poc_class poc = new Poc_class();
        Form1 f1 = null;

        delegate void MyDelegage_SetLog(String data,int type=0);


        MyDelegage_SetLog myDeleg_setlog;
        public Form2(String ip,int port)
        {
            InitializeComponent();
            //Control.CheckForIllegalCrossThreadCalls = false;
            this.textBox1.Text = ip;
            this.comboBox1.Text = port.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //this.openFileDialog1.ShowDialog();
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;//该值确定是否可以选择多个文件
            dialog.Title = "请选择用户名字典文件";
            dialog.Filter = "文本文件(*.txt*)|*.txt*";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.file_user_path = dialog.FileName;
            }
            else
            {
                MessageBox.Show("未选中txt文件");
                return;
            }
            string temp = string.Empty;
            FileStream fs = new FileStream(this.file_user_path, FileMode.Open);
            StreamReader reader = new StreamReader(fs, UnicodeEncoding.GetEncoding("UTF-8"));
            while ((temp = reader.ReadLine()) != null)
            {
                temp = temp.Trim().ToString();
                this.textBox3.AppendText(temp+"\r\n");
            }
            fs.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;//该值确定是否可以选择多个文件
            dialog.Title = "请选择密码字典文件";
            dialog.Filter = "文本文件(*.txt*)|*.txt*";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.file_pass_path = dialog.FileName;
            }
            else
            {
                MessageBox.Show("未选中txt文件");
                return;
            }
            string temp = string.Empty;
            FileStream fs = new FileStream(this.file_pass_path, FileMode.Open);
            StreamReader reader = new StreamReader(fs, UnicodeEncoding.GetEncoding("UTF-8"));
            while ((temp = reader.ReadLine()) != null)
            {
                temp = temp.Trim().ToString();
                this.textBox4.AppendText(temp + "\r\n");
            }
            fs.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            /*
             6*20=120
            thread=17
            120/15=7-----5
             */

            this.f1 = (Form1)this.Owner;//将本窗体的拥有者强制设为Form1类的实例f1
            int thread_num = int.Parse(this.textBox2.Text);
            int user_num = this.textBox3.Lines.Length;
            int pass_num = this.textBox4.Lines.Length;

            ThreadPool.SetMaxThreads(thread_num, thread_num);
            myDeleg_setlog = f1.setLOG;
            String ip = this.textBox1.Text;
            String port = this.comboBox1.Text;
            this.f1.setLOG("【" + DateTime.Now.ToLocalTime().ToString() + "】   开始爆破-" + ip + ":" + this.comboBox1.Text);


            //Thread thread = new Thread(new ParameterizedThreadStart(start_boom));
            //thread.Start((object)thread_num);


            
            for (int user = 0; user < user_num; user++)//开线程
            {
                if (this.textBox3.Lines[user].Equals("") || this.textBox3.Lines[user] == null) continue;
                for (int pass = 0; pass < pass_num; pass++)
                {

                    ThreadPool.QueueUserWorkItem(new WaitCallback(boom_process), ip + "," +this.comboBox1.Text+","+ this.textBox3.Lines[user] + "," + this.textBox4.Lines[pass]);
                    //task = new Task(()=>this.boom_process(ip + "," + this.comboBox1.Text + "," + this.textBox3.Lines[user] + "," + this.textBox4.Lines[pass]));
                    
                   // my_threadPool.AddTask(task);
                }

            }

            /*
            switch (this.comboBox1.Text)
            {
                case "21": {
                       
                    }
                    break;
                case "22": {
                        this.f1.setLOG("【" + DateTime.Now.ToLocalTime().ToString() + "】   开始爆破-" + ip + ":" + this.comboBox1.Text);
                        for (int user = 0; user < user_num; user++)//开线程
                        {
                            if (this.textBox3.Lines[user].Equals("") || this.textBox3.Lines[user] == null) continue;
                            for (int pass = 0; pass < pass_num; pass++)
                            {
                                ThreadPool.QueueUserWorkItem(new WaitCallback(boom_process), ip + ","+this. + this.textBox3.Lines[user] + "," + this.textBox4.Lines[pass]);

                            }

                        }
                    }
                    break;
                default: { }break;
            }
            //Console.WriteLine(poc.boom_ftp(ip: "127.0.0.1"));
            */

        }
        private void start_boom(Object threadNum)
        {
            ThreadPoolManager my_threadPool = new ThreadPoolManager((int)threadNum);
            Task task = null;
            String ip=null, port=null;
            this.Invoke((EventHandler)(delegate
            {
                // 这里写你的控件bai代码，比如
                //str= target.SelectedText;
                ip = this.textBox1.Text;
                port = this.comboBox1.Text;
            }
));
            foreach (String user in this.textBox3.Lines)
            {
                if (user.Equals("") || user == null) continue;
                foreach (String pass in this.textBox4.Lines)
                {
                    task = new Task(() => this.boom_process(ip + "," + port + "," + user + "," + pass));
                    my_threadPool.AddTask(task);
                }
            }
            
            //my_threadPool.CloseThread();
        }
        private void boom_process(object data)
        {
            string[] sArray =data.ToString().Split(new char[] { ',' });
            switch (sArray[1])
            {
                case "21": {
                        if (this.poc.boom_ftp(ip: sArray[0], user: sArray[2], pass: sArray[3])) {
                           // Console.WriteLine("【" + DateTime.Now.ToLocalTime().ToString() + "】   检测到弱口令-" + sArray[0] + "-" + sArray[1] + "-" + sArray[2] + "-" + sArray[3]);
                            this.BeginInvoke(myDeleg_setlog, "【" + DateTime.Now.ToLocalTime().ToString() + "】   检测到弱口令-" + sArray[0] + "-" + sArray[1] + "-" + sArray[2] + "-" + sArray[3], 1);
                        }
                    }break;
                case "22": {
                        //Console.WriteLine(data.ToString());
                        if (this.poc.boom_ssh(ip: sArray[0],port: int.Parse(sArray[1]),user: sArray[2], pass: sArray[3])) this.BeginInvoke(myDeleg_setlog, "【" + DateTime.Now.ToLocalTime().ToString() + "】   检测到弱口令-" + sArray[0] + "-"+ sArray[1] + "-" + sArray[2] + "-" + sArray[3], 1);
                    }break;
                case "3389":
                    {
                        //Console.WriteLine(data.ToString());
                        if (this.poc.boom_rdp(ip: sArray[0], port: int.Parse(sArray[1]), user: sArray[2], pass: sArray[3])) this.BeginInvoke(myDeleg_setlog, "【" + DateTime.Now.ToLocalTime().ToString() + "】   检测到弱口令-" + sArray[0] + "-" + sArray[1] + "-" + sArray[2] + "-" + sArray[3], 1);
                    }
                    break;
                case "3306":
                    {
                        //Console.WriteLine(data.ToString());
                        if (this.poc.boom_mysql(ip: sArray[0], port: int.Parse(sArray[1]), user: sArray[2], pass: sArray[3])) this.BeginInvoke(myDeleg_setlog, "【" + DateTime.Now.ToLocalTime().ToString() + "】   检测到弱口令-" + sArray[0] + "-" + sArray[1] + "-" + sArray[2] + "-" + sArray[3], 1);
                    }
                    break;
                default:break;
            }
        }

    }
}
