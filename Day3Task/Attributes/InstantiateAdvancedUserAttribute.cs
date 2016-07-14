using System;

namespace Attributes
{
    // Should be applied to assembly only.
    [AttributeUsage(AttributeTargets.Assembly)]
    public class InstantiateAdvancedUserAttribute : InstantiateUserAttribute
    {
        public int ExternalId { get; set; }

        public InstantiateAdvancedUserAttribute(int id, string firstName, string lastName, int externalId)
            : base(id, firstName, lastName)
        {
            ExternalId = externalId;
        }
    }
}
