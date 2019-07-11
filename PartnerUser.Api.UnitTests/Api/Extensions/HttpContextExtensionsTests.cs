using PartnerUser.Api.Extensions;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace PartnerUser.Api.UnitTests.Api.Extensions
{
    public class HttpContextExtensionsTests
    {
        private readonly Mock<HttpContext> _contextMock;

        public HttpContextExtensionsTests()
        {
            _contextMock = new Mock<HttpContext>();
        }

        [Fact(DisplayName = "GetFullUrl Should Return Empty Url If HttpContext Request Is Null")]
        public void GetFullUrl_Should_Return_Empty_Url_If_HttpContext_Request_Is_Null()
        {
            //Arrange
            _contextMock.Setup(c => c.Request).Returns((HttpRequest)null);

            //Act
            var fullUrl = _contextMock.Object.GetFullUrl();

            //Assert
            Assert.Equal(string.Empty, fullUrl);
        }

        [Fact(DisplayName = "Returns {schema}://{host}{path}{query-string} as full url when HttpContext.Request is valued")]
        public void GetFullUrl_Should_Return_Expected_Full_Url_If_HttpContext_Request_Is_Not_Null()
        {
            //Arrange
            _contextMock.Setup(c => c.Request.Scheme).Returns("https");
            _contextMock.Setup(c => c.Request.Host).Returns(new HostString("api.ofx.com"));
            _contextMock.Setup(c => c.Request.Path).Returns(new PathString("/apiService.RefData"));
            _contextMock.Setup(c => c.Request.QueryString).Returns(new QueryString("?currency=AUD"));

            //Act
            var fullUrl = _contextMock.Object.GetFullUrl();

            //Assert
            Assert.Equal("https://api.ofx.com/apiService.RefData?currency=AUD", fullUrl);
        }
    }
}
