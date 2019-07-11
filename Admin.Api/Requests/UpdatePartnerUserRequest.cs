using System;
using System.Net;
using Admin.Services.Results;

namespace Admin.Api.Requests
{
    public class UpdatePartnerUserRequest
    {
        private Guid? _beneficiaryId;
        private bool _isBeneficiaryIdSet;

        public Guid? BeneficiaryId {
            get => _beneficiaryId;
            set {
                _beneficiaryId = value;
                _isBeneficiaryIdSet = true;
            }
        }

        public HttpServiceResult ValidateAllEditablePropertiesSet()
        {
            var serviceResult = new HttpServiceResult{ Status = ServiceResultStatus.Success };

            if (_isBeneficiaryIdSet == false)
            {
                serviceResult.HttpStatusCode = HttpStatusCode.BadRequest;
                serviceResult.Status = ServiceResultStatus.Failure;
                serviceResult.Error = new Error
                {
                    ErrorCode = ErrorCodes.RequestPropertyNotSet,
                    SystemMessage = $"The {nameof(BeneficiaryId)} field must be set to null or a Guid."
                };
            }
            return serviceResult;
        }
    }
}
