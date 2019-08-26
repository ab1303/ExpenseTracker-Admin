using System;
using Admin.Api.Validation;

namespace Admin.Api.Requests
{
    public class CreateUserRequest
    {
        [NotNullOrEmptyGuid]
        public Guid UserGuid { get; set; }

        public string Email { get; set; }
        
    }
}
