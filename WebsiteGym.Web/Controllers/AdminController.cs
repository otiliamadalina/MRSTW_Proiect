using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using eUseControl.BusinessLogic.DBModel;
using eUseControl.BusinessLogic.Interface;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Entities.Discount;
using eUseControl.Domain.Entities.Membership;
using WebsiteGym.Web.Models;


namespace WebsiteGym.Web.Controllers
{
    public class AdminController : Controller
    {

        private readonly IOrderApi _order;
        private readonly IMembershipApi _membership;
        private readonly IDiscountCode _discount;

        private readonly UserContext _userContext;
        private readonly MembershipContext _membershipContext;

        public AdminController()
        {
            var bl = new BussinesLogic();

            _order = bl.GetOrderApi();
            _membership = bl.GetMembershipApi();
            _discount = bl.GetDiscountApi();

            _userContext = new UserContext(); 
            _membershipContext = new MembershipContext();
        }

        public ActionResult AdminDash()
        {
               using (var context = new UserContext())
               {
                    ViewBag.Users = context.Users.Count();
                    ViewBag.ActiveMemberships = context.Users.Count(u => u.MembershipStatus == true);
            }
               
                    return View();
        }
          public ActionResult ListOfUsers()
          {
               using (var context = new UserContext())
               {
                    var users = context.Users.ToList();
                    ViewBag.Users = users;
               }
               return View();
          }

          public ActionResult DeleteUser(int id)
          {
               using (var context = new UserContext())
               {
                    var user = context.Users.FirstOrDefault(u => u.Id == id);
                    if (user != null)
                    {
                         context.Users.Remove(user);
                         context.SaveChanges();
                    }
               }
               return RedirectToAction("ListOfUsers");
          }

        // TODO: Users table trebuie sa aiba MembershipId, ca sa-l putem afisa in profilul sau

        //public ActionResult ListOfActiveMemberships()
        //{
        //    var activeMemberships = (from user in _userContext.Users
        //                             join membership in _membershipContext.Memberships on user.MembershipId equals membership.Id
        //                             where user.MembershipStatus == 1
        //                             select new ActiveMembershipViewModel
        //                             {
        //                                 MembershipId = membership.Id,
        //                                 UserName = user.UserName,
        //                                 MembershipName = membership.MembershipName,
        //                                 Price = membership.Price,
        //                                 Details = membership.Details
        //                             }).ToList();

        //    return View(activeMemberships);
        //}













        public ActionResult ManageDiscountCodes()
        {

            var model = new DiscountViewModel
            {
                DiscountCodes = _discount.GetAllDiscountCodes()
            };

            return View(model);

        }

        [HttpPost]
        public ActionResult ManageDiscountCodes(DiscountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dto = new NewDiscountDto
                {
                    DiscountCode = model.DiscountCode,
                    DiscountPercentage = (int)model.DiscountPercentage
                };

                _discount.CreateDiscountCode(dto);

                ModelState.Clear();
                model.DiscountCode = string.Empty;
                model.DiscountPercentage = null;
            }

            model.DiscountCodes = _discount.GetAllDiscountCodes();

            return View(model);
        }

        [HttpGet]
        public ActionResult EditDiscountCode(int id)
        {
            var discountCode = _discount.GetDiscountCodeById(new NewDiscountDto { Id = id });
            if (discountCode == null)
            {
                return HttpNotFound();
            }

            return View("EditDiscountCode", discountCode);
        }

        [HttpPost]
        public ActionResult EditDiscountCode(DiscountDbTable model)
        {
            if (ModelState.IsValid)
            {
                _discount.EditDiscountCode(new NewDiscountDto
                {
                    Id = model.Id,
                    DiscountCode = model.DiscountCode,
                    DiscountPercentage = (int)model.DiscountPercentage,
                });

                return RedirectToAction("ManageDiscountCodes");
            }

            return View("EditDiscountCode", model);
        }

        public ActionResult DeleteDiscountCode(int id)
        {
            _discount.RemoveDiscountCode(new NewDiscountDto { Id = id });

            return RedirectToAction("ManageDiscountCodes");
        }


















        public ActionResult ManageMemberships()
        {
            var model = new MembershipViewModel
            {
                Memberships = _membership.GetAllMemberships()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult ManageMemberships(MembershipViewModel model)
        {
            if (!ModelState.IsValid)
            { 
                model.Memberships = _membership.GetAllMemberships(); 
                return View(model);
            }

            var dto = new NewMembershipDto
            {
                membershipName = model.MembershipName,
                price = (decimal)model.Price,
                details = model.Details
            };

            _membership.CreateMembership(dto);

            ModelState.Clear();
            model.MembershipName = string.Empty;
            model.Price = null;
            model.Details = string.Empty;

            model.Memberships = _membership.GetAllMemberships();

            return View(model);
        }


        [HttpGet]
        public ActionResult EditMembership(int id)
        {
            var dto = new NewMembershipDto { Id = id };
            var existing = _membership.GetMembershipById(dto);

            if (existing == null)
                return HttpNotFound();

            var model = new MembershipViewModel
            {
                Id = existing.Id,
                MembershipName = existing.MembershipName,
                Price = existing.Price,
                Details = existing.Details
            };

            return View(model);
        }

        [HttpPost]
        public void EditMembership(NewMembershipDto membership)
        {
            if (membership.Id < 0)
            {
                return;
            }

            using (var context = new MembershipContext())
            {
                var membershipToEdit = context.Memberships.FirstOrDefault(m => m.Id == membership.Id);

                if (membershipToEdit != null)
                {
                    membershipToEdit.MembershipName = membership.membershipName;
                    membershipToEdit.Price = membership.price;
                    membershipToEdit.Details = membership.details;

                    context.SaveChanges();
                }
            }
        }


        public ActionResult DeleteMembership(int id)
        {
            _membership.RemoveMembership(new NewMembershipDto { Id = id }); 
            return RedirectToAction("ManageMemberships");
        }

    }
}