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
                        Coach.UpdateCard(model.CoachId, cardResult.CardId);
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
            //TODO testimonials page http://bootsnipp.com/snippets/featured/responsive-quote-carousel //should i write/store interface for testimonials or just hard code them?
            //TODO switch plans page to be dynamic rather than static?
            //TODO conditions terms of service etc.
            ViewBag.Error = "This submission could not be accepted as a required field was missing";
            return View("Error");
        }

        [HttpGet]
        public ActionResult UpdateCard()
        {
            return View(new MyAccountUpdateCardViewModel(User.Identity.Name));
        }

        [HttpPost]
        public ActionResult UpdateCard(MyAccountUpdateCardSubmissionViewModel model)
        {
            //if !model.haspaymentplan, don't delete
            //create view model that has token, customerId, cardId
            //delete card needs customerId, cardId, check if result successful
            //set cardId to null in db for customer, needs customerId
            //create card needs token, customerId
            //set cardId to cardId in db for customer, needs customerId, and cardId
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