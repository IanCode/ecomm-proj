using ShippingApi.Controllers;
using System;
using Xunit;
using Neon.Xunit;
using Neon.Xunit.Couchbase;

using Microsoft.Extensions.Logging;
using Couchbase.Linq;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using System.Net;
//using Couchbase.Extensions.DependencyInjection;

namespace ShippingApi.Tests
{
    public class UnitTest1 : IClassFixture<WebApplicationFactory<ShippingApi.Startup>>
    {
        private readonly WebApplicationFactory<ShippingApi.Startup> factory;

        public UnitTest1(WebApplicationFactory<Startup> _factory)
        {
            factory = _factory;
        }

        [Fact]
        public async Task GetProducts()
        {
            var client = factory.CreateClient();

            var response = await client.GetAsync("http://localhost:5000/products/getall");

            var products = response.Content;

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void InsertProduct()
        {
        }

        [Fact]
        public void TestShipDates()
        {
            //todo: add test cases for weekdays, weekends, shipOnWeekends as true/false
        }

    }
}
