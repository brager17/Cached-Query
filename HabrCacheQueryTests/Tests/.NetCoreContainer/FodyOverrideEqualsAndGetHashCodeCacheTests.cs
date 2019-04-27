using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
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
    public class FodyOverrideEqualsAndGetHashCodeCacheTests : CacheUsingCoreContainerBaseTests
    {
        protected override void QueryInitial()
        {
            using (Scope)
            {
                AsyncQuery = Scope.ServiceProvider.GetService<IAsyncQuery<StubForFodyCanCacheMySelf, Something>>();
                Query = Scope.ServiceProvider.GetService<IQuery<StubForFodyCanCacheMySelf, Something>>();
            }
        }

        [Test]
        public async Task AsyncQueryTest()
        {
            var stub = new StubForFodyCanCacheMySelf();
            var s = AsyncQuery.Query(stub);
            var ss = s.Status;
            Thread.Sleep(3000);
            Assert.True(s.IsCompleted);
        }

        [Test]
        public void QueryTest()
        {
            var stub = new StubForFodyCanCacheMySelf();
            Query.Query(stub);
            Query.Query(stub);
            VerifyOneCall();
        }

        private IQuery<StubForFodyCanCacheMySelf, Something> Query { get; set; }
        private IAsyncQuery<StubForFodyCanCacheMySelf, Something> AsyncQuery { get; set; }
    }
}