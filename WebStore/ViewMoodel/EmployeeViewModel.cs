using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.ViewMoodel
{
    public class EmployeeViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Имя является обязательным")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Длина имени должна быть от 3 до 200 символов")]
        [RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Ошибка формата имени")]
        public string FirstName { get; set; }        
        
        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Фамилия является обязательной")]
        [MinLength(3, ErrorMessage = "Длина фамилии должна быть больше 3 символов")]
        public string Surname { get; set; }
        
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        [Display(Name = "Возраст")]
        [Required]
        [Range(20,70, ErrorMessage = "Возраст должен быть в пределах от 20 до 70 лет")]
        public int Age { get; set; }
    }
}
