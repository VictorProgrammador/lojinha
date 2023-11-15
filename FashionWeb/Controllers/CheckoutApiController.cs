using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stripe;
using Stripe.Checkout;

namespace FashionWeb.Controllers
{
    [Route("create-checkout-session")]
    [ApiController]
    public class CheckoutApiController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateOrder()
        {
            var domain = "http://localhost:44381/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    // Provide the exact Price ID (for example, pr_1234) of the product you want to sell
                    Price = "price_1Nu55rIFFx4wIOSRJU0QdQhK",
                    Quantity = 1,
                  },
                },
                Mode = "payment",
                SuccessUrl = domain + "User" + "/SuccessOrder",
                CancelUrl = domain + "User" + "/CanceledOrder",
            };
            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
    }
}
