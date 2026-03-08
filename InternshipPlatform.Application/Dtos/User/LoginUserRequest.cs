using System.ComponentModel.DataAnnotations;

namespace InternshipPlatform.Application.Dtos.User
{
    public class LoginUserRequest
    {
        [Required(ErrorMessage = "Поле Логин является обязательным")]
        [EmailAddress(ErrorMessage = "Укажите адрес электронной почты")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле Пароль является обязательным")]
        [MinLength(8, ErrorMessage = "Длина пароля не менее 8 символов")]
        public string Password { get; set; }
    }
}
