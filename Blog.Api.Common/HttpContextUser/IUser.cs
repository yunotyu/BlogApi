using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Common.HttpContextUser
{
    public interface IUser
    {
        string UserName { get; }
        int Id { get; }
        long TenantId { get; }
        bool IsAuthenticated();
        IEnumerable<Claim> GetClaimsIdentity();

        List<string> GetClaimValueByType(string claimType);
        string GetToken();
        List<string> GetUserInfoFormToken(string claimType);

    }
}
