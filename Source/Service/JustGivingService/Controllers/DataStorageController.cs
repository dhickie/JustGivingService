using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JustGivingService.Model;

namespace JustGivingService.Controllers
{
    /// <summary>
    /// DataStorageController is a singleton class which manages storage all information pertaining to
    /// the fundraisers in question.
    /// </summary>
    public class DataStorageController
    {
        // The singleton instance
        private static DataStorageController instance;
        private static object instanceLock = new object();

        // Member variables
        private Dictionary<string, Fundraiser> m_fundraisers;

        private DataStorageController()
        {
            m_fundraisers = new Dictionary<string, Fundraiser>();
        }

        private static DataStorageController Instance
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
                            instance = new DataStorageController();
                        }
                    }
                }
                return instance;
            }
        }

        // Accessors
        public static Fundraiser GetFundraiser(string fundraiserId)
        {
            return Instance.GetFundraiserImpl(fundraiserId);
        }

        private Fundraiser GetFundraiserImpl(string fundraiserId)
        {
            Fundraiser fundraiser = null;

            if (m_fundraisers.ContainsKey(fundraiserId))
            {
                fundraiser = m_fundraisers[fundraiserId];
            }

            return fundraiser;
        }

        public static void AddFundraiser(Fundraiser newFundraiser)
        {
            Instance.AddFundraiserImpl(newFundraiser);
        }

        private void AddFundraiserImpl(Fundraiser newFundraiser)
        {
            m_fundraisers.Add(newFundraiser.Id, newFundraiser);
        }
    }
}
