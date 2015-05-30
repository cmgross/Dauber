using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dauber.Controllers
{
    [Attributes.RequireHttps]
    //[AllowAnonymous]
    //[OverrideAuthentication]
    public class PlansController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}