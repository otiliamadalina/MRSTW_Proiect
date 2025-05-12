using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eUseControl.Domain.Entities.BaseEntities;

namespace eUseControl.Domain.Entities
{
    public class Coach : BaseEntity
    {
        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Name is not valid")]
        public string Name { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Surname is not valid")]
        public string Surname { get; set; }
        [Required]
        public DateTime Birthdate { get; set; }
        [Required]
        [StringLength(70, MinimumLength = 10, ErrorMessage = "Please specify a detailed speciality for the trainer")]
        public string Speciality { get; set; }
    }
}