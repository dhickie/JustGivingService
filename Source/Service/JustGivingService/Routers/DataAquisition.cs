using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JustGiving.Api.Sdk;
using JustGiving.Api.Sdk.Model.Page;
using JustGivingService.Model;
using JustGivingService.Controllers;
using System.Threading;

namespace JustGivingService.Routers
{
    /// <summary>
    /// Class responsible for getting data for a particular fundraising page and routing it to the DataStorageController.
    /// </summary>
    public static class DataAquisition
    {
        private static int MAX_RECENT_DONATIONS = 4;

        // Entry point for threads which periodically poll for a fundraiser's details.
        public static void EntryPoint(object threadId)
        {
            int threadIdInt = (int)threadId;

            // Create the API client
            JustGivingClient client = new JustGivingClient("9cce4750");

            // Enter the loop which polls the API periodically
            while (true)
            {
                // Get the page ID from the config controller based on this thread's thread ID
                String pageId = ConfigController.GetPageIds()[threadIdInt];
                int pollingPeriod = ConfigController.GetApiPollingPeriod();

                // Get the page
                FundraisingPage page = client.Page.Retrieve(pageId);

                if (page != null)
                {
                    // Get the information we need and put it in to our model
                    string id = pageId;
                    string name = page.PageTitle;
                    decimal? fundingTarget = page.TargetAmount;
                    string currentFunding = page.TotalRaised;
                    string currencySymbol = page.CurrencySymbol;
                    DateTime endDate = page.PageExpiryDate;
                    List<Model.Donation> recentDonations = new List<Model.Donation>();

                    // Get the donations with a separate API call
                    FundraisingPageDonations pageDonations = client.Page.RetrieveDonationsForPage(pageId);
                    List<FundraisingPageDonation> donations = pageDonations.Donations;
                    for (int i = 0; i < donations.Count && i < MAX_RECENT_DONATIONS; i++)
                    {
                        FundraisingPageDonation donation = donations[i];

                        DateTime? date = donation.DonationDate;
                        string donator = donation.DonorDisplayName;
                        decimal? value = donation.Amount;

                        Model.Donation newDonation = new Model.Donation(date, donator, value);
                        recentDonations.Add(newDonation);
                    }

                    Fundraiser fundraiser = new Fundraiser(id, 
                        name, 
                        fundingTarget, 
                        currentFunding, 
                        currencySymbol, 
                        endDate, 
                        recentDonations);

                    // Check to see whether the storage controller currently has this fundraiser stored
                    Fundraiser existingFundraiser = DataStorageController.GetFundraiser(pageId);
                    if (existingFundraiser == null)
                    {
                        // It doesn't exist yet, so just add it
                        DataStorageController.AddFundraiser(fundraiser);
                    }
                    else
                    {
                        // There's an existing item, lock it and then copy over the new details
                        lock (existingFundraiser.ReadWriteLock)
                        {
                            existingFundraiser.CopyDetails(fundraiser);
                        }
                    }
                }

                Thread.Sleep(pollingPeriod);
            }
        }
    }
}
