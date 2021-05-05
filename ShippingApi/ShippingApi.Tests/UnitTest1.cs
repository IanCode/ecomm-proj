using ShippingApi.Controllers;
using System;
using Xunit;
using Neon.Xunit;
using Neon.Xunit.Couchbase;

using Microsoft.Extensions.Logging;
using Couchbase.Linq;
//using Couchbase.Extensions.DependencyInjection;

namespace ShippingApi.Tests
{
    public class UnitTest1
    {
        //private ComposedFixture composedFixture;
        private BucketContext bucketContext;

        private readonly CouchbaseFixture couchbaseFixture;

        [Fact]
        public void GetProducts()
        {
            //var controller = new ProductController();
        }

        [Fact]
        public void InsertProduct()
        {
            //var controller = new ProductController(new Logger<ProductController>(), )
        }
    }
}
