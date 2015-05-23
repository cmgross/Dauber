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
        public List<Coach> ActiveCoaches { get; set; }
        public List<Coach> InactiveCoaches { get; set; }

        public AdminAllCoachesViewModel()
        {
            var coaches = Coach.GetAllCoaches();
            ActiveCoaches = coaches.Where(c => c.Active).ToList();
            InactiveCoaches = coaches.Where(c => !c.Active).ToList();
        }
    }
}