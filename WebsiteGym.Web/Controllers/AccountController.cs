using System.Web.Mvc;
using eUseControl.BusinessLogic.Interface;
using WebsiteGym.Web.Models;
using eUseControl.BusinessLogic.Core;
using eUseControl.Domain.Entities.User;
using eUseControl.Domain.Enums;
using System;
using System.Runtime.Remoting.Messaging;
namespace WebsiteGym.Web.Controllers
{
     public class AccountController : Controller
     {
          public ActionResult UserDashboard()
          {
               if (Session["UserRole"]?.ToString() == "Admin")
               {
                    return RedirectToAction("AdminDashboard", "Admin");
               }
               else if (Session["UserRole"]?.ToString() == "User")
               {
                    int userId = (int)Session["UserId"];
                    var userService = new UserServices();
                    var user = userService.GetUserById(userId);
                    UserMembership userMembership = null;
                    if (user != null)
                    {
                         if (user.UserMembershipID != null && user.MembershipStatus)
                         {
                              userMembership = userService.GetUserMembershipById((int)user.UserMembershipID);
                         }
                         var model = new UserDashDto
                         {
                              UserName = user.Name,
                              Email = user.Email,
                              MembershipStatus = user.MembershipStatus,
                              RegisterDateTime = user.ReggisterDateTime,
                              MembershipExpiration = userMembership?.MembershipExperationDate,
                              MembershipType = userMembership?.MembershipType,

                         };
                         return View(model);
                    }
                    else
                    {
                         ModelState.AddModelError("", "User not found");
                         return RedirectToAction("AuthPage", "Home");
                    }
               }
               else
               {
                    return RedirectToAction("AuthPage", "Home");
               }
          }
          [HttpPost]
          public ActionResult Register(AuthPageModel model)
          { 
               if (!ModelState.IsValid)
               {
                    ModelState.AddModelError("", "Invalid data");
                    return View("~/Views/Home/AuthPage.cshtml", model);
               }
               else
               {
                    var user = new User
                    {
                         Name = model.Register.UserName,
                         Email = model.Register.Email,
                         Password = model.Register.Password,
                         Role = UserRoles.User,
                         ReggisterDateTime = DateTime.Now,
                         MembershipStatus = false,
                         UserMembershipID = null,

                    };

                    var userService = new UserServices();
                    bool result = userService.RegisterUser(user);

                    if (result)
                    {
                         return RedirectToAction("AuthPage", "Home");
                    }
                    else
                    {
                         ModelState.AddModelError("", "User already exists");
                         return View("~/Views/Home/AuthPage.cshtml", model);
                    }
               }
          }
          [HttpPost]
        public ActionResult Login(AuthPageModel model)
        {
               if (!ModelState.IsValid) {
                    ModelState.AddModelError("", "Invalid data");
                    return View("~/Views/Home/AuthPage.cshtml", model);
               }
               else
               {
                    var user = new User
                    {
                         Name = model.Login.UserName,
                         Password = model.Login.Password,
                    };
                    var userService = new UserServices();
                    var foundUser = userService.LoginUser(user);
                    if (foundUser != null)
                    {
                         Session["UserId"] = foundUser.Id;
                         Session["UserName"] = foundUser.Name;
                         Session["UserRole"] = foundUser.Role;
                         
                         return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                         ModelState.AddModelError("", "Invalid username or password");
                         return View("~/Views/Home/AuthPage.cshtml", model);
                    }
               }
        }

          public ActionResult Logout()
          {
               Session.Clear();    
               Session.Abandon();   

               return RedirectToAction("Index", "Home");
          }

     }
}