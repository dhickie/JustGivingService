using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JustGivingService.Model;

namespace JustGivingService.DataOutputters
{
    /// <summary>
    /// Abstract class encapsulating all functionality required of data outputters.
    /// </summary>
    public abstract class DataOutputter
    {
        /// <summary>
        /// Sends fundraiser data to an external application
        /// </summary>
        /// <param name="fundraiser">The fundraiser to send</param>
        /// <returns>Whether the operation succeeded.</returns>
        public abstract bool SendFundraiser(Fundraiser fundraiser);
    }
}
