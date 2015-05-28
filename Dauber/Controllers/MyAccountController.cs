using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dauber.Models;


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
        public ActionResult Plan()
        {
            return View(new MyAccountUpdateViewModel(User.Identity.Name));
        }
    }
}