using GlobalGrub.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GlobalGrubTests
{
    [TestClass]
    public class DummiesControllerTest
    {
        [TestMethod]
        public void IndexReturnsSomething()
        {
            // arrange - set up any objects / vars / params needed to call the method we want to test
            var controller = new DummiesController();

            // act - execute the method
            var result = controller.Index();

            // assert - evaluate if we got the result we expected
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void IndexLoadsIndexView()
        {
            // arrange - set up any objects / vars / params needed to call the method we want to test
            var controller = new DummiesController();

            // act - execute the method. method returns IActionResult, we must cast it to the child class ViewResult
            var result = (ViewResult)controller.Index();

            // assert - did we get the correct view?
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void IndexViewDataShowsMessage()
        {
            // arrange
            var controller = new DummiesController();

            // act
            var result = (ViewResult)controller.Index();

            // assert
            Assert.AreEqual("This is a viewdata message", result.ViewData["Message"]);
        }
    }
}
