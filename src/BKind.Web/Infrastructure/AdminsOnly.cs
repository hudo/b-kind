using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace BKind.Web.Infrastructure
{
    public class AdminsOnlyAttribute : AuthorizeAttribute
    {
        public AdminsOnlyAttribute() 
        {
            Policy = "isAdmin";
        }
    }
}