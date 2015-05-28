using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dauber.Models;
using DAL;
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
            if (ModelState.IsValid)
            {
                try
                {
                    StripeService.CreateCard(model.Token, model.CustomerId);//returns true
                    StripeService.UpdatePlan(model.CustomerId, model.SubscriptionId, model.PlanId);
                    Coach.UpdatePlan(model.CoachId, model.PlanId);
                    return RedirectToAction("Index", "MyAccount");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    return View("Error");
                }
            }
            //TODO change Stripe.cs methods from void to StripeResult and handle them in this form
            //TODO add blockUI around submitting form, creating token, etc
            //TODO pretty up stripe CC form by looking at Stripe examples
            //TODO validation of cc info using https://stripe.com/docs/stripe.js
            //TODO maybe validation via https://stripe.com/blog/jquery-payment https://stripe.com/docs/tutorials/forms
            //TODO Update Card page/action via API https://github.com/jaymedavis/stripe.net
            ViewBag.Error = "This submission could not be accepted as a required field was missing";
            return View("Error");
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