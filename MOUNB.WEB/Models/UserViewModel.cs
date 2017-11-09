using System;
using MOUNB.BLL.DTO;
using System.ComponentModel.DataAnnotations;

namespace MOUNB.WEB.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        // Фамилия Имя Отчество

        //[Required]
        [Display(Name = "ФИО")]
        //[MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string Name { get; set; }
        // Логин
        //[Required]
        [Display(Name = "Логин")]
        //[MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string Login { get; set; }
        // Пароль
        //[Required]
        [Display(Name = "Пароль")]
        //[MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string Password { get; set; }
        // Должность
        [Display(Name = "Должность")]
        //[MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string Position { get; set; }
        // Идентификатор роли
        //[Required]
        [Display(Name = "Роль")]
        //[NotAllowedUserRole(ErrorMessage = "Выберите роль пользователя")] // Собственная логика валидации
        public UserRole Role { get; set; }
    } // Конец класса

    public enum UserRole : byte
    {
        Нет = 0,
        Администратор = 1,
        Библиотекарь = 2
    }

    public class NotAllowedUserRoleAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            UserRole role = (UserRole)value;
            if (role == UserRole.Нет)
            {
                return false;
            }
            return true;
        }
    }
}