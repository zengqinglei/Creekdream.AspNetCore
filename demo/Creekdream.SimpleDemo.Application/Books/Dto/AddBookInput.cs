using System;
using System.ComponentModel.DataAnnotations;

namespace Creekdream.SimpleDemo.Books.Dto
{
    /// <summary>
    /// Add an book
    /// </summary>
    public class AddBookInput
    {
        /// <summary>
        /// User Id
        /// </summary>
        [Display(Name = "用户Id")]
        [Required(ErrorMessage = "请填写{0}")]
        public Guid UserId { get; set; }

        /// <summary>
        /// Book name
        /// </summary>
        [Display(Name = "书名")]
        [Required(ErrorMessage = "请填写{0}")]
        [MaxLength(Book.MaxNameLength, ErrorMessage = "最多只能填写{1}位")]
        public string Name { get; set; }
    }
}


