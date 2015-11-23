using System;
using System.Collections.Generic;
using System.Threading;
using JustGivingService.Routers;
using JustGivingService.Controllers;
using System.IO;

namespace JustGivingService
{
    public class Service
    {
        public static void Main()
        {
            // Set the current directory to the executable's directory so that we can use relative paths
            // no matter where this is run from.
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            // Initialise the threads
            InitialiseThreads();

            // Use an event handle to stop this thread from ending without consuming CPU time
            EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
            waitHandle.WaitOne();
        }

        private static void InitialiseThreads()
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
    }
}