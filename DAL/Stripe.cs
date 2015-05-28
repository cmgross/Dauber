using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stripe;

namespace DAL
{
    public class StripeService
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

        public static StripeCustomer GetCustomer(string customerId)
        {
            //TODO need to do try catch here?
            var customerService = new StripeCustomerService { ApiKey = ConfigurationManager.AppSettings["StripeApiKey"] };
            return customerService.Get(customerId);
        }

        public static List<StripePlan> GetPlans()
        {
            var planService = new StripePlanService { ApiKey = ConfigurationManager.AppSettings["StripeApiKey"] };
            return planService.List().ToList(); // optional StripeListOptions
        }

        public static void UpdatePlan(string customerId, string subscriptionId, string planId, StripeToken token = null)
        {
            //TODO accept the token, optional parameter
            //TODO don't return void, everything needs to be in try/catch
            var subscriptionService = new StripeSubscriptionService{ ApiKey = ConfigurationManager.AppSettings["StripeApiKey"] };
            var subscriptionOptions = new StripeSubscriptionUpdateOptions();
            if (token != null)
            {
                subscriptionOptions.Card = new StripeCreditCardOptions{TokenId = token.Id};
            }
            subscriptionOptions.PlanId = planId;
            //TODO customer has no attached payment source!!!
            StripeSubscription stripeSubscription = subscriptionService.Update(customerId, subscriptionId, subscriptionOptions);
        }
    }

    public class StripeResult
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public string CustomerId { get; set; }
    }
}
