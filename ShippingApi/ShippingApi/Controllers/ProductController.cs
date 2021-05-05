using Couchbase.Core;
using Couchbase.Extensions.DependencyInjection;

using Couchbase.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using ShippingApi.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ShippingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly ILogger<ProductController> _logger;

        private readonly INamedBucketProvider _bucketProvider;

        public ProductController(INamedBucketProvider bucketProvider, ILogger<ProductController> logger = null)
        {
            _logger = logger;
            _bucketProvider = bucketProvider;

            // In production I would load data differently.
            Task.Run(async () => await LoadProducts()).Wait();
        }

        [HttpPost]
        [Route("/products/insert")]
        public async Task<string> InsertProduct(Product product)
        {
            var key = Guid.NewGuid().ToString();
            var context = new BucketContext(await _bucketProvider.GetBucketAsync());

            var collection = context.Bucket.DefaultCollection();

            var upsertResult = await collection.UpsertAsync(key, product);

            return await Task.FromResult(key);
        }

        [HttpGet]
        [Route("/products/getall")]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var context = new BucketContext(await _bucketProvider.GetBucketAsync());

            var results = context.Query<Product>().ToList();

            foreach (var p in results)
            {
                Console.WriteLine($"{p.ProductId}");
                Console.WriteLine($"{p.ProductName}");
                Console.WriteLine($"{p.InventoryQuantity}");
                Console.WriteLine($"{p.ShipOnWeekends}");
                Console.WriteLine($"{p.MaxBusinessDaysToShip}");
            }

            return Ok(results);
        }

        /// <summary>
        /// This endpoint exists because business logic should generally be kept server side.
        /// This endpoint is currently unused but could be useful.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/products/shipdate")]
        public async Task<ActionResult<DateTime>> GetProductShipDate(string productId)
        {
            var context = new BucketContext(await _bucketProvider.GetBucketAsync());

            var product = context.Query<Product>().Where(p => p.ProductId.ToString() == productId).FirstOrDefault();

            if(product == null)
            {
                return NotFound();
            }

            var shipDate = GetShipDate(product);

            return Ok(shipDate);
        }

        private async Task LoadProducts()
        {
            var context = new BucketContext(await _bucketProvider.GetBucketAsync());

            var collection = context.Bucket.DefaultCollection();

            var products = new List<Product>()
            {
                new Product()
                {
                    ProductId = 1,
                    ProductName = "fugiat exercitation adipisicing",
                    InventoryQuantity = 43,
                    ShipOnWeekends = true,
                    MaxBusinessDaysToShip = 13
                },
                new Product()
                {
                    ProductId = 2,
                    ProductName = "mollit cupidatat Lorem",
                    InventoryQuantity = 70,
                    ShipOnWeekends = true,
                    MaxBusinessDaysToShip = 18
                },
                new Product()
                {
                    ProductId = 3,
                    ProductName = "non quis sint",
                    InventoryQuantity = 33,
                    ShipOnWeekends = false,
                    MaxBusinessDaysToShip = 15
                },
                new Product()
                {
                    ProductId = 4,
                    ProductName = "voluptate cupidatat non",
                    InventoryQuantity = 52,
                    ShipOnWeekends = false,
                    MaxBusinessDaysToShip = 18
                },
                new Product()
                {
                    ProductId = 5,
                    ProductName = "anim amet occaecat",
                    InventoryQuantity = 39,
                    ShipOnWeekends = true,
                    MaxBusinessDaysToShip = 19
                },
                new Product()
                {
                    ProductId = 6,
                    ProductName = "cillum deserunt elit",
                    InventoryQuantity = 47,
                    ShipOnWeekends = false,
                    MaxBusinessDaysToShip = 20
                },
                new Product()
                {
                    ProductId = 7,
                    ProductName = "adipisicing reprehenderit et",
                    InventoryQuantity = 71,
                    ShipOnWeekends = false,
                    MaxBusinessDaysToShip = 15
                },
                new Product()
                {
                    ProductId = 8,
                    ProductName = "ex mollit laboris",
                    InventoryQuantity = 80,
                    ShipOnWeekends = false,
                    MaxBusinessDaysToShip = 15
                },
            };

            foreach(var product in products)
            {
                var key = $"{product.ProductId}";

                product.ShipDate = GetShipDate(product);
                var upsertResult = await collection.UpsertAsync(key, product);
            }
        }

        /// <summary>
        /// Helper method to calculate ship dates.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        private DateTime GetShipDate(Product product)
        {
            var orderDate = DateTime.Now.Date;

            DateTime shipDate = orderDate.Date.AddDays(product.MaxBusinessDaysToShip);

            // add days here to account for weekends
            if (!product.ShipOnWeekends)
            {
                if(orderDate.DayOfWeek != DayOfWeek.Saturday || orderDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    shipDate = shipDate.AddDays(-1);
                }
                for (int i = 0; i < product.MaxBusinessDaysToShip; i++)
                {
                    var date = orderDate.AddDays(i).Date;
                    var dayOfWeek = date.DayOfWeek;
                    if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Friday)
                    {
                        if (date.DayOfWeek == DayOfWeek.Saturday)
                        {
                            shipDate = shipDate.AddDays(2);
                        }
                        if (date.DayOfWeek == DayOfWeek.Friday)
                        {
                            shipDate = shipDate.AddDays(3);
                        }
                    }
                }
            }
            else
            {
                shipDate = shipDate.AddDays(-1);
            }

            return shipDate;
        }
    }
}
