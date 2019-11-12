using Creekdream.SimpleDemo.UserManage.Dto;
using System.Threading.Tasks;

namespace Creekdream.SimpleDemo.UserManage
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public interface IUserService
    {

        /// <summary>
        /// 新增用户信息
        /// </summary>
        Task<GetUserOutput> Create(CreateUserInput input);
    }
}
