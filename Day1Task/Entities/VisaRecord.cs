using System;

namespace Entities
{
    public struct VisaRecord
    {
        public VisaRecord(string country, DateTime starts, DateTime ends)
        {
            Country = country;
            Starts = starts;
            Ends = ends;
        }

        public string Country { get; private set; }
        public DateTime Starts { get; private set; }
        public DateTime Ends { get; private set; }
    }
}
