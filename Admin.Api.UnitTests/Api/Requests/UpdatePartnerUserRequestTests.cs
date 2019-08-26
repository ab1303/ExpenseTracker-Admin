using System;
using System.Net;
using Admin.Api.Requests;
using Admin.Services.Results;
using Xunit;

namespace Admin.Api.UnitTests.Api.Requests
{
    public class UpdatePartnerUserRequestTests
    {
        [Fact(DisplayName = "ValidateAllEditablePropertiesSet Should Return Success If BeneficaryId Set To Null")]
        public void ValidateAllEditablePropertiesSet_Should_Return_Success_If_BeneficaryId_Set_To_Null()
        {
            //Arrange
            var request = new UpdateUserRequest{
                FirstName = null
            };

            //Act
            var validateResult = request.ValidateAllEditablePropertiesSet();

            //Assert
            Assert.NotNull(validateResult);
            Assert.True(validateResult.IsSuccess);
        }

        [Fact(DisplayName = "ValidateAllEditablePropertiesSet Should Return Success If BeneficaryId Set To Guid")]
        public void ValidateAllEditablePropertiesSet_Should_Return_Success_If_BeneficaryId_Set_To_Guid()
        {
            //Arrange
            var request = new UpdateUserRequest
            {
                FirstName = "Updated FirstName"
            };

            //Act
            var validateResult = request.ValidateAllEditablePropertiesSet();

            //Assert
            Assert.NotNull(validateResult);
            Assert.True(validateResult.IsSuccess);
        }

        [Fact(DisplayName = "ValidateAllEditablePropertiesSet Should Return Fail If BeneficaryId Not Set")]
        public void ValidateAllEditablePropertiesSet_Should_Return_Fail_If_BeneficaryId_Not_Set()
        {
            //Arrange
            var request = new UpdateUserRequest();

            //Act
            var validateResult = request.ValidateAllEditablePropertiesSet();

            //Assert
            Assert.NotNull(validateResult);
            Assert.False(validateResult.IsSuccess);
            Assert.Equal(HttpStatusCode.BadRequest, validateResult.HttpStatusCode);
            Assert.Equal(ErrorCodes.RequestPropertyNotSet, validateResult.Error.ErrorCode);
        }
    }
}
