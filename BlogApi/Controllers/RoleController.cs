using Blog.Api.Common;
using Blog.Api.Common.HttpContextUser;
using Blog.Api.IServices;
using Blog.Api.Model.Dto;
using Blog.Api.Model.Models;
using Blog.Api.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Controllers
{
    public class RoleController:BaseApiController
    {
        private readonly IRoleServices _roleServices;
        private readonly IRolepermission _rolepermission;
        private readonly IUserRoleServices _userRoleServices;
        private readonly ILogger<RoleController> _logger;
        private readonly IUserServices _userServices;
        private readonly IUser _globalUser;

        public RoleController(IRoleServices roleServices, ILogger<RoleController> logger, IRolepermission rolepermission,
                                IUserRoleServices userRoleServices, IUserServices userServices, IUser globalUser)
        {
            _roleServices = roleServices;
            _logger = logger;
            _rolepermission = rolepermission;
            _userRoleServices = userRoleServices;
            _userServices = userServices;
            _globalUser = globalUser;
        }

        [HttpPost]
        //[Authorize(Roles ="admin")]
        public async Task<ActionResult<ResultMsg<PageDataDto<RoleDto>>>> PageData(int pageIndex=1, int pageCount=5)
        {
            var roles = await _roleServices.QueryPage(pageIndex, pageCount).ToListAsync();
            var roleDto=new List<RoleDto>();
            foreach (var item in roles)
            {
                roleDto.Add(new RoleDto { 
                    Id=item.Id,
                    RoleName=item.RoleName,
                    CreateTime=item.CreateTime,
                    CreateUsername=item.CreateUsername,
                    ModifyTime=item.ModifyTime,
                    ModifyUsername=item.ModifyUsername,
                    Description =item.Description,
                    Enable=item.Enable,
                    Level=item.Level
                });
            }
            long totalCount = _roleServices.Count().Result;
            var pegaDatDto = new PageDataDto<RoleDto>()
            {
                PageCount = pageCount,
                PageIndex = pageIndex,
                TotalCount = totalCount,
                PageData = roleDto,
                TotalPages = (long)Math.Ceiling((double)totalCount / pageCount)

            };
            return Success<PageDataDto<RoleDto>>(pegaDatDto);
        }

        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Authorize(Roles ="admin")]
        public async Task<ActionResult<ResultMsg<RoleNameDto>>> GetUserRoleById(long userId)
        {
            var dto = new RoleNameDto();
            if (!await IsExistUser(userId))
            {
                dto.Msg = "用户不存在";
                return Fail<RoleNameDto>(dto);
            }
            List<long> rIds=new List<long>();
            var roleNames = _roleServices.GetUserRoleNames(userId, out rIds).ToList();
            
            for (int i = 0; i < rIds.Count; i++)
            {
                dto.RoleDatas.Add(new RoleData()
                {
                    Id = rIds[i],
                    Name = roleNames[i],
                }) ;
            }
            dto.Msg = "ok";
            return Success<RoleNameDto>(dto);
        }

        /// <summary>
        /// 修改用户角色
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //[Authorize(Roles ="admin")]
        public async Task<ActionResult<ResultMsg<string>>> ModifyUserRole([FromBody]UserRoleViewModel vm)
        {
            if (!await IsExistUser(vm.UserId))
            {
                return Fail("用户不存在");
            }
            var exceptIds= GeteExceptRoleIds(vm.RoleIds);
            if (exceptIds.Count > 0)
            {
                return Fail($"{string.Join(",", exceptIds.ToArray())}:角色id不存在");
            }

            using(_userRoleServices.DbContextTransaction)
            {
                try
                {
                    _userRoleServices.BeginTransaction();

                    //先删除该用户所有角色
                    var urs = _userRoleServices.QueryWhere(ur => ur.UserId == vm.UserId).ToList();
                    await _userRoleServices.DeleteData(urs);
                    //await _userRoleServices.BulkDeleteData(urs);

                    //插入新的用户角色
                    var newURs = new List<Userrole>();
                    foreach (var item in vm.RoleIds)
                    {
                        newURs.Add(new Userrole()
                        {
                            UserId = vm.UserId,
                            RoleId = item,
                            CreateTime = DateTime.Now,
                            CreateUsername = _globalUser.UserName,
                            ModifyUsername = _globalUser.UserName,
                            ModifyTime = DateTime.Now
                        });
                    }
                    await _userRoleServices.Add(newURs);
                    //await _userRoleServices.AddBulk(newURs);
                    _userRoleServices.Commit();
                }
                catch (Exception ex)
                {
                    _userRoleServices.RollbackTransaction();
                    _logger.LogError(ex.ToString());
                    return Fail("新增数据异常");
                }
            }
            return Success("ok");
        }

        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize(Roles ="admin")]
        public async Task<ActionResult<ResultMsg<string>>> Update(List<Role>roles)
        {
            var roleIds = roles.Select(r => r.Id).ToList();
            var existRoles = _roleServices.QueryWhere(r => roleIds.Contains(r.Id)).Select(r =>new { r.Id,r.RoleName}).AsNoTracking().ToList();
            var existIds = existRoles.Select(r => r.Id).ToList();
            var exceptIds = roleIds.Except(existIds).ToList();
            if (exceptIds.Count > 0)
            {
                var names = roles.Where(r => exceptIds.Contains(r.Id)).Select(r => r.RoleName).ToList();
                return Fail($"{string.Join(",", names)}:角色不存在");
            }
            var updateRoles = new List<Role>();
            foreach (var item in roles)
            {
                updateRoles.Add(new Role()
                {
                    Id=item.Id,
                    RoleName=item.RoleName,
                    Description=item.Description,
                    Enable=item.Enable,
                    ModifyTime=DateTime.Now,
                    CreateTime=DateTime.Now,
                    CreateUsername=_globalUser.UserName,
                    ModifyUsername=_globalUser.UserName,
                    IsDel=item.IsDel,
                });
            }
            await _roleServices.Update(updateRoles);
            return Success("ok");
        }

        /// <summary>
        /// 添加角色信息
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize(Roles ="admin")]
        public async Task<ActionResult<ResultMsg<string>>> Add(List<AddRoleViewModel> roles)
        {
            var roleNames = roles.Select(r => r.RoleName).ToList();
            var rNames = _roleServices.QueryWhere(r => roleNames.Contains(r.RoleName)).Select(r => r.RoleName).ToList();
            if (rNames.Count > 0)
            {
                return Fail($"{string.Join(",", rNames)}:角色已经存在");
            }

            var addRoles=new List<Role>();

            for (int i = 0; i < roles.Count; i++)
            {
                addRoles.Add(new Role()
                {
                    RoleName = roles[i].RoleName,
                    Description= roles[i].Description,
                    CreateTime = DateTime.Now,
                    CreateUsername = _globalUser.UserName,
                    IsDel = false,
                    Enable=true,
                    ModifyTime = DateTime.Now,
                });
            }
            await _roleServices.AddBulk(addRoles);
            return Success("ok");
        }

        /// <summary>
        /// 删除角色信息
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize(Roles ="admin")]
        public async Task<ActionResult<ResultMsg<string>>> Delete(List<long> roleIds)
        {
            var roles = _roleServices.QueryWhere(r => roleIds.Contains(r.Id)).ToList();
            var exitsIds = roles.Select(r => r.Id).ToList();
            var exceptIds= roleIds.Except(exitsIds).ToList();
            if (exceptIds.Count > 0)
            {
                return Fail($"{string.Join(",", exceptIds)}:id角色不存在");
            }
            for (int i = 0; i < roles.Count; i++)
            {
                roles[i].IsDel = true;
            }
            await _roleServices.Delete(roles);
            return Success("");
        }

        /// <summary>
        /// 通过角色id获取角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        //[Authorize(Roles ="admin")]
        public async Task<ActionResult<ResultMsg<RoleDto>>> GetRoleById(long id)
        {
            var role=await _roleServices.QueryWhere(r=>r.Id == id&&!r.IsDel).FirstOrDefaultAsync();
            if (role == null) return Fail<RoleDto>(null, "角色不存在");
            RoleDto roleDto = new RoleDto()
            {
                Id= role.Id,
                RoleName=role.RoleName,
                Description=role.Description,
                Level=role.Level,
                Enable=role.Enable,
                CreateUsername=role.CreateUsername,
                CreateTime=role.CreateTime,
                ModifyTime=role.ModifyTime,
                ModifyUsername=role.ModifyUsername,
            };
            return Success(roleDto);
        }

        private async Task<bool> IsExistUser(long id)
        {
            var user = await _userServices.QueryWhere(u => u.Id == id && !u.IsDel).FirstOrDefaultAsync();
            if (user == null) return false;
            return true;
        }

        /// <summary>
        /// 获取不存在的角色id
        /// </summary>
        /// <param name="rids"></param>
        /// <returns></returns>
        private List<long> GeteExceptRoleIds(List<long> rids)
        {
            var existIds= _roleServices.QueryWhere(r => rids.Contains(r.Id)).Select(r=>r.Id);
            var exceptIds=rids.Except(existIds).ToList();
            return exceptIds;
        }
    }
}
