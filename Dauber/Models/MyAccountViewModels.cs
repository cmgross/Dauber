using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAnnotationsExtensions;
using DAL;


namespace Dauber.Models
{
    public class MyAccountIndexViewModel
    {
        public string User { get; set; }
        [Display(Name = "Plan")]
        public string PlanDescription { get; set; }
        [Display(Name = "Current Clients")]
        public int CurrentClients { get; set; }
        [Display(Name = "Max Clients")]
        public int MaxClients { get; set; }
        [Display(Name = "Card On File")]
        public bool HasCardOnFile { get; set; }
        [Display(Name = "Card Type")]
        public string CardType { get; set; }
        [Display(Name = "Card Number")]
        public string CardNumber { get; set; }
        [Display(Name = "Expiration Date")]
        public string ExpirationDate { get; set; }

        public MyAccountIndexViewModel() { }

        public MyAccountIndexViewModel(string userName)
        {
            var coach = Coach.Get(userName);
            User = coach.UserName;
            PlanDescription = coach.Plan.Name;
            MaxClients = coach.Plan.MaxClients;
            CurrentClients = coach.Clients.Count;
            var customer = StripeService.GetCustomer(coach.StripeCustomerId);
            HasCardOnFile = customer.StripeCardList.TotalCount > 0;
            if (customer.StripeCardList.TotalCount == 0) return;
            var card = customer.StripeCardList.StripeCards[0];
            CardType = card.Brand;
            CardNumber = "************" + card.Last4;
            ExpirationDate = card.ExpirationMonth + "/" + card.ExpirationYear;
        }
    }
    public class MyAccountUpdatePlanViewModel
    {
        public bool HasPaymentMethod { get; set; }
        public string CustomerId { get; set; }
        public int CoachId { get; set; }
        [Display(Name = "Plan")]
        public string PlanId { get; set; }
        [Display(Name = "Current Clients")]
        public int NumberOfClients { get; set; }
        [Display(Name = "Max Clients")]
        public int PlanMax { get; set; }
        [Display(Name = "Monthly Cost")]
        public int PlanCost { get; set; }
        public List<SelectListItem> Plans { get; set; }
        public string ApiKey { get; set; }
        public string UserName { get; set; }
        public string SubscriptionId { get; set; }
        public string CurrentPlanId { get; set; }
        public MyAccountCardInfoViewModel CardInfoViewModel { get; set; }

        public MyAccountUpdatePlanViewModel() { }
        public MyAccountUpdatePlanViewModel(string userName)
        {
            var coach = Coach.Get(userName);
            var customer = StripeService.GetCustomer(coach.StripeCustomerId);
            var plans = StripeService.GetPlans();

            Plans = plans.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id,
                Selected = p.Id == coach.PlanId
            }).ToList();

            PlanMax = coach.Plan.MaxClients;
            PlanCost = coach.Plan.Cost;
            UserName = coach.UserName;
            HasPaymentMethod = customer.StripeCardList.TotalCount > 0;
            CustomerId = customer.Id;
            PlanId = coach.PlanId;
            CurrentPlanId = coach.PlanId;
            CoachId = coach.CoachId;
            NumberOfClients = coach.Clients.Count;
            ApiKey = ConfigurationManager.AppSettings["StripeApiKeyPublic"];
            SubscriptionId = customer.StripeSubscriptionList.StripeSubscriptions[0].Id;
            CardInfoViewModel = new MyAccountCardInfoViewModel();
        }
    }

    public class MyAccountCardInfoViewModel
    {
        [StringLength(16, MinimumLength = 12, ErrorMessage = "Credit card numbers must be 12-16 digits")]
        [DataAnnotationsExtensions.CreditCard]
        [Digits]
        [Required]
        [Display(Name = "Card Number")]
        public int CreditCardNumber { get; set; }
        [StringLength(3, MinimumLength = 3, ErrorMessage = "CVC must be 3 digits")]
        [Required]
        public int Cvc { get; set; }
        [Required]
        [Display(Name = "Expiry Month")]
        public string ExpiryMonth { get; set; }
        [Required]
        [Year]
        [Display(Name = "Expiry Year")]
        public string ExpiryYear { get; set; }
        public List<SelectListItem> ExpiryYears { get; set; }
        public List<SelectListItem> ExpiryMonths { get; set; }

        public MyAccountCardInfoViewModel()
        {
            var years = Enumerable.Range(DateTime.Now.Year, 15);
            ExpiryYears = years.Select(y => new SelectListItem
            {
                Text = y.ToString(),
                Value = y.ToString(),
                Selected = y.ToString() == DateTime.Now.Year.ToString()
            }).ToList();

            var months = Enumerable.Range(1, 12);
            ExpiryMonths = months.Select(m => new SelectListItem
            {
                Text = m.ToString(),
                Value = m.ToString(),
                Selected = m.ToString() == "12"
            }).ToList();
        }
    }
    public class MyAccountUpdatePlanSubmisionViewModel
    {
        public string Token { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string PlanId { get; set; }
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public string SubscriptionId { get; set; }
        [Required]
        public string CoachId { get; set; }

    }

    public class MyAccountUpdateCardViewModel
    {
        public bool HasPaymentMethod { get; set; }
        public string CustomerId { get; set; }
        public int CoachId { get; set; }
        public string CardId { get; set; }
        public MyAccountCardInfoViewModel CardInfoViewModel { get; set; }
        public string ApiKey { get; set; }
        public string UserName { get; set; }

        public MyAccountUpdateCardViewModel(string userName)
        {
            var coach = Coach.Get(userName);
            var customer = StripeService.GetCustomer(coach.StripeCustomerId);
            CardInfoViewModel = new MyAccountCardInfoViewModel();
            ApiKey = ConfigurationManager.AppSettings["StripeApiKeyPublic"];
            UserName = coach.UserName;
            HasPaymentMethod = customer.StripeCardList.TotalCount > 0;
            CoachId = coach.CoachId;
            CustomerId = customer.Id;
            if (customer.StripeCardList.TotalCount == 0) return;
            CardId = customer.StripeCardList.StripeCards[0].Id;
        }
    }

    public class MyAccountUpdateCardSubmissionViewModel
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public string CardId { get; set; }
        [Required]
        public string CoachId { get; set; }
        [Required]
        public bool HasPaymentMethod { get; set; }
    }
}