using GlobalGrub.Controllers;
using GlobalGrub.Data;
using GlobalGrub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalGrubTests
{
    // all test classes MUST BE public so add the public scope if it's not here when you create the class
    [TestClass]
    public class ProductsControllerTests
    {
        // class level vars shared by all tests
        private ApplicationDbContext _context;
        ProductsController controller;
        List<Product> products = new List<Product>();

        // this runs automatically before each test
        [TestInitialize]
        public void TestInitialize()
        {
            // create in-memory db
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);

            // populate db w/mock data
            var category = new Category
            {
                CategoryId = 500,
                Name = "My Dummy Category"
            };
            _context.Categories.Add(category);

            products.Add(new Product
            {
                ProductId = 741,
                Name = "The Best Taco Seasoning",
                Price = 9,
                CategoryId = 500,
                Category = category
            });

            products.Add(new Product
            {
                ProductId = 924,
                Name = "Delicious Food",
                Price = 19,
                CategoryId = 500,
                Category = category
            });

            products.Add(new Product
            {
                ProductId = 683,
                Name = "Special Sauce",
                Price = 7,
                CategoryId = 500,
                Category = category
            });            

            foreach (var product in products)
            {
                _context.Products.Add(product);
            }
            _context.SaveChanges();

            // instantiate controller w/db dependency
            controller = new ProductsController(_context);
        }

        #region Index

        [TestMethod]
        public void IndexLoadsIndexView()
        {
            // arrange - now done in TestInitialize instead            

            // act
            var result = (ViewResult)controller.Index().Result;

            // assert
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void IndexLoadsProducts()
        {
            // act
            var result = (ViewResult)controller.Index().Result;
            List<Product> model = (List<Product>)result.Model;

            // assert
            CollectionAssert.AreEqual(products.OrderBy(p => p.Name).ToList(), model);
        }

        #endregion

        #region Details

        [TestMethod]
        public void DetailsNullIdLoads404()
        {
            // act
            var result = (ViewResult)controller.Details(null).Result;

            // assert
            Assert.AreEqual("404", result.ViewName);
        }

        [TestMethod]
        public void DetailsInvalidIdLoads404()
        {
            // act
            var result = (ViewResult)controller.Details(1001).Result;

            // assert
            Assert.AreEqual("404", result.ViewName);
        }

        [TestMethod]
        public void DetailsValidIdLoadsProduct()
        {
            // act
            var result = (ViewResult)controller.Details(924).Result;

            // assert
            Assert.AreEqual(products[1], result.Model);
        }

        [TestMethod]
        public void DetailsValidIdLoadsView()
        {
            // act
            var result = (ViewResult)controller.Details(924).Result;

            // assert
            Assert.AreEqual("Details", result.ViewName);
        }

        #endregion


    }
}
