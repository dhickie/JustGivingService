using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration.Install;
using System.ComponentModel;
using System.ServiceProcess;

namespace JustGivingService
{
    /// <summary>
    /// This class manages installing the windows service when InstallUtil is run on the compiled executable.
    /// </summary>
    [RunInstaller(true)]
    public class JustGivingServiceInstaller : Installer
    {
        private ServiceProcessInstaller processInstaller;
        private ServiceInstaller serviceInstaller;

        public JustGivingServiceInstaller()
        {
            processInstaller = new ServiceProcessInstaller();
            serviceInstaller = new ServiceInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.StartType = ServiceStartMode.Manual;
            serviceInstaller.ServiceName = "JustGivingService";
            serviceInstaller.Description = "Retrieves fundraiser page information for outputting to external applications.";

            Installers.Add(serviceInstaller);
            Installers.Add(processInstaller);
        }
    }
}
