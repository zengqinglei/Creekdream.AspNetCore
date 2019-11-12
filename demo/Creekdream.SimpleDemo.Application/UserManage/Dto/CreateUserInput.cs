using System.ComponentModel.DataAnnotations;

namespace Creekdream.SimpleDemo.UserManage.Dto
{
    /// <summary>
    /// 用户信息输入
    /// </summary>
    public class CreateUserInput
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name = "用户名")]
        [Required(ErrorMessage = "请填写{0}")]
        [StringLength(User.MaxUserNameLength, MinimumLength = User.MinUserNameLength, ErrorMessage = "请填写填写{1}-{2}位")]
        public string UserName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [Display(Name = "用户密码")]
        [Required(ErrorMessage = "请填写{0}")]
        [StringLength(User.MaxPasswordLength, MinimumLength = User.MinPasswordLength, ErrorMessage = "请填写填写{1}-{2}位")]
        public string Password { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        [Required(ErrorMessage = "请填写{0}")]
        [MaxLength(UserInfo.MaxNameLength, ErrorMessage = "最多只能填写{1}位")]
        public string Name { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        [Display(Name = "年龄")]
        public int Age { get; set; }
    }
}
