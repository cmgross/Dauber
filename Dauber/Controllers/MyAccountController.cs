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
            //TODO for limiting number of clients being added
            //TODO review notes to see if I missed anything
            //TODO Update Card page/action via API https://github.com/jaymedavis/stripe.net, no card on file? Say so. Ask to add one? or just mention card will be added when changing plan
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