using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eUseControl.Domain.Entities.User;

namespace WebsiteGym.Web.Models
{
     public class UserDashDto
     {
          public string UserName { get; set; }
          public string Email { get; set; }
          public bool MembershipStatus { get; set; }
          public DateTime RegisterDateTime { get; set; }
          public int? UserMembershipID { get; set; }
          public DateTime? MembershipExpiration { get; set; }
          public string MembershipType { get; set; }
          public int? RemainingDays
          {
               get
               {
                    if (MembershipExpiration == null) return null;
                    var days = (MembershipExpiration.Value - DateTime.Now).Days;
                    return Math.Max(days, 0);
               }
          }
     }
}