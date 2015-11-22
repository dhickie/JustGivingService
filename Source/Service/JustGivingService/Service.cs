using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Threading;
using JustGivingService.Routers;
using JustGivingService.Controllers;

namespace JustGivingService
{
    public class Service : ServiceBase
    {
        public Service()
        {
            this.ServiceName = "JustGivingService";
            this.CanStop = true;
            this.CanPauseAndContinue = false;
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            // Start up the thrift config server
            ThreadStart configServerStart = new ThreadStart(ConfigServer.EntryPoint);
            Thread configServerThread = new Thread(configServerStart);
            configServerThread.Start();

            // Get the page ID we're interested in from the config controller
            List<String> pageIds = ConfigController.GetPageIds();

            for (int i = 0; i < pageIds.Count; i++)
            {
                // Create the thread to call the JustGiving API
                ParameterizedThreadStart apiPollerStart = new ParameterizedThreadStart(DataAquisition.EntryPoint);
                Thread apiThread = new Thread(apiPollerStart);
                apiThread.Start(i);

                // Create the thread to check for fundraising data to send to the output
                ParameterizedThreadStart dataOutputStart = new ParameterizedThreadStart(DataOutput.EntryPoint);
                Thread outputThread = new Thread(dataOutputStart);
                outputThread.Start(i);
            }
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

        public static void Main()
        {
            // This is a crude way of dealing with debugging, but it would seem windows services don't play nice with
            // visual studio express edition.
#if (!DEBUG)
            ServiceBase.Run(new Service());
#else
            Service service = new Service();
            service.OnStart(null);
            while (true) ;
#endif
        }
    }
}