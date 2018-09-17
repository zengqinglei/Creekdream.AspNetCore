using System.ComponentModel.DataAnnotations;

namespace Creekdream.Application.Service.Dto
{
    /// <summary>
    /// Paging result query
    /// </summary>
    public abstract class PagedResultInput
    {
        /// <summary>
        /// Number of rows
        /// </summary>
        [Display(Name = "返回行数")]
        [Range(1, int.MaxValue, ErrorMessage = "值必须在{1}~{2}之间")]
        public virtual int MaxResultCount { get; set; } = 10;

        /// <summary>
        /// Number of rows skipped
        /// </summary>
        [Display(Name = "跳过行数")]
        [Range(0, int.MaxValue, ErrorMessage = "值必须在{1}~{2}之间")]
        public virtual int SkipCount { get; set; }
    }
}

