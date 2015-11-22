using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustGivingService.Model
{
    /// <summary>
    /// This class represents a JustGiving fundraiser.
    /// </summary>
    public class Fundraiser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal? FundingTarget { get; set; }
        public string CurrentFunding { get; set; }
        public string CurrencySymbol { get; set; }
        public DateTime EndDate { get; set; }
        public List<Donation> Donations { get; set; }

        // Lock for whenever the fundraiser being written to or read from
        public object ReadWriteLock { get; }

        public Fundraiser(string id)
        {
            Id = id;
            Name = String.Empty;
            FundingTarget = 0;
            CurrentFunding = String.Empty;
            CurrencySymbol = String.Empty;
            EndDate = DateTime.MaxValue;
            Donations = new List<Donation>();

            ReadWriteLock = new object();
        }

        public Fundraiser(string id, 
            string name, 
            decimal? fundingTarget, 
            string currentFunding, 
            string currencySymbol, 
            DateTime endDate, 
            List<Donation> donations)
        {
            Id = id;
            Name = name;
            FundingTarget = fundingTarget;
            CurrentFunding = currentFunding;
            CurrencySymbol = currencySymbol;
            EndDate = endDate;
            Donations = donations;

            ReadWriteLock = new object();
        }

        public void CopyDetails(Fundraiser newFundraiser)
        {
            // Check that these are actually the same fundraising page
            if (Id == newFundraiser.Id)
            {
                Name = newFundraiser.Name;
                FundingTarget = newFundraiser.FundingTarget;
                CurrentFunding = newFundraiser.CurrentFunding;
                CurrencySymbol = newFundraiser.CurrencySymbol;
                EndDate = newFundraiser.EndDate;
                Donations = newFundraiser.Donations;
            }
            else
            {
                throw new InvalidOperationException("Invalid fundraiser ID - ID of the new page must match the existing ID.");
            }
        }

        public override bool Equals(object obj)
        {
            Fundraiser fundraiser = (Fundraiser)obj;
            bool same = true;

            if (Id != fundraiser.Id)
            {
                same = false;
            }
            if (String.Compare(Name, fundraiser.Name) != 0)
            {
                same = false;
            }
            if (FundingTarget != fundraiser.FundingTarget)
            {
                same = false;
            }
            if (CurrentFunding != fundraiser.CurrentFunding)
            {
                same = false;
            }
            if (CurrencySymbol != fundraiser.CurrentFunding)
            {
                same = false;
            }
            if (DateTime.Compare(EndDate, fundraiser.EndDate) != 0)
            {
                same = false;
            }
            if (Donations.Count == fundraiser.Donations.Count)
            {
                for (int i = 0; i < Donations.Count; i++)
                {
                    if (!Donations[i].Equals(fundraiser.Donations[i]))
                    {
                        same = false;
                    }
                }
            }

            return same;
        }
    }
}
