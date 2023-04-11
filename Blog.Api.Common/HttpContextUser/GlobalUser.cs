using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Common.HttpContextUser
{
    public class GlobalUser : IUser
    {
        public string UserName => throw new NotImplementedException();

        public int Id => throw new NotImplementedException();

        public long TenantId => throw new NotImplementedException();

        public bool IsAuthenticated => throw new NotImplementedException();

        public IEnumerable<Claim> GetClaimsIdentity()
        {
            throw new NotImplementedException();
        }

        public List<string> GetClaimValueByType(string claimType)
        {
            throw new NotImplementedException();
        }

        public string GetToken()
        {
            throw new NotImplementedException();
        }

        public List<string> GetUserInfoFormToken(string claimType)
        {
            throw new NotImplementedException();
        }
    }
}
