using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KillCity.Contract.Model
{

    public class UserIdentity : IIdentity
    {
        [Display(Name = "Логин")]
        public string Login { get; set; }
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class UserSettings : Entity
    {
        [Display(Name = "ИД пользователя")]
        public Guid UserId { get; set; }
        
        [Display(Name = "Пользователь")]
        public string User { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Телефон")]
        public string Phone { get; set; }

        [Display(Name = "Звук")]
        public bool SoundOn { get; set; }
    }

    public abstract class Entity : IEntity
    {
        /// <summary>
        /// Идентификтаор
        /// </summary>
        [Display(Name = "Идентификатор")]
        public Guid Id { get; set; }
        /// <summary>
        /// Дата последнего изменения
        /// </summary>
        [Display(Name = "Дата последнего изменения")]
        public DateTimeOffset VersionDate { get; set; }
    }

    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
