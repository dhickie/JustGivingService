using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JustGivingService.Model;
using JustGivingService.DataOutputters;

namespace JustGivingService.Controllers
{
    /// <summary>
    /// Controller class responsible for outputting fundraiser data to the correct data outputter.
    /// </summary>
    public class DataOutputController
    {
        // The singleton instance
        private static DataOutputController instance;
        private static object instanceLock = new object();

        // Member variables
        private Dictionary<int, Fundraiser> m_lastSent;
        private DataOutputter m_outputter;

        private DataOutputController()
        {
            m_lastSent = new Dictionary<int, Fundraiser>();
            m_outputter = new RainmeterOutputter();
        }

        public static DataOutputController Instance
        {
            get
            {
                // Do a double-check lock to make sure two threads don't try to initialise
                // at the same time.
                if (instance == null)
                {
                    lock (instanceLock)
                    {
                        if (instance == null)
                        {
                            instance = new DataOutputController();
                        }
                    }
                }
                return instance;
            }
        }

        public static bool SendFundraiserData(int threadId, Fundraiser fundraiser)
        {
            return Instance.SendFundraiserDataImpl(threadId, fundraiser);
        }

        private bool SendFundraiserDataImpl(int threadId, Fundraiser fundraiser)
        {
            bool result = true;
            bool sendData = true;
            String pageId = fundraiser.Id;

            // Check to see if we've sent the data for this fundraiser in the past,
            // if we have then don't bother sending it if it hasn't changed
            if (m_lastSent.ContainsKey(threadId))
            {
                if (m_lastSent[threadId].Equals(fundraiser))
                {
                    sendData = false;
                }
            }

            if (sendData)
            {
                // Lock the fundraiser before sending it - we don't want the storage controller changing it
                // when it's in the middle of being dispatched to output.
                lock (fundraiser.ReadWriteLock)
                {
                    result = m_outputter.SendFundraiser(fundraiser);
                }

                if (result)
                {
                    Fundraiser lastSent = new Fundraiser(pageId);
                    lastSent.CopyDetails(fundraiser);
                    if (m_lastSent.ContainsKey(threadId))
                    {
                        m_lastSent[threadId] = lastSent;
                    }
                    else
                    {
                        m_lastSent.Add(threadId, lastSent);
                    }
                }
            }

            return result;
        }
    }
}
