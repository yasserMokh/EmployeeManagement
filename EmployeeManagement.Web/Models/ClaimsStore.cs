using System.Collections.Generic;
using System.Security.Claims;

namespace EmployeeManagement.Web.Models
{
    public static class ClaimsStore
    {
        public static IEnumerable<Claim> AllClaims = new List<Claim>()
        {
            new Claim("Create Role", "Create Role"),
            new Claim("Edit Role", "Edit Role"),
            new Claim("Delete Role", "Delete Role")
    };
    }
}
