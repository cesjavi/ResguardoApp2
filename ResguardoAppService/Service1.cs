using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

using System.ServiceProcess;

namespace ResguardoAppService
{
    public class Program
    {
        public static void Main()
        {
            ServiceBase.Run(new ResguardoService());
        }
    }

    public class ResguardoService : ServiceBase
    {
        public ResguardoService()
        {
            this.ServiceName = "ResguardoAppService";
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
