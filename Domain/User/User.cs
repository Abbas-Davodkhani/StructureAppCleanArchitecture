using Domain.Attributes;
using Microsoft.AspNetCore.Identity;

namespace Domain.User
{
    [AuditableAttribute]
    public class User : IdentityUser
    {
        public string FullName { get; set; }
    }
}
