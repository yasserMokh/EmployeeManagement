using System.Collections.Generic;
using System.Security.Claims;

namespace EmployeeManagement.Web.Models
{
    public static class ClaimStore
    {
        public const string EditRole = "Edit Role";
        public const string DeleteRole = "Delete Role";
        public const string CreateRole = "Create Role";

        public static readonly IEnumerable<Claim> AllClaims = new List<Claim>()
        {
            new Claim(ClaimStore.CreateRole, ClaimStore.CreateRole),
            new Claim(ClaimStore.EditRole, ClaimStore.EditRole),
            new Claim(ClaimStore.DeleteRole, ClaimStore.DeleteRole)
    };
    }
}
