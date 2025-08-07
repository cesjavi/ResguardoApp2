using System.ServiceProcess;

namespace ResguardoAppService
{
    internal static class Program
    {
        /// <summary>
        /// Entry point for the service application.
        /// </summary>
        static void Main()
        {
            ServiceBase.Run(new Service1());
        }
    }
}

