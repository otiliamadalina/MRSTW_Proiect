using eUseControl.BusinessLogic.Interface;
using eUseControl.Domain.Entities.User;
using eUseControl.Domain.Entities.Order;
using eUseControl.Domain.Entities;
using System;
using System.Linq;
using System.Web.Mvc;
using WebsiteGym.Web.Models;
using System.Web.Security;

namespace WebsiteGym.Web.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly IOrderApi _order;
        private readonly IMembershipApi _membership;
        private readonly IDiscountCode _discountCodeService;

        public CheckoutController(IUserServices userServices, IOrderApi order, IMembershipApi membership, IDiscountCode discountCodeService)
        {
            _userServices = userServices;
            _order = order;
            _membership = membership;
            _discountCodeService = discountCodeService;
        }

        public CheckoutController()
        {
            var bl = new BussinesLogic();
            _order = bl.GetOrderApi();
            _membership = bl.GetMembershipApi();
            _userServices = bl.GetUserApi();
            _discountCodeService = bl.GetDiscountApi();
        }

        // GET: CheckoutMembership
        public ActionResult CheckoutMembership(int membershipId)
        {
            if (Session["UserRole"]?.ToString() != "User")
            {
               return RedirectToAction("AuthPage", "Home");
            }

            var selectedMembership = _membership.GetAllMemberships().FirstOrDefault(m => m.Id == membershipId);

            var model = new OrderViewModel
            {
                AvailableMemberships = _membership.GetAllMemberships(),
                MembershipName = selectedMembership?.MembershipName,
                AvailableDiscountCodes = _discountCodeService.GetAllDiscountCodes(),
                MembershipDuration = 0,
                Subtotal = 0,
                TotalPrice = 0
            };

            return View(model);
        }

        [HttpPost]
        public JsonResult CalculatePrice(string membershipName, int membershipDuration)
        {
            var membership = _membership.GetAllMemberships().FirstOrDefault(m => m.MembershipName == membershipName);

            decimal price = (decimal)membership.Price * membershipDuration;

            return Json(new { success = true, subtotal = price, totalPrice = price });
        }

        [HttpPost]
        public JsonResult ApplyDiscount(string membershipName, int membershipDuration, string discountCode)
        {
            var membership = _membership.GetAllMemberships().FirstOrDefault(m => m.MembershipName == membershipName);

            if (membership == null)
            {
                return Json(new { success = false, message = "Invalid membership." });
            }

            var discount = _discountCodeService.GetAllDiscountCodes().FirstOrDefault(d => d.DiscountCode.Equals(discountCode, StringComparison.OrdinalIgnoreCase));

            if (discount == null) 
            {
                return Json(new { success = false, message = "Invalid discount code." });
            }

            decimal price = (decimal)membership.Price * membershipDuration;
            price -= (price * discount.DiscountPercentage / 100m); 

            return Json(new { success = true, subtotal = price, totalPrice = price });
        }


        // POST: CheckoutMembership
        [HttpPost]
        public ActionResult CheckoutMembership(OrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                    foreach (var entry in ModelState)
                    {
                         foreach (var error in entry.Value.Errors)
                         {
                              System.Diagnostics.Debug.WriteLine($"[Model Error] {entry.Key}: {error.ErrorMessage}");
                         }
                    }

                    model.AvailableMemberships = _membership.GetAllMemberships();
                    model.AvailableDiscountCodes = _discountCodeService.GetAllDiscountCodes();
                    return View(model);
            }

            if (!string.IsNullOrEmpty(model.DiscountCode))
            {
                var discount = _discountCodeService.GetAllDiscountCodes()
                                  .FirstOrDefault(d => d.DiscountCode.Equals(model.DiscountCode, StringComparison.OrdinalIgnoreCase));
                if (discount != null)
                {
                    model.TotalPrice -= (model.TotalPrice * discount.DiscountPercentage / 100m);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid discount code.");
                    model.AvailableMemberships = _membership.GetAllMemberships();
                    model.AvailableDiscountCodes = _discountCodeService.GetAllDiscountCodes();
                    return View(model);
                }
            }

            var newOrder = new ODbTable
            {
                MembershipName = model.MembershipName,
                UserName = Session["UserName"]?.ToString(),
                 OrderDate = DateTime.Now,
                TotalPrice = model.TotalPrice
            };


              
            bool created = _order.CreateOrder(newOrder);

            if (!created)
            {
                ModelState.AddModelError("", "Could not create order.");
                    model.AvailableMemberships = _membership.GetAllMemberships();
                    model.AvailableDiscountCodes = _discountCodeService.GetAllDiscountCodes();
                    return View(model);
             }
               else
             {
                    var userId = (int)Session["UserId"];
                    var newUserMembership = new UserMembership
                    {
                         MembershipType = model.MembershipName,
                         MembershipExperationDate = DateTime.Now.AddMonths(model.MembershipDuration),

                    };

                    var newMembershipId = _userServices.SaveUserMembership(newUserMembership);

                    var User = _userServices.GetUserById(userId);
                    if (User != null && newMembershipId != null)
                    {
                         _userServices.UpdateUserMembership(newMembershipId, User);
                    }
                    else
                    {
                         ModelState.AddModelError("", "User not found.");
                         model.AvailableMemberships = _membership.GetAllMemberships();
                         model.AvailableMemberships = _membership.GetAllMemberships();
                         return View(model);
                    }

                    return RedirectToAction("OrderSuccess");
             }

                   
        }

        public ActionResult OrderSuccess()
        {
               if (Session["UserRole"]?.ToString() != "User")
               {
                    return RedirectToAction("AuthPage", "Home");
               }
               return View();
        }
    }
}
