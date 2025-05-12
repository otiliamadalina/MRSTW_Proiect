using eUseControl.Domain.Entities.BaseEntities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eUseControl.Domain.Enums;

namespace eUseControl.Domain.Entities.User
{
    public class User : BaseEntity
     {
          [Required]
          [Display(Name = "User Name")]
          [StringLength(50, MinimumLength = 5, ErrorMessage ="Username is not valid")]
          public string Name { get; set; }
          [Required]
          [Display(Name = "User Email")]
          [StringLength(50, MinimumLength = 11, ErrorMessage = "Email is not valid")]
          public string Email { get; set; }
          [Required]
          [Display(Name = "User Password")]
          [StringLength(250, MinimumLength = 8, ErrorMessage = "Password is not valid")]
          public string Password { get; set; }
          [Display(Name = "RegisterDateTime")]

          public int? UserMembershipID { get; set; }

          [ForeignKey("UserMembershipID")]
          public virtual UserMembership UserMembership { get; set; }
          public bool MembershipStatus { get; set; } = false;
          public DateTime ReggisterDateTime { get; set; }   
          public UserRoles Role { get; set; }
     }

}