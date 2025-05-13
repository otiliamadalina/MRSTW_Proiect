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
            //if (Session["UserRole"]?.ToString() != "User")
            //{
            //    return RedirectToAction("AuthPage", "Home");
            //}

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

        // POST: CheckoutMembership
        [HttpPost]
        public ActionResult CheckoutMembership(OrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
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
                    return View(model);
                }
            }

            var newOrder = new NewOrderDto
            {
                membershipName = model.MembershipName,
                userName = model.UserName,
                membershipDuration = model.MembershipDuration,
                orderDate = DateTime.Now,
                totalPrice = model.TotalPrice
            };

            bool created = _order.CreateOrder(newOrder);

            if (!created)
            {
                ModelState.AddModelError("", "Could not create order.");
                return View(model);
            }

            return RedirectToAction("OrderSuccess");
        }

        public ActionResult OrderSuccess()
        {
            return View();
        }
    }
}
