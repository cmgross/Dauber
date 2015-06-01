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
                    if (model.Token != null)
                    {
                        var cardResult = StripeService.CreateCard(model.Token, model.CustomerId);
                        if (!cardResult.Success)
                        {
                            ViewBag.Error = cardResult.Error;
                            return View("Error");
                        }
                    }

                    var planResult = StripeService.UpdatePlan(model.CustomerId, model.SubscriptionId, model.PlanId);
                    if (!planResult.Success)
                    {
                        ViewBag.Error = planResult.Error;
                        return View("Error");
                    }

                    Coach.UpdatePlan(model.CoachId, model.PlanId);
                    return RedirectToAction("Index", "MyAccount");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    return View("Error");
                }
            }
            //TODO Add new card with new expiration 
            //TODO http://stackoverflow.com/questions/20065939/change-credit-card-information-stripe
            //TODO Update Card page/action via API https://github.com/jaymedavis/stripe.net,
            //TODO testimonials page
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
            var plan = Plan.Get(planId);
            var result = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = JsonConvert.SerializeObject(plan)
            };
            return result;
        }
    }
}