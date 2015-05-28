using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dauber.Models;
using Newtonsoft.Json;


namespace Dauber.Controllers
{
    [Attributes.RequireHttps]
    [Authorize]
    public class MyAccountController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View(new MyAccountIndexViewModel(User.Identity.Name));
        }

        [HttpGet]
        public ActionResult UpdatePlan()
        {
            return View(new MyAccountUpdatePlanViewModel(User.Identity.Name));
        }

        [HttpPost]
        public ActionResult UpdatePlan(MyAccountUpdatePlanSubmisionViewModel model)
        {
            //TODO http://stackoverflow.com/questions/8757963/avoiding-duplicate-form-submission-in-asp-net-mvc-by-clicking-submit-twice
            //TODO add card to customer StripeService.CreateCard(model.CustomerId, model.Token, model.username is Cardholder)
            //TODO update user plan 
            //TODO StripeService.UpdatePlan(CustomerId, what, "mithril");
            //TODO update user in local db to have a different plan
            //TODO try catch, error handling
            //TODO validation of cc info using https://stripe.com/docs/stripe.js
            return View();
        }

        [HttpGet]
        public ActionResult UpdateCard()
        {
            return View();
        }



        [HttpGet]
        public JsonResult GetPlan(string planId)
        {
            var plan = DAL.Plan.Get(planId);
            var result = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = JsonConvert.SerializeObject(plan)
            };
            return result;
        }
    }
}