using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using DAL;

namespace Dauber.Controllers.v1
{
    public class FitocracyController : ApiController
    {
        //GET api/v1/fitocracy
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        //GET api/v1/diary/1/fitocracy/03-08-2015
        public HttpResponseMessage Get(string id, string userName, DateTime date)
        {
            int coachId;
            int.TryParse(id, out coachId);
            if (coachId == 0) return Request.CreateResponse(HttpStatusCode.BadRequest);

            var coach = Cacheable.GetCoach(coachId);
            //var coach = Coach.Get(coachId);
            if (!coach.Active) return Request.CreateResponse(HttpStatusCode.BadRequest);
            if (!coach.Clients.Select(c => c.FitocracyUserName).Contains(userName)) return Request.CreateResponse(HttpStatusCode.BadRequest);
           
            var user = coach.Clients.FirstOrDefault(c => c.FitocracyUserName == userName);
            if (user == null) return Request.CreateResponse(HttpStatusCode.BadRequest);
            var recentWorkouts = Fitocracy.Scrape.ScrapeActivityFeed(user.FitocracyId.ToString());
            var results = recentWorkouts.Count(w => w >= (date.AddDays(-7)));

            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(results.ToString()) };
            return responseMessage;
        }
    }
}
