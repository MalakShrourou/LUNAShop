using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Infrastructure.Authorization
{
    public class PermissionAuthorizationHandler : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            var requirement = context.Requirements.OfType<PermissionRequirement>().FirstOrDefault();
            if (requirement != null)
            {
                if (context.User.HasClaim(c => c.Type == "Permission" && c.Value == requirement.Permission.ToString()))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
