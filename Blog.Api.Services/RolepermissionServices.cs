using Blog.Api.IServices;
using Blog.Api.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Services
{
    public class RolepermissionServices : BaseServices<Rolepermission>,IRolepermission
    {
        public RolepermissionServices(BlogsqlContext DbContext) : base(DbContext)
        {
        }
    }
}
