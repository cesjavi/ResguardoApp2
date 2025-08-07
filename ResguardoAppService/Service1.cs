using System.ServiceProcess;
using System.Diagnostics;

namespace ResguardoAppService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
            if (!EventLog.SourceExists("ResguardoAppService"))
            {
                EventLog.CreateEventSource("ResguardoAppService", "Application");
            }
        }

        protected override void OnStart(string[] args)
        {
            EventLog.WriteEntry("ResguardoAppService", "Servicio iniciado.", EventLogEntryType.Information);
            // Lógica de respaldo programado

            // Podés usar un Timer, leer tu archivo de configuración, etc.
        }

        protected override void OnStop()
        {
                EventLog.WriteEntry("ResguardoAppService", "Servicio detenido.", EventLogEntryType.Information);

            // Lógica de limpieza/parada
        }
    }
}

