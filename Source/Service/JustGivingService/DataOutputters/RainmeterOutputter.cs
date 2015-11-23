using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using JustGivingService.Model;
using JustGivingService.Controllers;

namespace JustGivingService.DataOutputters
{
    /// <summary>
    /// Class responsible for outputting fundraiser data to the JustGiving rainmeter skin.
    /// Rainmeter allows variables in rainmeter skins to be set through command line arguments
    /// to the rainmeter executable.
    /// </summary>
    class RainmeterOutputter : DataOutputter
    {
        // Consts
        int MAX_PAGE_NAME_LENGTH = 49;
        int MAX_DONATOR_NAME_LENGTH = 22;

        // Member variables
        private String m_rainmeterDir;

        public RainmeterOutputter()
        {
            m_rainmeterDir = ConfigController.GetRainmeterDir();
        }

        /// <summary>
        /// Sends the supplied fundraiser data to the JustGiving rainmeter skin.
        /// </summary>
        /// <param name="fundraiser">The fundraiser to send.</param>
        /// <returns>Whether the operation succeeded.</returns>
        public override bool SendFundraiser(Fundraiser fundraiser)
        {
            bool result = true;

            // Set all the variables in the rainmeter skin for this fundraiser
            try
            {
                // The rainmeter skin can display a limited number of characters, if the name goes over this then truncate it
                String name = fundraiser.Name;
                if (name.Length > MAX_PAGE_NAME_LENGTH)
                {
                    name = name.Substring(0, MAX_PAGE_NAME_LENGTH - 3) + "...";
                }
                SetRainmeterVariable("fundRaiserName", name);

                if (fundraiser.FundingTarget != null)
                {
                    SetRainmeterVariable("fundingTarget", fundraiser.FundingTarget.ToString());
                }
                else
                {
                    SetRainmeterVariable("fundingTarget", String.Empty);
                }
                SetRainmeterVariable("currentFunding", Decimal.Parse(fundraiser.CurrentFunding).ToString("#.##"));
                SetRainmeterVariable("currencySymbol", fundraiser.CurrencySymbol);
                SetRainmeterVariable("endDate", fundraiser.EndDate.ToString("dd/MM/yyyy"));

                for (int i = 0; i < fundraiser.Donations.Count; i++)
                {
                    Donation donation = fundraiser.Donations[i];

                    String dateVarName = String.Format("donation{0}Date", i + 1);
                    String nameVarName = String.Format("donation{0}Name", i + 1);
                    String valueVarName = String.Format("donation{0}Value", i + 1);

                    if (donation.Date != null)
                    {
                        SetRainmeterVariable(dateVarName, ((DateTime)donation.Date).ToString("dd/MM/yyyy"));
                    }
                    else
                    {
                        SetRainmeterVariable(dateVarName, String.Empty);
                    }

                    // The rainmeter skin can display a limited number of characters, if the donator's name goes over this then truncate it
                    String donatorName = donation.Name;
                    if (donatorName.Length > MAX_DONATOR_NAME_LENGTH)
                    {
                        name = name.Substring(0, MAX_DONATOR_NAME_LENGTH - 3) + "...";
                    }
                    SetRainmeterVariable(nameVarName, donatorName);

                    if (donation.Value != null)
                    {
                        // The rainmeter skin requires us to add the currency symbol to the start of the donation amount.
                        Decimal value = (Decimal)donation.Value;
                        String valueString = String.Format("{0}{1}", fundraiser.CurrencySymbol, value.ToString("F"));
                        SetRainmeterVariable(valueVarName, valueString);
                    }
                    else
                    {
                        SetRainmeterVariable(valueVarName, String.Empty);
                    }
                }
            }
            catch (ExternalException e)
            {
                // There's been an error setting one of the variables.
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Sets a variable in the rainmeter skin to a new value.
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <param name="value">The new value.</param>
        private void SetRainmeterVariable(String name, String value)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = m_rainmeterDir;
            startInfo.Arguments = String.Format("!SetVariable {0} \"{1}\"", name, value);
            
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new ExternalException("An error occured setting the rainmeter variable.");
            }
        }
    }
}
