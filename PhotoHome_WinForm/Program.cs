using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoHome
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);

#if !DEBUG
            if (principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
            {
                if (SuperControls.frmRegister.VerifyLicense())
                {

                    if (args.Length > 0)
                    {
                        frmMain mainForm = new frmMain();
                        mainForm.PathFromArg = args[0];
                        Application.Run(mainForm);
                    }
                    else
                    {
                        Application.Run(new frmMain());
                    }
                }
            }
        
            else
            {
                Utils.ProgressUIQueue.ShowProgressForm("show-main-window", "正在启动"+Application.ProductName);
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.UseShellExecute = true;
                startInfo.WorkingDirectory = Environment.CurrentDirectory;
                startInfo.FileName = Application.ExecutablePath;
                if (args.Length > 0)
                    startInfo.Arguments = args[0];
                startInfo.Verb = "runas";
                try
                {
                    Utils.ProgressUIQueue.CloseProgressForm("show-main-window");
                    System.Diagnostics.Process.Start(startInfo);
                }
                catch
                {
                    return;
                }

                Application.Exit();
            }
#else
            
            if (args.Length > 0)
            {
                frmMain mainForm = new frmMain();
                mainForm.PathFromArg = args[0];
                Application.Run(mainForm);
            }
            else
            {
                Application.Run(new frmMain());
            }
#endif

            //Application.Run(new Tests.frmRectTest());
        }
    }
}
