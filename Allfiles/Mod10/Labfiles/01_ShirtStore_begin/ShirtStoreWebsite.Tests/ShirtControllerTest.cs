using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShirtStoreWebsite.Controllers;
using ShirtStoreWebsite.Models;
using ShirtStoreWebsite.Services;
using ShirtStoreWebsite.Tests.FakeRepositories;
using System.Collections.Generic;
using System.Linq;

namespace ShirtStoreWebsite.Tests
{
    [TestClass]
    public class ShirtControllerTest
    {
        [TestMethod]
        public void IndexModelShouldContainAllShirts()
        {
            IShirtRepository fakeShirtRepository = new FakeShirtRepository();
            var mockLogger = new Mock<ILogger<ShirtController>>();
            var shirtController = new ShirtController(fakeShirtRepository, mockLogger.Object);

            var viewResult = shirtController.Index() as ViewResult;

            var shirts = viewResult.Model as IEnumerable<Shirt>;
            Assert.AreEqual(expected: 3, actual: shirts.Count());
        }
    }
}
