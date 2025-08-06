using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace ResguardoApp
{
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        private ServiceProcessInstaller serviceProcessInstaller;
        private ServiceInstaller serviceInstaller;

        public ProjectInstaller()
        {
            serviceProcessInstaller = new ServiceProcessInstaller();
            serviceInstaller = new ServiceInstaller();

            // ServiceProcessInstaller
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            serviceProcessInstaller.Password = null;
            serviceProcessInstaller.Username = null;

            // ServiceInstaller
            serviceInstaller.ServiceName = "ResguardoAppService";
            serviceInstaller.DisplayName = "ResguardoApp Backup Service";
            serviceInstaller.Description = "Realiza respaldos automáticos a una hora programada.";
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            Installers.AddRange(new Installer[] {
                serviceProcessInstaller,
                serviceInstaller
            });
        }
    }
}
