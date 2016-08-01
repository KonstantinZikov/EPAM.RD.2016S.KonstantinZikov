using System;
using System.Runtime.Serialization;

namespace Entities
{
    [DataContract]
    [Serializable]
    public struct VisaRecord
    {
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public DateTime Starts { get; set; }
        [DataMember]
        public DateTime Ends { get; set; }
    }
}
