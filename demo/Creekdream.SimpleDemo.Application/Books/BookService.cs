using Creekdream.Application.Service;
using Creekdream.Application.Service.Dto;
using Creekdream.Domain.Repositories;
using Creekdream.Mapping;
using Creekdream.Orm.EntityFrameworkCore;
using Creekdream.SimpleDemo.Books.Dto;
using Creekdream.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Creekdream.SimpleDemo.Books
{
    /// <inheritdoc />
    public class BookService : ApplicationService, IBookService
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<Book, Guid> _bookRepository;

        /// <inheritdoc />
        public BookService(IUnitOfWorkManager unitOfWorkManager, IRepository<Book, Guid> bookRepository)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _bookRepository = bookRepository;
        }

        /// <inheritdoc />
        public async Task<GetBookOutput> Get(Guid id)
        {
            var book = await _bookRepository.GetAsync(id);
            return book.MapTo<GetBookOutput>();
        }

        /// <inheritdoc />
        public async Task<PagedResultOutput<GetBookOutput>> GetPaged(GetPagedBookInput input)
        {
            var query = _bookRepository.GetQueryIncluding(p => p.User);
            if (!string.IsNullOrEmpty(input.Name))
            {
                query = query.Where(m => m.Name.Contains(input.Name));
            }
            var totalCount = await query.CountAsync();
            var books = await query.OrderBy(input.Sorting)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToListAsync();

            return new PagedResultOutput<GetBookOutput>()
            {
                TotalCount = totalCount,
                Items = books.MapTo<List<GetBookOutput>>()
            };
        }

        /// <inheritdoc />
        [UnitOfWork]
        public async Task<GetBookOutput> Add(AddBookInput input)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                var book = input.MapTo<Book>();
                book = await _bookRepository.InsertAsync(book);

                uow.Complete();

                return book.MapTo<GetBookOutput>();
            }
        }

        /// <inheritdoc />
        public async Task<GetBookOutput> Update(Guid id, UpdateBookInput input)
        {
            var book = await _bookRepository.GetAsync(id);
            input.MapTo(book);
            book = await _bookRepository.UpdateAsync(book);
            return book.MapTo<GetBookOutput>();
        }

        /// <inheritdoc />
        public async Task Delete(Guid id)
        {
            await _bookRepository.DeleteAsync(id);
        }
    }
}


