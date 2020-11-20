using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;

namespace MySqlWindowsService
{
    public partial class Service1 : ServiceBase
    {
        private Process mySqlProcess = null;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            WriteLog("MySqlWindowsService  Service Start");
            StartMysqlService();
        }

        protected override void OnStop()
        {
        }
        private void StartMysqlService(Object sender = null, EventArgs e = null)
        {
            WriteLog("mySqlProcess  service start ");
            mySqlProcess = StartProcess(AppDomain.CurrentDomain.BaseDirectory + "baseDir\\mysql-8.0.13-winx64\\mysql_init_db.bat ");
        }
        private static Process StartProcess(string command)
        {
            var processInfo = new ProcessStartInfo("cmd.exe", "/c " + command)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                Verb = "runas"
            };

            return Process.Start(processInfo);
        }
        private static void KillProcessAndChildren(int pid)
        {
            using (var searcher = new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" + pid))
            {
                var managementObjects = searcher.Get();

                foreach (var obj in managementObjects)
                {
                    var managementObject = (ManagementObject)obj;
                    KillProcessAndChildren(Convert.ToInt32(managementObject["ProcessID"]));
                }

                try
                {
                    var proc = Process.GetProcessById(pid);
                    proc.Kill();
                }
                catch (ArgumentException)
                {
                    // Process already exited.
                }
            }
        }
        private void CloseZookeeper()
        {
            WriteLog("Kill mySqlProcess  service");
            if (null == mySqlProcess)
                return;
            mySqlProcess.Kill();
            mySqlProcess.Close();
            mySqlProcess = null;
        }
        private void WriteLog(string logStr, bool wTime = true)
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\ZookeeperAutoService.log", true))
            {
                string timeStr = wTime == true ? DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss ") : "";
                sw.WriteLine(timeStr + logStr);
            }
        }
    }
}
