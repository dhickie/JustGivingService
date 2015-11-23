using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using JustGivingService.Controllers;
using JustGivingService.Model;

namespace JustGivingService.Routers
{
    /// <summary>
    /// Class responsible for encapsulating the thread for outputting data to the DataOutputController for a particular
    /// fundraising page.
    /// </summary>
    public static class DataOutput
    {
        public static void EntryPoint(object threadId)
        {
            int threadIdInt = (int)threadId;

            while (true)
            {
                // Get the pageId from the config controller based on the thread ID
                String pageId = ConfigController.GetPageIds()[threadIdInt];
                int pollingPeriod = ConfigController.GetApiPollingPeriod();

                // Get the current data for this fundraising page
                Fundraiser fundraiser = DataStorageController.GetFundraiser(pageId);

                if (fundraiser != null)
                {
                    // Send it across to the DataOutputController
                    DataOutputController.SendFundraiserData(threadIdInt, fundraiser);
                }

                Thread.Sleep(pollingPeriod);
            }
        }
    }
}
