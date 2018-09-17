using System.ComponentModel.DataAnnotations;

namespace Creekdream.Application.Service.Dto
{
    /// <summary>
    /// Sort paging condition query
    /// </summary>
    public class PagedAndSortedResultInput : PagedResultInput
    {
        /// <summary>
        /// Sort fields and order (default: creationTime desc)
        /// </summary>
        [Display(Name = "排序字段及顺序(CreationTime Desc)")]
        public virtual string Sorting { get; set; } = "CreationTime Desc";
    }
}

