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
    public class DiaryController : ApiController
    {
        //GET api/v1/diary
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        //GET api/v1/diary/1/chuckgross/03-08-2015
        public HttpResponseMessage Get(string id, string userName, DateTime date)
        {
            int coachId;
            int.TryParse(id, out coachId);
            if (coachId == 0) return Request.CreateResponse(HttpStatusCode.BadRequest);
            var coach = Cacheable.GetCoach(coachId);
            if (!coach.Active) return Request.CreateResponse(HttpStatusCode.BadRequest);
            if (!coach.Clients.Select(c => c.ClientUserName).Contains(userName)) return Request.CreateResponse(HttpStatusCode.BadRequest);
            var formattedDate = date.ToString("yyyy-MM-dd");  //date format is going to come in as MM/DD/YYYY, reformat it to YYYY-MM-DD
            var results = MFP.Scrape.GetMacros(userName, formattedDate);
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(results.ToString()) };
            responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            responseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "results.csv"
            };
            return responseMessage;
        }
    }
}
