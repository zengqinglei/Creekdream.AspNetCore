using System.Collections.Generic;

namespace Creekdream.Application.Service.Dto
{
    /// <summary>
    /// Paging output
    /// </summary>
    public class PagedResultOutput<T>
    {
        /// <summary>
        /// Total number of records
        /// </summary>
        public long TotalCount { get; set; }

        /// <summary>
        /// Record collection
        /// </summary>
        public List<T> Items { get; set; }
    }
}

