using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HabrCacheQuery.ExampleQuery;
using HabrCacheQuery.Query;
using HabrCacheQuery.ServiceCollectionExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using MockRepository = HabrCacheQuery.ExampleQuery.MockRepository;

namespace Tests
{
    public class FodyCacheCacheTests : CacheUsingCoreContainerBaseTests
    {
        [SetUp]
        public void Setup()
        {
            using (var service = ServiceScope.ServiceProvider.CreateScope())
            {
                AsyncQuery = service.ServiceProvider.GetService<IAsyncQuery<StubForFodyCanCacheMySelf, Something>>();
                Query = service.ServiceProvider.GetService<IQuery<StubForFodyCanCacheMySelf, Something>>();
            }
        }

        [Test]
        public async Task AsyncQueryTest()
        {
            var stub = new StubForFodyCanCacheMySelf();
            await AsyncQuery.Query(stub);
            var task = AsyncQuery.Query(stub);
            Assert.True(task.IsCompleted);
        }

        [Test]
        public void QueryTest()
        {
            var stub = new StubForFodyCanCacheMySelf();
            Query.Query(stub);
            Query.Query(stub);
            RepositoryMock.Verify(x => x.GetSomething(), Times.Once);
        }

        private IQuery<StubForFodyCanCacheMySelf, Something> Query { get; set; }
        private IAsyncQuery<StubForFodyCanCacheMySelf, Something> AsyncQuery { get; set; }

    }
}