using System;
using Entities;

namespace Services
{
    [Serializable]
    public class Message
    {
        public MessageCode Code { get; set; }

        public User User { get; set; }
    }
}
