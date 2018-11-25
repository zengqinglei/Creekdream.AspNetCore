using Creekdream.SimpleDemo.UserManage;
using Creekdream.SimpleDemo.UserManage.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Creekdream.SimpleDemo.Api.Controllers
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;

        /// <inheritdoc />
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 新用户信息
        /// </summary>
        [HttpPost]
        public async Task<GetUserOutput> Post([FromBody]AddUserInput input)
        {
            return await _userService.Add(input);
        }
    }
}


