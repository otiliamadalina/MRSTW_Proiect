using eUseControl.BusinessLogic.Interface;
using eUseControl.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eUseControl.BusinessLogic.DBModel;
using eUseControl.Helper.AssistingLogic;
using Microsoft.Win32;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Entities.Order;

namespace eUseControl.BusinessLogic.Core
{
    public class UserServices: IUserServices
    {
          public UserMembership GetUserMembershipById(int id)
          {
               using (var context = new UserContext())
               {
                    return context.UserMemberships.FirstOrDefault(u => u.Id == id);
               }
          }

          public User GetUserById(int id)
          {
               using (var context = new UserContext())
               {
                    return context.Users.FirstOrDefault(u => u.Id == id);
               }
          }

          public bool RegisterUser(User user)
          {
               var helper = new UserHelper();
               using (var context = new UserContext())
               {
                  if ((context.Users.Any(u => u.Email == user.Email) || (context.Users.Any(u => u.Name == user.Name)))){
                    return false;
                  }

                  user.Password = helper.PasswordHash(user.Password);
                  context.Users.Add(user);
                  context.SaveChanges();
                  return true;
               }
              
          }

          public User LoginUser(User user)
          {
               var helper = new UserHelper();
               var hashedPassword= helper.PasswordHash(user.Password);
               using (var context = new UserContext())
               {
                    var UserExists = context.Users.FirstOrDefault(u => u.Name == user.Name && u.Password == hashedPassword);
                    if (UserExists != null)
                    {
                         return UserExists;
                    }
                    else
                    {
                         return null;
                    }
               }
          }

          public bool RemoveUser(string name, string email)
          {
               return true;
          }

        public bool CreateNewOrderAction(ODbTable order)
        {
            if (order.TotalPrice < 0)
            {
                return false;
            }

            using (var context = new OrderContext())
            {
                context.Orders.Add(order);
                context.SaveChanges();
            }
            return true;
        }

          public int? SaveUserMembership(UserMembership userMembership)
          {
               if (userMembership == null)
               {
                    return null;
               }
               using (var context = new UserContext())
               {
                    context.UserMemberships.Add(userMembership);
                    context.SaveChanges();
                    return userMembership.Id;
               }
          }

          public bool UpdateUserMembership(int? userMembershipId, User foundUser)
          {
               using (var context = new UserContext())
               {
                         foundUser.UserMembershipID = userMembershipId;
                         foundUser.MembershipStatus = true;
                         context.SaveChanges();
                         return true;
                         
               }
          }
     }
}
