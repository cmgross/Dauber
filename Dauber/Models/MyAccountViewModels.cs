using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

        public MyAccountIndexViewModel() { }

        public MyAccountIndexViewModel(string userName)
        {
            var coach = Coach.Get(userName);
            User = coach.UserName;
            PlanDescription = coach.Plan.Name;
            MaxClients = coach.Plan.MaxClients;
            CurrentClients = coach.Clients.Count;
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
        }
    }

    public class MyAccountUpdatePlanSubmisionViewModel
    {
        [Required]
        public string Token { get; set; } //to process charge we need a token,  and a username
        [Required]
        public string UserName { get; set; }
        [Required]
        public string PlanId { get; set; }//to update user's plan and charge it, we need planId, customerId, subscription Id
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public string SubscriptionId { get; set; }
        [Required]
        public string CoachId { get; set; } //to update a user in dauber, we need a planId, and coachId

    }
}