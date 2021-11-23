using GlobalGrub.Controllers;
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
    }
}
