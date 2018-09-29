using Creekdream.Application.Service.Dto;
using System.ComponentModel.DataAnnotations;

namespace Creekdream.SimpleDemo.Books.Dto
{
    /// <summary>
    /// Paging query book information
    /// </summary>
    public class GetPagedBookInput : PagedAndSortedResultInput
    {
        /// <summary>
        /// Book name(Fuzzy matching)
        /// </summary>
        [Display(Name = "书名")]
        [MaxLength(Book.MaxNameLength)]
        public string Name { get; set; }
    }
}


