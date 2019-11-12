using Creekdream.SimpleDemo.Books.Dto;
using System.Threading.Tasks;
using Creekdream.Application.Service;
using Creekdream.Application.Service.Dto;
using System;

namespace Creekdream.SimpleDemo.Books
{
    /// <summary>
    /// 书信息服务
    /// </summary>
    public interface IBookService : IApplicationService
    {
        /// <summary>
        /// 获取书信息
        /// </summary>
        Task<GetBookOutput> Get(Guid id);

        /// <summary>
        /// 获取书信息
        /// </summary>
        Task<PagedResultOutput<GetBookOutput>> GetPaged(GetPagedBookInput input);

        /// <summary>
        /// 新增书信息
        /// </summary>
        Task<GetBookOutput> Create(CreateBookInput input);

        /// <summary>
        /// 修改书信息
        /// </summary>
        Task<GetBookOutput> Update(Guid id, UpdateBookInput input);

        /// <summary>
        /// 删除书信息
        /// </summary>
        Task Delete(Guid id);
    }
}


