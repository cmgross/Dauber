using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dauber.Controllers
{
    [Attributes.RequireHttps]
    public class TestimonialsController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}