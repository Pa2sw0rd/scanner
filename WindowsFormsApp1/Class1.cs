using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.Net;
using Renci.SshNet;
using FreeRDP.Core;
using System.Threading;
using System.Runtime.InteropServices;
using MS17010Test;
using MySql.Data.MySqlClient;
namespace WindowsFormsApp1
{
    class myScanner
    {
        public Boolean scanOpenPort(String ip,int port)
        {
            
            TcpClient client = null;
            try
            {
                client = new TcpClient(ip, port);
                /*
                NetworkStream netStream = client.GetStream();
                string msg = "";
                int bytesRead;
                //netStream.Write(System.Text.Encoding.Default.GetBytes("hello"),0,4) ;
                //netStream.WriteByte(0xab);
                netStream.ReadTimeout = 1;
                netStream.Read(buffer, 0, buffer.Length); msg += Encoding.Unicode.GetString(buffer, 0, buffer.Length);
                
                netStream.Close();
                */
                client.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
                //throw;
            }
            return false;
        }
        public Boolean boomSSH(String ip,String username,String pass,int port= 22)
        {
            return false;
        }
    }
    class myTools
    {
        public Boolean checkIP(String ip)
        {

            int index = ip.IndexOf("/");
            if (index != -1)
            {
                string yanma = ip.Substring(index + 1);
                try
                {
                    if (int.Parse(yanma) > 32 || int.Parse(yanma) < 0) return false;
                }
                catch (Exception)
                {
                    return false;
                }
                ip = ip.Remove(index);
            }
            try
            {
                string[] sArray = ip.Split(new char[] { '.' });
                if (sArray.Length != 4) return false;
                foreach (string i in sArray)
                {
                    if (int.Parse(i) < 0 || int.Parse(i) > 255) return false;
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
            return false;
        }
        public bool checkAlive(string ip)
        {
            try
            {
                Ping ping = new Ping();
                PingReply pingReply = ping.Send(ip);
                if (pingReply.Status == IPStatus.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }
            
        }
        
    }
    class MyException : Exception
    {
        public MyException(string message) : base(message)
        {
        }
    }
    class IPSegment
    {

        private UInt32 _ip;
        private UInt32 _mask;
        private bool flag = false;
        public IPSegment(string data)
        {

            int index = data.IndexOf("/");
            if (index == -1)
            {
                try
                {
                    string[] sArray = data.Split(new char[] { '.' });
                    if (sArray.Length != 4) throw new MyException("");
                    foreach (string i in sArray)
                    {
                        if (int.Parse(i) < 0 || int.Parse(i) > 255) throw new MyException("");
                    }
                    this.flag = true;
                    this._ip = data.ParseIp();
                }
                catch (Exception)
                {

                    throw new MyException("");
                }
            }
            else
            {
                try
                {
                    String mask = data.Substring(index + 1);
                    uint test = ~(0xffffffff >> Convert.ToInt32(mask));
                    byte[] bytes = BitConverter.GetBytes(test);
                    Array.Reverse(bytes);
                    String m = bytes[0].ToString() + "." + bytes[1].ToString() + "." + bytes[2].ToString() + "." + bytes[3].ToString();
                    _ip = data.Remove(index).ParseIp();
                    _mask = m.ParseIp();
                }
                catch (Exception)
                {

                    throw;
                }
                
            }
            

            
        }

        public UInt32 NumberOfHosts
        {
            get { return ~_mask + 1; }
        }

        public UInt32 NetworkAddress
        {
            get { return _ip & _mask; }
        }

        public UInt32 BroadcastAddress
        {
            get { return NetworkAddress + ~_mask; }
        }

        public IEnumerable<UInt32> Hosts()
        {
            if (flag) {

                yield return this._ip;
            }
            else
            {
                for (var host = NetworkAddress + 1; host < BroadcastAddress; host++)
                {
                    yield return host;
                }
            }
            
        }

    }

    public static class IpHelpers
    {
        public static string ToIpString(this UInt32 value)
        {
            var bitmask = 0xff000000;
            var parts = new string[4];
            for (var i = 0; i < 4; i++)
            {
                var masked = (value & bitmask) >> ((3 - i) * 8);
                bitmask >>= 8;
                parts[i] = masked.ToString(CultureInfo.InvariantCulture);
            }
            return String.Join(".", parts);
        }

        public static UInt32 ParseIp(this string ipAddress)
        {
            var splitted = ipAddress.Split('.');
            UInt32 ip = 0;
            for (var i = 0; i < 4; i++)
            {
                ip = (ip << 8) + UInt32.Parse(splitted[i]);
            }
            return ip;
        }
    }
    public class Poc_class
    {
        private List<int> boom_ports = new List<int> {21,22,23,3389,3306,100 };
        private String type = null;
        private static object _lock = new object();

        public Boolean checkBlue(String ip,int port=445)
        {

            return false;
        }
        public Boolean checkBlack(String ip, int port = 445)
        {
            try
            {
                string smb = "\x00\x00\x00\xc0\xfeSMB@\x00\x00\x00\x00\x00\x00\x00\x00\x00\x1f\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00$\x00\x08\x00\x01\x00\x00\x00\x7f\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00x\x00\x00\x00\x02\x00\x00\x00\x02\x02\x10\x02\"\x02$\x02\x00\x03\x02\x03\x10\x03\x11\x03\x00\x00\x00\x00\x01\x00 &\x00\x00\x00\x00\x00\x01\x00 \x00\x01\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x03\x00\n\x00\x00\x00\x00\x00\x01\x00\x00\x00\x01\x00\x00\x00\x01\x00\x00\x00\x00\x00\x00\x00";

                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.ReceiveTimeout = 5;
                client.SendTimeout = 5;

                IPAddress ipAddress = IPAddress.Parse(ip);
                client.Connect(new IPEndPoint(ipAddress, port));
                byte[] smb_b = new byte[smb.Length];
                for (int i = 0; i < smb.Length; i++)
                {
                    smb_b[i] = (byte)smb[i];
                }
                //Console.WriteLine(smb);
                client.Send(smb_b);
                byte[] temp = new byte[4];

                var receivedBytes = client.Receive(temp);
                String temp_str = BitConverter.ToString(temp);
                temp_str = temp_str.Replace("-", "");
                //Console.WriteLine(temp_str);
                //Console.WriteLine(Convert.ToInt32(temp_str, 16));
                //Console.WriteLine($"Received bytes: {BitConverter.ToString(temp)}");

                byte[] smb_last = new byte[Convert.ToInt32(temp_str, 16)];
                client.Receive(smb_last);
                //Console.WriteLine(BitConverter.ToString(smb_last));
                //Console.WriteLine("68-70:" + BitConverter.ToString(smb_last.Skip(67).Take(2).ToArray()) + ";70-72:" + BitConverter.ToString(smb_last.Skip(69).Take(2).ToArray()));
                client.Close();
                if (BitConverter.ToString(smb_last.Skip(67).Take(2).ToArray()).Equals("11-03") || BitConverter.ToString(smb_last.Skip(69).Take(2).ToArray()).Equals("02-00")) return false;//存在
                else return true;


                
            }
            catch (Exception ex)
            {

                return false;
            }






            return false;
        }




        public Boolean canBoom(int port)
        {
            return boom_ports.Exists(t => t == port);
        }
        public Boolean boom_ftp(String ip,String user= "anonymous", String pass="",int port = 21)
        {
            try
            {

                //Console.WriteLine(user + ";" + pass);
                FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ip + "/"));
                ftp.Method = WebRequestMethods.Ftp.ListDirectory;
                // 指定数据传输类型
                ftp.UseBinary = true;
                ftp.Credentials = new NetworkCredential(user, pass);
                FtpWebResponse wr = (FtpWebResponse)ftp.GetResponse();
                
                
                return true;
            }
            catch (Exception)
            {

                return false;
            }
            
            return false;
        }
        public Boolean boom_ssh(String ip,String user,String pass,int port = 22)
        {
            try
            {
                var client = new SshClient(ip,port, user, pass);
                client.Connect();
                client.Disconnect();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
            return false;
        }
        unsafe public Boolean boom_rdp(String ip,String user,String pass,int port = 3389)
        {
            
            try
            {
                lock(_lock){
                    RDP rdp = new RDP();
                    rdp.Connect(ip, "", user, pass, port);
                    rdp.Disconnect();
                }
                
                return true;
            }
            catch (Exception)
            {

                return false;
            }
            return false;
        }

        public Boolean boom_mysql(String ip,String user,String pass,int port = 3306)
        {
            try
            {
                string M_str_sqlcon = "server="+ip+";user id="+user+";password="+pass+";port="+port.ToString(); //根据自己的设置
                MySqlConnection myCon = new MySqlConnection(M_str_sqlcon);
                myCon.Open();
                myCon.Close();

                //Console.WriteLine("OK");
                return true;
            }
            catch (Exception)
            {

                //Console.WriteLine("异常！");
                return false;
            }


            return false;
        }
    }

    public class myMenuItem{
        public System.Windows.Forms.ToolStripMenuItem myMenyItem()
        {
            return null;
        }
    }

    /*
     开一个总线程管理所有线程，for依次开启任务线程，提前判断是否推出。
     */

    /*
    class myThreadPool
    {
        private int threadNum=20;
        private Thread allThread;
        private Boolean flag = false;
        private Thread[] threadList = null;
        //private List<Thread> threadList =new List<Thread>();//线程列表
        private Object[] temp = new object[] {null,null };//加入队列时的中间变量[方法，参数Object]
        private Queue<Object[]> taskList = new Queue<object[]>();//任务队列
        public myThreadPool(int threadNum)//构造方法初始化最大线程数
        {
            this.threadNum = threadNum;
            this.threadList = new Thread[this.threadNum];
        }
        public void init_task()
        {
            this.allThread = new Thread(new ThreadStart(myTask));
            this.allThread.Start();
            this.flag = true;
        }
        public void push(Action task,Object parm)
        {
            this.temp[0] = task;
            this.temp[1] = parm;
            this.taskList.Enqueue(this.temp);
        }
        private void myTask()
        {
            while (flag) {
                for (int i = 0;i < threadList.Length;i++)//检测线程状态
                {
                    if (threadList[i].ThreadState == ThreadState.Aborted)
                    {
                        threadList[i] = new Action((Action)(Object[])this.taskList.Dequeue()[0]);
                    }
                }
            }
        }
        public void stopAllTask()
        {
            Action a = new Action(myTask);
            a.BeginInvoke();
            a.
            this.flag = false;
        }
    }
    */
}




