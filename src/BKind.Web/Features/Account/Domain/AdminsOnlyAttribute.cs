using Microsoft.AspNetCore.Authorization;

namespace BKind.Web.Features.Account.Domain
{
    public class AdminsOnlyAttribute : AuthorizeAttribute
    {
        public AdminsOnlyAttribute() 
        {
            Policy = "isAdmin";
        }
    }
}