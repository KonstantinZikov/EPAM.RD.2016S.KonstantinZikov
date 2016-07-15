using System;
using System.Collections.Generic;

namespace Entities
{
    [Serializable]
    public class User
    {
        public User()
        {
            VisaRecords = new List<VisaRecord>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PersonalId { get; set; }
        public Gender Gender { get; set; }
        public List<VisaRecord> VisaRecords { get; private set; }

        public override bool Equals(object obj)
        {
            var comparing = obj as User;
            if (comparing == null)
            {
                return false;
            }
            return Equals(comparing);
        }

        public bool Equals(User user)
        {
            if (user == null)
            {
                return false;
            }
            if (FirstName == user.FirstName &&
                LastName == user.LastName)
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            var firstPart = 0;
            var secondPart = 0;
            if (FirstName != null)
                firstPart = FirstName.GetHashCode();
            if (LastName != null)
                secondPart = LastName.GetHashCode();
            return firstPart ^ secondPart;
        }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }
}
