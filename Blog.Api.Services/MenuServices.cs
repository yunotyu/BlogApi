using Blog.Api.IServices;
using Blog.Api.Model;
using Blog.Api.Model.Models;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using NetTopologySuite.Index.HPRtree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Blog.Api.Services
{
    public class MenuServices : BaseServices<Menu>, IMenuServices
    {
        public MenuServices(BlogsqlContext DbContext) : base(DbContext)
        {
        }
    }
}
