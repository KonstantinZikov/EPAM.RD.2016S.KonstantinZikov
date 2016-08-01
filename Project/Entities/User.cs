using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Entities
{   
    [Serializable]
    [DataContract]
    public class User
    {
        public User()
        {
            this.VisaRecords = new List<VisaRecord>();
        }

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public DateTime DateOfBirth { get; set; }
        [DataMember]
        public string PersonalId { get; set; }
        [DataMember]
        public Gender Gender { get; set; }
        [DataMember]
        public List<VisaRecord> VisaRecords { get; private set; }

        public override bool Equals(object obj)
        {
            var comparing = obj as User;
            if (comparing == null)
            {
                return false;
            }

            return this.Equals(comparing);
        }

        public bool Equals(User user)
        {
            if (user == null)
            {
                return false;
            }

            if (this.FirstName == user.FirstName && this.LastName == user.LastName)
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            var firstPart = 0;
            var secondPart = 0;
            if (this.FirstName != null)
            {
                firstPart = this.FirstName.GetHashCode();
            }
                
            if (this.LastName != null)
            {
                secondPart = this.LastName.GetHashCode();
            }
                
            return firstPart ^ secondPart;
        }

        public override string ToString()
        {
            return this.FirstName + " " + this.LastName;
        }
    }
}
