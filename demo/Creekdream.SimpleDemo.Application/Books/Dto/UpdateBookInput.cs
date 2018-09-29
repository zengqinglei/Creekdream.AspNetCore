using System.ComponentModel.DataAnnotations;

namespace Creekdream.SimpleDemo.Books.Dto
{
    /// <summary>
    /// Update an book
    /// </summary>
    public class UpdateBookInput
    {
        /// <summary>
        /// book name
        /// </summary>
        [Display(Name = "书名")]
        [Required(ErrorMessage = "请填写{0}")]
        [MaxLength(Book.MaxNameLength, ErrorMessage = "最多只能填写{1}位")]
        public string Name { get; set; }
    }
}


