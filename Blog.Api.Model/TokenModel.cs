using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Model
{
    public class TokenModel
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public double Expires { get; set; }
    }
}
