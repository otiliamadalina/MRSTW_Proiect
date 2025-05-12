using System;
using System.Collections.Generic;
using eUseControl.Domain.Entities.User;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eUseControl.BusinessLogic.DBModel
{
    public class UserContext : DbContext 
    {
          public UserContext() : base("name=eUseControl")
          {
          }
          public virtual DbSet<User> Users { get; set; }
          public virtual DbSet<UserMembership> UserMemberships { get; set; }
     }
}
