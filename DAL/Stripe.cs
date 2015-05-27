using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stripe;

namespace DAL
{
    public class Stripe
    {
        public static StripeResult CreateCustomer(string userName, string email, string planId)
        {
            try
            {
                var myCustomer = new StripeCustomerCreateOptions
                {
                    Email = email,
                    Description = userName,
                    PlanId = planId
                };
                var customerService = new StripeCustomerService { ApiKey = ConfigurationManager.AppSettings["StripeApiKey"] };
                StripeCustomer stripeCustomer = customerService.Create(myCustomer);
                return new StripeResult { Success = true, Error = string.Empty, CustomerId = stripeCustomer.Id };
            }
            catch (StripeException exception)
            {
                return new StripeResult { Error = exception.Message };
            }
        }
    }

    public class StripeResult
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public string CustomerId { get; set; }
    }
}
