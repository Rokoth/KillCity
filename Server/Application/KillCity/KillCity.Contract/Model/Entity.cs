using System.ComponentModel.DataAnnotations;

namespace KillCity.Contract.Model
{
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
}
