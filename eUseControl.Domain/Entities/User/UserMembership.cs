using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eUseControl.Domain.Entities.BaseEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eUseControl.Domain.Entities.User
{
     public class UserMembership : BaseEntity
     {
          [Required]
          public string MembershipType { get; set; }
          [Required]
          public DateTime MembershipExperationDate { get; set; }
     }
}
