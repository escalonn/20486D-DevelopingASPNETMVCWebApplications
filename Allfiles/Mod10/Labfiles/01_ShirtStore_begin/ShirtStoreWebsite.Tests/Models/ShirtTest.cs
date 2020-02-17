using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShirtStoreWebsite.Models;

namespace ShirtStoreWebsite.Tests.Models
{
    [TestClass]
    public class ShirtTest
    {
        [TestMethod]
        public void IsGetFormattedTaxedPriceReturnsCorrectly()
        {
            var shirt = new Shirt { Price = 10F, Tax = 1.2F };
            string taxedPrice = shirt.FormattedTaxedPrice;
            Assert.AreEqual("$12.00", taxedPrice);
        }
    }
}
