using System;
using System.ComponentModel.DataAnnotations;

namespace PartnerUser.Api.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class NotNullOrEmptyGuidAttribute : ValidationAttribute
    {
        public NotNullOrEmptyGuidAttribute() : base ("The {0} field requires a non-empty guid.")
        {
            
        }

        public override bool IsValid(object value)
        {
            if (value is null)
            {
                return false;
            }

            switch (value)
            {
                case Guid guid:
                    return guid != Guid.Empty;
                default:
                    return true;
            }
        }
    }
}
