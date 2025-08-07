using System.ServiceProcess;

namespace ResguardoAppService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // Lógica de respaldo programado
            // Podés usar un Timer, leer tu archivo de configuración, etc.
        }

        protected override void OnStop()
        {
            // Lógica de limpieza/parada
        }
    }
}

