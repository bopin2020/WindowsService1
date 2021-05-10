using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsService1
{
    /// <summary>
    /// Reference from https://www.cnblogs.com/yinrq/p/5587464.html
    /// </summary>
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 执行服务事件   一般采用线程方式执行     便于隔一段时间执行一回
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            Thread t = new Thread(() =>
            {
                var context = "[+] Service1: Service Started \t" + DateTime.Now;
                WriteLogs($"{AppDomain.CurrentDomain.BaseDirectory}start.log", context);
            });
            t.Start();
            t.Join();
        }

        /// <summary>
        /// 关闭服务
        /// </summary>
        protected override void OnStop()
        {
            // 不能使用线程池
            Thread t = new Thread(() =>
            {
                Handle();
            });
            t.Start();
            t.Join();
        }

        private void Handle()
        {
            try
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + "service.log";
                var context = "[+] Service1: Service Stopped \t" + DateTime.Now;
                WriteLogs(path, context);
            }
            catch (Exception e)
            {
                WriteLogs($"{AppDomain.CurrentDomain.BaseDirectory} + error.log", e.Message.ToString());
            }
        }
        private void WriteLogs(string path, string context)
        {
            var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.BaseStream.Seek(0, SeekOrigin.End);
                sw.WriteLine(context);
            }
            fs.Close();
        }
    }
}
