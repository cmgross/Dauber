using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DAL;

namespace Dauber.Models
{
    public class AdminAllCoachesViewModel
    {
        public List<Coach> ActivatedCoaches { get; set; }
        public List<Coach> UnactivatedCoaches { get; set; }
        public List<Coach> InactiveCoaches { get; set; }

        public AdminAllCoachesViewModel()
        {
            List<Coach> coaches = Coach.GetAllCoaches().Select(c => Coach.GetCoachById(c.Id)).ToList();
            ActivatedCoaches = coaches.Where(c => c.Active && c.Clients.Count > 0).ToList();
            UnactivatedCoaches = coaches.Where(c => c.Active && c.Clients.Count == 0).ToList();
            InactiveCoaches = coaches.Where(c => !c.Active).ToList();
        }
    }

}