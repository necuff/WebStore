using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entities.Base.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStore.Domain.Entities.Base
{    
    public abstract class BaseEntity : IBaseEntity
    {
        [Key]   //Primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   //Автоинкремент
        public int Id { get; set; }
    }
}
