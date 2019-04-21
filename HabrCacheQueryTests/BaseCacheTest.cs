using System;
using System.Linq;
using System.Reflection;
using HabrCacheQuery;
using HabrCacheQuery.ExampleQuery;
using HabrCacheQuery.ServiceCollectionExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MockRepository = HabrCacheQuery.ExampleQuery.MockRepository;

namespace Tests
{
    public class CacheUsingCoreContainerBaseTests : BaseCacheTest
    {
        protected CacheUsingCoreContainerBaseTests()
            : base(collection =>
            {
                collection.AddScoped<IRepository, MockRepository>(x => MockRepositoryObject);
                collection.AddCachedQueries();
            })
        {
        }
    }

    public class BaseCacheTest
    {
        private readonly Action<ServiceCollection> _cachedQueryRealization;
        protected IServiceScope ServiceScope => ServiceProvider.CreateScope();
        private IServiceProvider ServiceProvider { get; set; }


        protected BaseCacheTest(
            Action<IServiceCollection> registrations)
        {
            ServiceProviderInitial(registrations);
        }

        protected void ServiceProviderInitial(Action<IServiceCollection> Registrations)
        {
            RepositoryMock = new Mock<MockRepository>();
            var collection = new ServiceCollection();

            Registrations(collection);
            ServiceProvider = collection.BuildServiceProvider();
        }


        protected static Mock<MockRepository> RepositoryMock { get; private set; } = new Mock<MockRepository>();
        protected static MockRepository MockRepositoryObject => RepositoryMock.Object;

        protected static int CountCallRepositoryGetSomethingMethod =>
            RepositoryMock.Invocations.Count(x => x.Method.Name == nameof(IRepository.GetSomething));
    }
}