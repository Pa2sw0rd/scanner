using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Windows.Forms;
using MS17010Test;
using System.Net;
using MySql.Data.MySqlClient;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private Thread scan_thread = null;
        delegate void MyDelegage_setListView(ListViewItem item);
        public delegate void MyDelegage_setLog(String text,int type=0);
        delegate void MyDelegage_updateButton(Boolean status = false);
        delegate void MyDelegage_updateProcessbar(int value);


        MyDelegage_setListView myDeleg_setList;
        public MyDelegage_setLog myDeleg_setLog;
        MyDelegage_updateButton myDeleg_updateButton;
        MyDelegage_updateProcessbar myDeleg_updateProcessbar;

        private void setListView(ListViewItem item)
        {
            this.listView1.BeginUpdate();
            //this.listBox1.Items.Add(item);
            this.listView1.Items.Add(item);
            this.listView1.EndUpdate();
        }
        public void setLOG(string text,int type=0)
        {
            switch (type)
            {
                case 0: { this.richTextBox1.SelectionColor = Color.Green; } break;
                case 1: { this.richTextBox1.SelectionColor = Color.Red; } break;
                case 2: { this.richTextBox1.SelectionColor = Color.Pink; } break;
                case 3: { this.richTextBox1.SelectionColor = Color.Blue; } break;
                case 4: { this.richTextBox1.SelectionColor = Color.Yellow; } break;
                case 5: { this.richTextBox1.SelectionColor = Color.Violet; } break;
                default:break;
                   
            }
            this.richTextBox1.AppendText(text + "\r\n");           
            //this.textBox4.AppendText(text + "\r\n");
        }
        private void updateButton(Boolean status = false)
        {

            if (status == true) this.button1.Text = "停止";
            else this.button1.Text = "开始扫描";

        }
        private void updateProcessbar(int value)
        {

            this.toolStripProgressBar1.Value = value;
            //Console.WriteLine(value);

        }

        private myScanner scan = new myScanner();
        private myTools tools = new myTools();
        private IPSegment ip = null;
        private IEnumerable<UInt32> ips = null;
        public Form1()
        {
            InitializeComponent();
            init_listview();
        }

        private void init_listview()
        {
            this.listView1.View = View.Details;
            this.listView1.Columns.Add("地址", 120, HorizontalAlignment.Center); //一步添加
            this.listView1.Columns.Add("端口", 120, HorizontalAlignment.Center); //一步添加
            //this.listView1.Columns.Add("描述", 500, HorizontalAlignment.Center); //一步添加
        }



        private void button1_Click(object sender, EventArgs e)

        {
            /*
            string str = Console.ReadLine();

            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "nmap/nmap.exe";
            p.StartInfo.Arguments = this.textBox1.Text;
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = true;//不显示程序窗口
            p.Start();//启动程序

            //向cmd窗口发送输入信息
            //p.StandardInput.WriteLine(str + "&exit");
            //p.StandardInput.WriteLine(this.textBox1.Text + "&exit");

            p.StandardInput.AutoFlush = true;
            //p.StandardInput.WriteLine("exit");
            //向标准输入写入要执行的命令。这里使用&是批处理命令的符号，表示前面一个命令不管是否执行成功都执行后面(exit)命令，如果不执行exit命令，后面调用ReadToEnd()方法会假死
            //同类的符号还有&&和||前者表示必须前一个命令执行成功才会执行后面的命令，后者表示必须前一个命令执行失败才会执行后面的命令



            //获取cmd窗口的输出信息
            string output = p.StandardOutput.ReadToEnd();

            //StreamReader reader = p.StandardOutput;
            //string line=reader.ReadLine();
            //while (!reader.EndOfStream)
            //{
            //    str += line + "  ";
            //    line = reader.ReadLine();
            //}

            p.WaitForExit();//等待程序执行完退出进程
            p.Close();

            this.textBox4.AppendText(output);
            Console.WriteLine(output);















            return;
            this.listView1.Items.Clear();
            myDeleg = this.setListBox;
            
            this.analyzeIPtoThread();
            
            foreach (var i in ip.Hosts())
            {
                Console.WriteLine(i.ToIpString());
            }
            //byte a=0b10000001;

            //if (!tools.checkIP(this.textBox1.Text)) MessageBox.Show("IP不合法\n");
            //this.listBox1.Items.Add(tools.IpRangeToList(this.textBox1.Text, this.textBox2.Text));
            //this.listBox1.Items = tools.IpRangeToList(this.textBox1.Text, this.textBox2.Text);

            //this.userControl11.Text;
            //Thread process = new Thread(new ThreadStart(this.scan_process));
            //process.Start();
            */
            //ParameterizedThreadStart pts_scanprocess = new ParameterizedThreadStart(scan_process);
            //Thread thread_scanProcess = new Thread(pts_scanprocess);
            //thread_scanProcess.Start()

            

            


            try
            {
                if (this.button1.Text.Equals("开始扫描"))
                {
                    if (this.listView1.Items.Count != 0)
                    {
                        DialogResult MsgBoxResult;//设置对话框的返回值
                        MsgBoxResult = System.Windows.Forms.MessageBox.Show("是否清除右侧扫描结果？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);//定义对话框的按钮式样
                        if (MsgBoxResult.ToString() == "Yes")//如果对话框的返回值是YES（按"Y"按钮）
                        {
                            this.listView1.Items.Clear();
                        }
                        if (MsgBoxResult.ToString() == "No")//如果对话框的返回值是NO（按"N"按钮）
                        {
                            //选择了No，继续
                        }
                    }
                    this.scan_thread = new Thread(new ThreadStart(this.scan_process));
                    this.button1.Text = "停止";
                    scan_thread.Start();
                }
                else if (this.button1.Text.Equals("停止"))
                {
                    this.scan_thread.Abort();
                    this.setLOG("【" + DateTime.Now.ToLocalTime().ToString() + "】   被用户强制中断扫描！",2);
                    this.button1.Text = "开始扫描";
                }
            }
            catch (Exception)
            {
                
                
            }
            
            

        }




        /*
        private void analyzeIPtoThread()
        {
            int allNum = ips.Count();
            int threadNum = int.Parse(this.textBox2.Text);           
            if (allNum < threadNum)//线程数大于目标数
            {
                foreach (var ip in this.ips)
                {
                    if (!tools.checkAlive(ip.ToIpString())) break;
                    ParameterizedThreadStart start = new ParameterizedThreadStart(scan_process);
                    Thread thread = new Thread(start);
                    thread.Start(ip.ToIpString());
                }
            }
            else{

            }
        }
        */
        private void scan_process()
        {

            myDeleg_setList = this.setListView;
            myDeleg_setLog = this.setLOG;
            myDeleg_updateButton = this.updateButton;
            myDeleg_updateProcessbar = this.updateProcessbar;
            try
            {
                this.ip = new IPSegment(this.textBox1.Text);
                this.ips = this.ip.Hosts();
            }
            catch (Exception)
            {

                this.BeginInvoke(myDeleg_setLog, "【" + DateTime.Now.ToLocalTime().ToString() + "】   扫描出错，输入目标格式错误，请使用单个IP地址或IP/CIDR形式！",1);
                this.BeginInvoke(myDeleg_updateButton, false);
                return;
            }
            
            string str = this.textBox3.Text;
            string[] sArray = str.Split(new char[] { ',' });
            List<string> openPorts = new List<string> { };
            this.BeginInvoke(myDeleg_setLog, "【"+DateTime.Now.ToLocalTime().ToString()+"】   开始扫描，共" +ips.Count().ToString()+"个目标。",2);
            this.BeginInvoke(myDeleg_updateButton,true);
            float all_num = (float)ips.Count();
            
            float a = 0;
            foreach (var ip in this.ips){
                a++;
                this.BeginInvoke(myDeleg_updateProcessbar,(int)((a/all_num)*100));
                //Console.WriteLine((a / all_num)*100);
                if (!tools.checkAlive(ip.ToIpString())) {
                    this.BeginInvoke(myDeleg_setLog, "【" + DateTime.Now.ToLocalTime().ToString() + "】   主机" + ip.ToIpString() + "不在线。",0);
                    continue;
                }
                foreach (string i in sArray)
                {
                    //String res = scan.scanOpenPort(ip.ToIpString(), int.Parse(i));
                    if (scan.scanOpenPort(ip.ToIpString(), int.Parse(i)))
                    {
                        //openPorts.Add(i);
                        //Console.WriteLine(i.ToString() + "开放");
                        ListViewItem lvi = new ListViewItem();
                        lvi.Text = ip.ToIpString();
                        lvi.SubItems.Add(i.ToString());
                        //lvi.SubItems.Add((string)res);
                        this.BeginInvoke(myDeleg_setList, lvi);
                        this.BeginInvoke(myDeleg_setLog, "【" + DateTime.Now.ToLocalTime().ToString() + "】   主机" + ip.ToIpString() + "开放"+i.ToString()+"端口！",4);
                    } //this.listBox1.Items.Add(i.ToString()+"开放");    
                }
            }
            this.BeginInvoke(myDeleg_updateButton, false);
            this.BeginInvoke(myDeleg_setLog, "【" + DateTime.Now.ToLocalTime().ToString() + "】   扫描结束，共计"+this.listView1.Items.Count.ToString()+"个结果！",2);
            /*
            foreach (String i in openPorts)
            {
                temp += i+",";
            }
            this.BeginInvoke(myDeleg, temp);
            */
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.listView1.SelectedItems != null && e.Button== MouseButtons.Right)
            {
                Poc_class poc = new Poc_class();
                //System.Windows.Forms.ContextMenuStrip menu = new System.Windows.Forms.ContextMenuStrip(this.components);
                //menu.SuspendLayout();
                if (this.listView1.SelectedItems.Count == 1)
                {
                    int allNum = this.contextMenuStrip1.Items.Count;
                    for (int a = 0; a <allNum ; a++)
                    {
                        if (this.contextMenuStrip1.Items[a].Name.IndexOf("plug")==-1) {
                            this.contextMenuStrip1.Items.Remove(this.contextMenuStrip1.Items[a]);
                            a--;
                            allNum--;
                        }
                        else if(this.contextMenuStrip1.Items[a].Name.IndexOf("plug|")!=-1)
                        {
                            Console.WriteLine("aaaaa"+this.contextMenuStrip1.Items[a].Name);
                            int port = int.Parse(this.contextMenuStrip1.Items[a].Name.Split('|')[1]);
                            if (int.Parse(this.listView1.SelectedItems[0].SubItems[1].Text) != port)
                            {
                                this.contextMenuStrip1.Items.Remove(this.contextMenuStrip1.Items[a]);
                                a--;
                                allNum--;
                            }
                        }
                    }

                    /*
                    if (this.contextMenuStrip1.Items.Count != 0)
                    {
                        foreach (System.Windows.Forms.ToolStripMenuItem item in this.contextMenuStrip1.Items)
                        {
                            if (item.Name != "all") this.contextMenuStrip1.Items.Remove(item);
                        }
                    }*/
                    
                    if (poc.canBoom(int.Parse(this.listView1.SelectedItems[0].SubItems[1].Text)))
                    {
                        //this.contextMenuStrip1.Size = new System.Drawing.Size(101, 22);
                        //this.contextMenuStrip1.Items.Clear();
                        System.Windows.Forms.ToolStripMenuItem boomMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                        boomMenuItem.Text = "爆破";
                        boomMenuItem.Size= new System.Drawing.Size(100, 22);
                        boomMenuItem.Click += (a, b) => boom_Click(this.listView1.SelectedItems[0].SubItems[0].Text,int.Parse(this.listView1.SelectedItems[0].SubItems[1].Text));
                        this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {boomMenuItem });
                        this.contextMenuStrip1.Show(this.listView1,e.Location);
                    }
                    if (this.listView1.SelectedItems[0].SubItems[1].Text == 445.ToString())//MS17-010、CVE-2020-0796
                    {
                        //this.contextMenuStrip1.Items.Clear();
                        


                        System.Windows.Forms.ToolStripMenuItem blueMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                        System.Windows.Forms.ToolStripMenuItem blackMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                        blueMenuItem.Text = "MS17-010检测";
                        //blueMenuItem.Size = new System.Drawing.Size(100, 22);
                        blueMenuItem.Click += (a, b) => startMs17010(this.listView1.SelectedItems[0].SubItems[0].Text, int.Parse(this.listView1.SelectedItems[0].SubItems[1].Text));
                        

                        blackMenuItem.Text = "CVE-2020-0796检测";
                        //blackMenuItem.Size = new System.Drawing.Size(100, 22);
                        blackMenuItem.Click += (a, b) => startCVE0796(this.listView1.SelectedItems[0].SubItems[0].Text, int.Parse(this.listView1.SelectedItems[0].SubItems[1].Text));

                        this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {blueMenuItem, blackMenuItem });
                        this.contextMenuStrip1.Show(this.listView1, e.Location);
                    }
                }else if(this.listView1.SelectedItems.Count > 1)//选择项大于一就遍历
                {
                    foreach (ListViewItem value in this.listView1.SelectedItems)
                    {
                        Console.WriteLine(value.SubItems[1]);
                    }
                }
                
                //Console.WriteLine(this.listView1.SelectedItems[0].SubItems[1]);

                //this.contextMenuStrip1.Show(this.listView1,e.Location);
            }
        }

        
        private void startCVE0796(String ip,int port=445)
        {

            Thread thread = new Thread(new ParameterizedThreadStart((a) => checkCVE0796(ip, port)));
            thread.Start();
        }


        private void startMs17010(String ip, int port = 445)
        {
            Thread thread = new Thread(new ParameterizedThreadStart((a)=>checkMS17010(ip,port)));
            thread.Start();
        }

        private void checkCVE0796(String ip,int port=445)
        {
            Poc_class poc = new Poc_class();
            if (poc.checkBlack(ip,port)) this.BeginInvoke(myDeleg_setLog, "【" + DateTime.Now.ToLocalTime().ToString() + "】   " + ip + "存在CVE-2020-0796漏洞！", 1);
            else this.BeginInvoke(myDeleg_setLog, "【" + DateTime.Now.ToLocalTime().ToString() + "】   " + ip + "不存在CVE-2020-0796漏洞！", 3);
        }

        private void checkMS17010(String ip,int port = 445)
        {
            Tester ms17 = new Tester();
            TestResult res = ms17.TestIP(ip,port);
            if(res.IsVulnerable) this.BeginInvoke(myDeleg_setLog, "【" + DateTime.Now.ToLocalTime().ToString() + "】   "+ip + "存在MS17-010漏洞！系统："+res.OSName+"；版本："+res.OSBuild, 1);
            else this.BeginInvoke(myDeleg_setLog, "【" + DateTime.Now.ToLocalTime().ToString() + "】   " + ip + "不存在MS17-010漏洞！", 3);
        }



        private void boom_Click(String ip,int port)
        {
            Form win_boom= new Form2(ip,port);
            
            win_boom.Show(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {


            //String a = "plug|445";
            //int port = int.Parse(a.Split('|')[1]);
            //Console.WriteLine(port);

            //return;
            Form win_config = new Form_plug();

            win_config.Show(this);
            return;




            Form test = new Form_plug();
            test.Show();



            return;

            Form win_boom = new Form2("127.0.0.1", 21);

            win_boom.Show(this);
            //Form temp = new Form2();
            // temp.Owner = this;
            //temp.Show();
        }


        private String exec_path = null;
        private String arg = null;

        public void attacks(String exec_path,String arg)
        {
            this.exec_path = exec_path;
            this.arg = arg;
            Console.WriteLine(arg);
            Thread thread = new Thread(new ThreadStart(attacksProcess));
            thread.Start();


        }
        private void attacksProcess()
        {

            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = this.exec_path;



            String args = this.arg.Replace("$IP", this.listView1.SelectedItems[0].SubItems[0].Text);
            args = args.Replace("$PORT", this.listView1.SelectedItems[0].SubItems[1].Text);

            Console.WriteLine("arg:" + args);


            p.StartInfo.Arguments = args;
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = true;//不显示程序窗口
            p.Start();//启动程序

            //向cmd窗口发送输入信息
            //p.StandardInput.WriteLine(str + "&exit");
            //p.StandardInput.WriteLine(this.textBox1.Text + "&exit");

            p.StandardInput.AutoFlush = true;
            //p.StandardInput.WriteLine("exit");
            //向标准输入写入要执行的命令。这里使用&是批处理命令的符号，表示前面一个命令不管是否执行成功都执行后面(exit)命令，如果不执行exit命令，后面调用ReadToEnd()方法会假死
            //同类的符号还有&&和||前者表示必须前一个命令执行成功才会执行后面的命令，后者表示必须前一个命令执行失败才会执行后面的命令



            //获取cmd窗口的输出信息
            string output = p.StandardOutput.ReadToEnd();

            //StreamReader reader = p.StandardOutput;
            //string line=reader.ReadLine();
            //while (!reader.EndOfStream)
            //{
            //    str += line + "  ";
            //    line = reader.ReadLine();
            //}

            p.WaitForExit();//等待程序执行完退出进程
            p.Close();
            this.BeginInvoke(this.myDeleg_setLog, "☆☆☆☆脚本程序执行结果☆☆☆☆☆", 5);
            //this.BeginInvoke(myDeleg_setlog, "☆☆☆☆外部程序执行结果☆☆☆☆☆", 5);
            this.BeginInvoke(this.myDeleg_setLog, output, 5);
            this.BeginInvoke(this.myDeleg_setLog, "☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆", 5);

            //this.f1.setLOG("☆☆☆☆外部程序执行结果☆☆☆☆☆", 4);
            //this.f1.setLOG(output, 4);
            //this.f1.setLOG("☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆", 4);
            //this.textBox4.AppendText(output);
            //Console.WriteLine(output);
        }


        private void saveLOG()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "(*.txt)|*.txt|(*.*)|*.*";
            saveFileDialog.FileName = "扫描日志" + DateTime.Now.ToString("yyyyMMddHHmm") + ".txt";
            //将日期时间作为文件名
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName, true);
                streamWriter.Write(this.richTextBox1.Text);
                streamWriter.Close();
            }
        }


        private void richTextBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                saveLOG();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.comboBox1.SelectedIndex = 0;
        }
    }
}
