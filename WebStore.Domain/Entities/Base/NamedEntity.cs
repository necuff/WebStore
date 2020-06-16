using WebStore.Domain.Entities.Base.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace WebStore.Domain.Entities.Base
{
    public abstract class NamedEntity : BaseEntity, INamedEntity
    {
        [Required] //обязательное поле (not null)
        public string Name { get; set; }
    }
}
