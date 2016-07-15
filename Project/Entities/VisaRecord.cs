using System;

namespace Entities
{
    [Serializable]
    public struct VisaRecord
    {
        public string Country { get; set; }
        public DateTime Starts { get; set; }
        public DateTime Ends { get; set; }
    }
}
