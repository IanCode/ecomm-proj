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
using ShippingApi.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
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

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task InsertProduct()
        {
            var client = factory.CreateClient();

            var product3 = new Product()
            {
                ProductId = 12,
                ProductName = "Test Product 3",
                InventoryQuantity = 23,
                ShipOnWeekends = false,
                MaxBusinessDaysToShip = 33
            };

            var response = await client.PostAsync("http://localhost:5000/products/insert", new StringContent(JsonConvert.SerializeObject(product3), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task TestShipDates()
        {
            //todo: add test cases for weekdays, weekends, shipOnWeekends as true/false
            var client = factory.CreateClient();

            // load test products

            // expected shipdate 4/14/2021
            var expectedDate1 = new DateTime(2021, 4, 14).Date;
            var product1 = new Product()
            {
                ProductId = 10,
                ProductName = "Test Product 1",
                InventoryQuantity = 34,
                ShipOnWeekends = false,
                MaxBusinessDaysToShip = 8,
                OrderDate = new DateTime(2021, 4, 5)
            };

            // expected shipdate 4/19/2021
            var expectedDate2 = new DateTime(2021, 4, 19).Date;
            var product2 = new Product()
            {
                ProductId = 11,
                ProductName = "Test Product 2",
                InventoryQuantity = 80,
                ShipOnWeekends = true,
                MaxBusinessDaysToShip = 10,
                OrderDate = new DateTime(2021, 4, 10)
            };

            var content1 = new StringContent(JsonConvert.SerializeObject(product1), Encoding.UTF8, "application/json");
            var response1 = await client.PostAsync("http://localhost:5000/products/insert", content1);

            var content2 = new StringContent(JsonConvert.SerializeObject(product2), Encoding.UTF8, "application/json");
            var response2 = await client.PostAsync("http://localhost:5000/products/insert", content2);

            var responseContent1 = response1.Content.ReadAsStringAsync().Result;
            var responseContent2 = response2.Content.ReadAsStringAsync().Result;

            var responseProduct1 = JsonConvert.DeserializeObject<Product>(responseContent1);
            var responseProduct2 = JsonConvert.DeserializeObject<Product>(responseContent2);

            Assert.Equal(expectedDate1, responseProduct1.ShipDate.Date);
            Assert.Equal(expectedDate2, responseProduct2.ShipDate.Date);
           
        }

    }
}
