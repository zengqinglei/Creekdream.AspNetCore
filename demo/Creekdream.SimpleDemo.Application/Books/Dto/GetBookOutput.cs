using System;

namespace Creekdream.SimpleDemo.Books.Dto
{
    /// <summary>
    /// Book information output
    /// </summary>
    public class GetBookOutput
    {
        /// <summary>
        /// Book id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Book name
        /// </summary>
        public string Name { get; set; }
    }
}


