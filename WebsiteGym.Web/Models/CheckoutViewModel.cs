using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using eUseControl.BusinessLogic.Interface;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Entities.BaseEntities;

namespace WebsiteGym.Web.Models
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Membership ID is required.")]
        public int MembershipID { get; set; }
        [Required(ErrorMessage = "Please select a membership.")]
        [Display(Name = "Membership Name")]
        public string MembershipName { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please select membership duration.")]
        [Display(Name = "Membership Duration")]
        public int MembershipDuration { get; set; }

        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Base Price")]
        public decimal BasePrice { get; set; }

        [Required]
        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }

        //CA IN CHeckoutMembership.cshtml sa mearga

        [Required(ErrorMessage = "First name is required.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Card number is required.")]
        [Display(Name = "Card Number")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "CVV is required.")]
        [Display(Name = "CVV")]
        public string CVV { get; set; }

        [Required(ErrorMessage = "Expiration date is required.")]
        [Display(Name = "Expiration Date")]
        public string ExpDate { get; set; }


        public string DiscountCode { get; set; }
        public List<MDbTable> AvailableMemberships { get; set; }
        public List<DiscountDbTable> AvailableDiscountCodes { get; set; }
    }
}