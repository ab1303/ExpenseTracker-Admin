using System;
using Admin.Common.Models;

namespace Admin.Api.Requests
{
    public class AdminFilterRequest : PagedResourceBase
    {
        public Guid UserGuid { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
    }
}
