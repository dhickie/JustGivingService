using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustGivingService.Model
{
    /// <summary>
    /// This class represents a donation to a JustGiving fundraiser.
    /// </summary>
    public class Donation
    {
        public DateTime? Date { get; set; }
        public string Name { get; set; }
        public Decimal? Value { get; set; }

        public Donation (DateTime? date, string name, Decimal? value)
        {
            Date = date;
            Name = name;
            Value = value;
        }

        public override bool Equals(object obj)
        {
            Donation donation = (Donation)obj;
            bool same = true;

            if (Date != null && donation.Date != null)
            {
                DateTime date1 = (DateTime)Date;
                DateTime date2 = (DateTime)donation.Date;
                if (DateTime.Compare(date1, date2) != 0)
                {
                    same = false;
                }
            }
            else
            {
                if (Date == null ^ donation.Date == null)
                {
                    same = false;
                }
            }

            if (same)
            {
                if (String.Compare(Name, donation.Name) != 0)
                {
                    same = false;
                }
            }
            
            if (same)
            {
                if (Value != donation.Value)
                {
                    same = false;
                }
            }

            return same;
        }
    }
}
