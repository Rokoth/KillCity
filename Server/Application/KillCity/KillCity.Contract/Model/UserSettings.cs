using System.ComponentModel.DataAnnotations;

namespace KillCity.Contract.Model
{
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
}
