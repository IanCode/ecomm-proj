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
            //Task.Run(async () => await LoadProducts()).Wait();
        }

        [HttpPost]
        [Route("/products/insert")]
        public async Task<ActionResult<Product>> InsertProduct(Product product)
        {
            var context = new BucketContext(await _bucketProvider.GetBucketAsync());

            var collection = context.Bucket.DefaultCollection();

            var key = $"{product.ProductId}";

            var p = GetShipDate(product);

            var upsertResult = await collection.UpsertAsync(key, p);

            return Ok(p);
        }

        [HttpGet]
        [Route("/products/getall")]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var context = new BucketContext(await _bucketProvider.GetBucketAsync());

            var collection = context.Bucket.DefaultCollection();

            var results = context.Query<Product>().ToList();

            if(results.Count == 0)
            {
                return NotFound();
            }

            for(int i = 0; i < results.Count; i++)
            {
                var p = results[i];

                // default order date functionality
                p.OrderDate = DateTime.Now.Date;
                p.OrderDateFormatted = p.OrderDate.ToShortDateString();

                // default ship date functionality
                p = GetShipDate(p);

                Console.WriteLine($"{p.ProductId}");
                Console.WriteLine($"{p.ProductName}");
                Console.WriteLine($"{p.InventoryQuantity}");
                Console.WriteLine($"{p.ShipOnWeekends}");
                Console.WriteLine($"{p.MaxBusinessDaysToShip}");
                Console.WriteLine($"orderdate: {p.OrderDateFormatted}");
                Console.WriteLine($"shipdate: {p.ShipDateFormatted}");

                // update the product in the db
                var key = $"{p.ProductId}";
                var upsertResult = await collection.UpsertAsync(key, p);

                results[i] = p;
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

        /// <summary>
        /// Helper method to calculate ship dates.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        private Product GetShipDate(Product product)
        {
            var orderDate = DateTime.Now.Date;

            if (product.OrderDate != null)
            {
                orderDate = product.OrderDate;
            }

            var actualDaysToShip = product.MaxBusinessDaysToShip;

            // add days here to account for weekends
            if (!product.ShipOnWeekends)
            {
                var extraDays = 0;

                for (int i = 0; i < product.MaxBusinessDaysToShip; i++)
                {
                    var date = orderDate.AddDays(i + extraDays).Date;
                    var dayOfWeek = date.DayOfWeek;
                    if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
                    {
                        if (date.DayOfWeek == DayOfWeek.Saturday)
                        {
                            extraDays++;
                        }
                        if (date.DayOfWeek == DayOfWeek.Sunday)
                        {
                            extraDays++;
                        }
                    }
                }
                actualDaysToShip += extraDays;
            }
            else
            {
                //order date inclusive
                actualDaysToShip--;
            }

            product.ShipDate = orderDate.AddDays(actualDaysToShip).Date;
            product.ShipDateFormatted = product.ShipDate.ToShortDateString();

            return product;
        }
    }
}
