using Blog.Api.Common;
using Blog.Api.Common.HttpContextUser;
using Blog.Api.IServices;
using Blog.Api.Model.Dto;
using Blog.Api.Model.Models;
using Blog.Api.Model.ViewModels;
using Blog.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace Blog.Api.Controllers
{
    public class MenuController : BaseApiController
    {
        private readonly ILogger<MenuController> _logger;
        private readonly IMenuServices _menuServices;
        private readonly IPermissionServices _permissionServices;
        private readonly IUser _globalUser;
        public MenuController(ILogger<MenuController> logger, IMenuServices menuServices,
                            IPermissionServices permissionServices, IUser user)
        {
            _logger = logger;
            _menuServices = menuServices;
            _permissionServices = permissionServices;
            _globalUser = user;
        }

        [HttpGet]
        //[Authorize(Roles ="admin")]
        public ActionResult<ResultMsg<List<Menu>>> PageData(int pageIndex = 1, int pageSize = 5)
        {
            var menus = _menuServices.QueryPage(pageIndex, pageSize, m => !m.IsDel).ToList();
            return Success(menus);
        }

        [HttpGet]
        //[Authorize(Roles ="admin")]
        public async Task<ActionResult<ResultMsg<Menu>>> GetMenuByPermissionId(long permissionId)
        {
            var p = await _permissionServices.QueryWhere(p => p.Id == permissionId).FirstOrDefaultAsync();
            if (p == null)
            {
                return Fail<Menu>(null, "权限不存在");
            }
            var m = await _menuServices.QueryWhere(m => m.Id == p.MenuId).FirstOrDefaultAsync();
            if (m == null)
            {
                return Fail<Menu>(null, "菜单项不存在");
            }
            return Success(m);
        }

        /// <summary>
        /// 获取子菜单
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet]
        //[Authorize(Roles ="admin")]
        public async Task<ActionResult<ResultMsg<Menu>>> GetMenuChildNode(string parentId)
        {
            //使用linq获取当前父节点的全部子节点，无限层级获取
            var menus = GetDbSonIds(parentId).ToList();
            //获取当前父节点
            var parentMenu = _menuServices.QueryWhere(m => m.Id == parentId).FirstOrDefault();

            //获取当前的根节点的第一层子节点
            var rootChilds = menus.Where(m => m.ParentId == parentId).ToList();

            //对所有子节点进行无限层子节点添加
            parentMenu.ChildMenus.AddRange(rootChilds);
            for (int i = 0; i < parentMenu.ChildMenus.Count; i++)
            {
                AddChilds(parentMenu.ChildMenus[i], menus);
            }

            return Success(parentMenu);


            ////使用存储过程获取全部子节点
            //MySqlParameter[] mySqlParameter = new MySqlParameter[] { new MySqlParameter("pId", MySqlDbType.Int64) };
            //mySqlParameter[0].Value = parentId;
            //var childIds = _menuServices.ExecProcudure<MenuChildId>("GetChildNode",mySqlParameter);
            //return Success(childIds);
        }

        /// <summary>
        /// 获取父节点
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet]
        //[Authorize(Roles ="admin")]
        public async Task<ActionResult<ResultMsg<List<Menu>>>> GetMenuParentNode(string parentId)
        {
            var childMenus = _menuServices.QueryWhere(m => m.Id == parentId).ToList();
            return Success(childMenus);
        }

        [HttpPost]
        //[Authorize(Roles ="admin")]
        public async Task<ActionResult<ResultMsg<string>>> Add(AddMenuViewModel menu)
        {

            _menuServices.BeginTransaction();
            using (_menuServices.DbContextTransaction)
            {
                try
                {
                    var addMenus = new List<Menu>();
                    menu.Id = Guid.NewGuid().ToString();
                    //如果父节点存在，删除该父节点，重新添加
                    var parent = await _menuServices.QueryWhere(m => m.Id == menu.Id).FirstOrDefaultAsync();
                    if (parent != null)
                    {
                        //使用linq获取当前父节点的全部子节点，无限层级获取
                        var menus = GetDbSonIds(menu.Id).ToList();
                        await _menuServices.DeleteData(menus);
                        //先获取当前的父菜单id，插入，然后才能获取父菜单id
                        GetSons(menu, addMenus);
                        await _menuServices.Add(addMenus);
                    }
                    //父节点不存在，直接新增
                    else
                    {
                        addMenus.Add(new Menu()
                        {
                            Id = menu.Id,
                            MenuNames = menu.MenuNames,
                            Url = menu.Url,
                            IsDel = false

                        });
                        GetSons(menu, addMenus);
                        await _menuServices.Add(addMenus);
                    }
                    _menuServices.Commit();
                }
                catch (Exception ex)
                {
                    _menuServices.RollbackTransaction();
                    _logger.LogError(ex.ToString());
                    return Fail("", "添加失败");
                }
            }


            return Success("ok");
        }

        //private List<Menu> SetAddMenus(List<AddMenuViewModel> addMenuViewModels)
        //{
        //    var menus=new List<Menu>();
        //    foreach (var item in addMenuViewModels)
        //    {
        //        menus.Add(new Menu { Id = Guid.NewGuid().ToString() });
        //    }
        //}

        private void GetSons(AddMenuViewModel menu, List<Menu> menus)
        {
            for (int i = 0; i < menu.ChildMenus.Count; i++)
            {
                menu.ChildMenus[i].Id = Guid.NewGuid().ToString();

                menus.Add(new Menu()
                {
                    Id = menu.ChildMenus[i].Id,
                    ParentId = menu.Id,
                    MenuNames = menu.ChildMenus[i].MenuNames,
                    Url = menu.ChildMenus[i].Url,
                    IsDel = false
                });
                GetSons(menu.ChildMenus[i], menus);
            }
        }

        /// <summary>
        /// 递归获取数据库子节点
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private IEnumerable<Menu> GetDbSonIds(string parentId)
        {
            var menus = _menuServices.QueryWhere(m => m.ParentId == parentId).ToList();
            return menus.Concat(menus.SelectMany(m => GetDbSonIds(m.Id)));
        }

        /// <summary>
        /// 递归添加子节点
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="groupMenus"></param>
        private void AddChilds(Menu menu, List<Menu> groupMenus)
        {
            var findMenu = groupMenus.Find(m => m.ParentId == menu.Id);
            if (findMenu != null)
            {
                AddChilds(findMenu, groupMenus);
                menu.ChildMenus.Add(findMenu);
            }
        }
    }

    public class MenuChildId
    {
        public long ChildId { get; set; }
    }
}
