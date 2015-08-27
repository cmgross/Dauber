using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using Dauber.Attributes;
using Dauber.Models;
using DAL;

namespace Dauber.Controllers
{
    [IsAdmin]
    [Authorize]
    public class AdminController : Controller
    {
        // GET: Admin
        [HttpGet]
        public ActionResult Index()
        {
            return View(new AdminAllCoachesViewModel());
        }

        [HttpPost]
        public ActionResult ChangePartner(Coach coach)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Coach.ChangePartner(coach.Id, !coach.Partner);
                    Cacheable.PurgeCoach(coach.CoachId);
                    return RedirectToAction("Index", "Admin");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    return View("Error");
                }
            }
            ViewBag.Error = "This submission could not be accepted as a required field was missing";
            return View("Error");
        }

        [HttpPost]
        public ActionResult ChangeActive(Coach coach)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Coach.ChangeActive(coach.Id, !coach.Active);
                    Cacheable.PurgeCoach(coach.CoachId);
                    return RedirectToAction("Index", "Admin");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    return View("Error");
                }
            }
            ViewBag.Error = "This submission could not be accepted as a required field was missing";
            return View("Error");
        }

        [HttpPost]
        public ActionResult Delete(Coach coach)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Coach.Delete(coach.Id);
                    Cacheable.PurgeCoach(coach.CoachId);
                    return RedirectToAction("Index", "Admin");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    return View("Error");
                }
            }
            ViewBag.Error = "This submission could not be accepted as a required field was missing";
            return View("Error");
        }

    }
}