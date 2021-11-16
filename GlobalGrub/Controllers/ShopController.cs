using GlobalGrub.Data;
using GlobalGrub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalGrub.Controllers
{
    public class ShopController : Controller
    {
        // add DbContext to use the database
        private readonly ApplicationDbContext _context;

        // add config var so we can read vals from appsettings
        IConfiguration _iconfiguration;

        // constructor that takes an instance of our db connection & an app configuration object
        public ShopController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context; // assign incoming db connection so we can use it in any method in this controller
            _iconfiguration = configuration; // make config object available to all methods in this controller
        }

        // GET: /Shop
        public IActionResult Index()
        {
            // use Categories DbSet to fetch list of categories to display to shoppers
            var categories = _context.Categories.OrderBy(c => c.Name).ToList();
            return View(categories);
        }

        // GET: /Shop/ShopByCategory/5
        public IActionResult ShopByCategory(int id)
        {
            // get products in selected category
            var products = _context.Products.Where(p => p.CategoryId == id)
                .OrderBy(p => p.Name).ToList();

            // get name of selected category for display in page heading
            var category = _context.Categories.Find(id);
            ViewBag.Category = category.Name;

            return View(products);
        }

        // POST: /Shop/AddToCart
        [HttpPost]
        public IActionResult AddToCart(int ProductId, int Quantity) 
        {
            // look up price
            var product = _context.Products.Find(ProductId);
            var price = product.Price;

            // set UserId of cart item
            var userId = GetUserId();

            // check if item already in user's cart. if so, update quantity instead
            var cartItem = _context.CartItems
                .SingleOrDefault(c => c.ProductId == ProductId && c.UserId == userId);

            if (cartItem != null)
            {
                // update
                cartItem.Quantity += Quantity;
                _context.CartItems.Update(cartItem);
            }
            else
            {
                // insert
                cartItem = new CartItem
                {
                    ProductId = ProductId,
                    Quantity = Quantity,
                    Price = (double)price,
                    UserId = userId
                };

                // save to CartItems table in db
                _context.CartItems.Add(cartItem);
            }           

            _context.SaveChanges();

            // load cart page
            return RedirectToAction("Cart"); 
        }

        private string GetUserId()
        {
            // check session for an existing UserId for this user's cart
            if (HttpContext.Session.GetString("UserId") == null)
            {
                // this is user's 1st cart item
                var userId = "";
                if (User.Identity.IsAuthenticated)
                {
                    // user has logged in; use email 
                    userId = User.Identity.Name;
                }
                else
                {
                    // user anonymous.  generate unique identifier
                    userId = Guid.NewGuid().ToString();
                    // or use this: userId = HttpContext.Session.Id;
                }

                // store userId in a session var
                HttpContext.Session.SetString("UserId", userId);
            }

            return HttpContext.Session.GetString("UserId");
        }

        // GET: /Shop/Cart
        public IActionResult Cart()
        {
            // identity the user from the session var
            var userId = GetUserId();

            // load the cart items for this user from the db for display
            var cartItems = _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId).ToList();

            return View(cartItems);
        }

        // GET: /Shop/RemoveFromCart/5
        public IActionResult RemoveFromCart(int id)
        {
            var cartItem = _context.CartItems.Find(id);
            _context.CartItems.Remove(cartItem);
            _context.SaveChanges();
            return RedirectToAction("Cart");
        }

        // GET: /Shop/Checkout
        [Authorize]
        public IActionResult Checkout()
        {
            return View();
        }

        // POST: /Shop/Checkout
        [Authorize]
        [HttpPost]
        public IActionResult Checkout([Bind("FirstName,LastName,Address,City,Province,PostalCode,Phone")] Models.Order order)
        {
            // auto-fill total, date, user
            order.OrderDate = DateTime.Now;
            order.UserId = User.Identity.Name;
            order.Total = (from c in _context.CartItems
                           where c.UserId == order.UserId
                           select c.Quantity * c.Price).Sum();

            // save order to session so we can keep it in memory for saving once payment gets completed
            // using SessionExtensions 3rd party library for this
            HttpContext.Session.SetObject("Order", order);

            return RedirectToAction("Payment");
        }

        // GET: /Shop/Payment
        [Authorize]
        public IActionResult Payment()
        {
            return View();
        }

        // POST: /Shop/CreateCheckoutSession
        [Authorize]
        [HttpPost]
        public IActionResult CreateCheckoutSession()
        {
            // get order total
            var order = HttpContext.Session.GetObject<Models.Order>("Order");

            // set Stripe Secret Key
            StripeConfiguration.ApiKey = _iconfiguration.GetSection("Stripe")["SecretKey"];

            var options = new SessionCreateOptions
            {
              LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = ((long?)(order.Total * 100)),
                        Currency = "cad",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "GlobalGrub Purchase"
                        },
                    },
                    Quantity = 1
                  },
                },
                PaymentMethodTypes = new List<string>
                {
                  "card"
                },
                Mode = "payment",
                SuccessUrl = "https://" + Request.Host + "/Shop/SaveOrder",
                CancelUrl = "https://" + Request.Host + "/Shop/Cart",
            };

            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        // GET: /Shop/SaveOrder
        [Authorize]
        public IActionResult SaveOrder()
        {
            // save order to db
            var order = HttpContext.Session.GetObject<Models.Order>("Order");
            _context.Orders.Add(order);
            _context.SaveChanges();

            // save order items
            var cartItems = _context.CartItems.Where(c => c.UserId == order.UserId);

            foreach (var item in cartItems)
            {
                var orderItem = new Models.OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    OrderId = order.OrderId
                };
                _context.OrderItems.Add(orderItem);
            }
            _context.SaveChanges();

            // clear the cart
            foreach (var item in cartItems)
            {
                _context.CartItems.Remove(item);
            }
            _context.SaveChanges();

            // redirect to order details page
            return RedirectToAction("Details", "Orders", new { @id = order.OrderId });
        }
    }
}
