using Blog.Api.IServices;
using Blog.Api.Model;
using Blog.Api.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Services
{
    public class OperatelogServices : BaseServices<Operatelog>, IOperatelogServices
    {
        public OperatelogServices(BlogsqlContext DbContext) : base(DbContext)
        {
        }
    }
}
