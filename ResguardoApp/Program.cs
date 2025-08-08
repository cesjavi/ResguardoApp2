using SharedLib;
using System.ServiceProcess;

namespace ResguardoApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Environment.UserInteractive)
            {
                // Run as a normal application
                ApplicationConfiguration.Initialize();
                Application.Run(new MainForm());
            }
            else
            {
                // Run as a service
                using (var service = new ResguardoService())
                {
                    ServiceBase.Run(service);
                }
            }
        }
    }
}
