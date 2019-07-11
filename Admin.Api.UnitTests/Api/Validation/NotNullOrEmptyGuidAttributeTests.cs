using System;
using Admin.Api.Validation;
using Xunit;

namespace Admin.Api.UnitTests.Api.Validation
{
    public class NotNullOrEmptyGuidAttributeTests
    {
        [Fact(DisplayName = "NotNullOrEmptyGuidAttribute Should Return Valid For NonEmpty Guid")]
        public void NotNullOrEmptyGuidAttribute_Should_Return_Valid_For_NonEmpty_Guid()
        {
            // arrange
            var value = Guid.NewGuid();
            var attrib = new NotNullOrEmptyGuidAttribute();

            // act
            var result = attrib.IsValid(value);

            // assert
            Assert.True(result);
        }

        [Fact(DisplayName = "NotNullOrEmptyGuidAttribute Should Return InValid For Empty Guid")]
        public void NotNullOrEmptyGuidAttribute_Should_Return_InValid_For_Empty_Guid()
        {
            // arrange
            var value = Guid.Empty;
            var attrib = new NotNullOrEmptyGuidAttribute();

            // act
            var result = attrib.IsValid(value);

            // assert
            Assert.False(result);
        }

        [Fact(DisplayName = "NotNullOrEmptyGuidAttribute Should Return Invalid For Null Value")]
        public void NotNullOrEmptyGuidAttribute_Should_Return_Invalid_For_Null_Value()
        {
            // arrange
            var attrib = new NotNullOrEmptyGuidAttribute();

            // act
            var result = attrib.IsValid(null);

            // assert
            Assert.False(result);
        }
    }
}
