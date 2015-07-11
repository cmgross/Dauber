using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dauber.Models;
using DAL;
using Newtonsoft.Json;

namespace Dauber.Controllers
{
    [Authorize]
    public class ClientsController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var coach = Coach.Get(User.Identity.Name);
            if (coach.Active) return View(coach);
            ViewBag.Error = "Your account is not currently active. Please contact " + ConfigurationManager.AppSettings["InactiveMessage"] + "  for details.";
            return View("Error");
        }

        [HttpGet]
        public ActionResult Add(string coachId)
        {
            return View(new Client(coachId));
        }

        [HttpPost]
        public ActionResult Add(Client client)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    client.Save();
                    var coach = Coach.GetCoachById(client.UserId);
                    return RedirectToAction("Index", "Clients");
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

        [HttpGet]
        public ActionResult Edit(string clientId)
        {
            return View(Client.GetClient(clientId));
        }

        [HttpPost]
        public ActionResult Edit(Client client)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    client.Save();
                    var coach = Coach.GetCoachById(client.UserId);
                    return RedirectToAction("Index", "Clients");
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
        public ActionResult Delete(string clientId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Client.Delete(clientId);
                    return RedirectToAction("Index", "Clients");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    return View("Error");
                }
            }
            ViewBag.Error = "This request could not be processed as the form is invalid.";
            return View("Error");
        }

        [HttpGet]
        public JsonResult GetClientMatches(string clientUserName, string userId, string clientAction)
        {
            if (clientUserName == String.Empty) return new JsonResult
            {
                Data = string.Empty.ToArray(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

            var result = new JsonResult {JsonRequestBehavior = JsonRequestBehavior.AllowGet};
            switch (clientAction)
            {
                case "Add":
                    var newClientDiaryViewModel = new NewClientDiaryViewModel(clientUserName, userId);
                    result.Data = JsonConvert.SerializeObject(newClientDiaryViewModel);
                    break;
                case "Edit":
                    var clientDiaryViewModel = new ClientDiaryViewModel(clientUserName, userId);
                    result.Data = JsonConvert.SerializeObject(clientDiaryViewModel);
                    break;
            }
           
            return result;
        }
    }
}