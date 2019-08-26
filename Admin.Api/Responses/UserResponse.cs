using System;

namespace Admin.Api.Responses
{
    public class UserResponse
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
    }
}
