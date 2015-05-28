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
    public class MyAccountUpdateViewModel
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

        public MyAccountUpdateViewModel() { }
        public MyAccountUpdateViewModel(string userName)
        {
            var coach = Coach.Get(userName);
            var customer = StripeService.GetCustomer(coach.StripeCustomerId);
            var plans = StripeService.GetPlans(); //TODO cache these!!

            Plans = plans.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id,
                Selected = p.Id == coach.PlanId
            }).ToList();

            PlanMax = coach.Plan.MaxClients;
            PlanCost = coach.Plan.Cost;

            //TODO fields not in use yet below
            UserName = coach.UserName; //TODO username is cardholder name for the token
            HasPaymentMethod = customer.StripeCardList.TotalCount > 0;
            CustomerId = customer.Id;
            PlanId = coach.PlanId;
            CoachId = coach.CoachId;
            NumberOfClients = coach.Clients.Count;
            ApiKey = ConfigurationManager.AppSettings["StripeApiKeyPublic"];
            var what = customer.StripeSubscriptionList.StripeSubscriptions[0].Id;

            //StripeService.UpdatePlan(CustomerId, what, "mithril");
            //need to deal with the Max clients per plan from plans database, and if coach has more than max
            //TODO use public API key for the javascript
        }
    }
}