using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models.ViewModels
{
    public class CheckoutViewModel
    {
        // Shipping Information
        //[Required(ErrorMessage = "Full name is required")]
        [Display(Name = "Full Name")]
        [MaxLength(100)]
        public string ShippingFullName { get; set; } = string.Empty;

       // [Required(ErrorMessage = "Address is required")]
        [Display(Name = "Shipping Address")]
        [MaxLength(200)]
        public string ShippingAddress { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Phone number is required")]
        [Display(Name = "Phone Number")]
        [MaxLength(20)]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        public string ShippingPhone { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Email is required")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [MaxLength(100)]
        public string ShippingEmail { get; set; } = string.Empty;

        // Payment Method
       // [Required(ErrorMessage = "Please select a payment method")]
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; } = "Cash"; // Cash, Instapay, Wallet

        // Cart Summary
        public List<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();
        public decimal CartTotal { get; set; }
        public int CartCount { get; set; }

        // User info (for pre-filling)
        public string? UserEmail { get; set; }
        public string? UserFullName { get; set; }
        public string? UserPhone { get; set; }
    }
}