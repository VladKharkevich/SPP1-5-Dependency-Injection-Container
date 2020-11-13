using ClassExamples;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dependency_Injection_Container.Tests
{
    [TestClass]
    public class ContainerTests
    {
        DependencyProvider provider;

        [TestInitialize]
        public void TestInit()
        {
            var dependencies = new DependenciesConfiguration();
            dependencies.Register<IService, Service>();
            dependencies.Register<IRepository, Repository>();
            dependencies.Register<Repository, Repository>(true);
            dependencies.Register<AbstractServ, Serv1>();
            dependencies.Register<AbstractServ, Serv2>();
            dependencies.Register<Temp, Temp>();

            dependencies.Register<Repository, TRepository>();
            dependencies.Register<IFoo<IRepository>, Foo<IRepository>>();
            provider = new DependencyProvider(dependencies);
        }

        [TestMethod]
        public void TestResolve()
        {
            IRepository repos = provider.Resolve<IRepository>();
            repos.dzen().Should().Be(34);
        }

        [TestMethod]
        public void TestSingleton()
        {
            Repository repos1 = provider.Resolve<Repository>();
            Repository repos2 = provider.Resolve<Repository>();
            repos1.Should().BeSameAs(repos2);

            IRepository repos3 = provider.Resolve<IRepository>();
            IRepository repos4 = provider.Resolve<IRepository>();

            repos3.Should().NotBeSameAs(repos4);
        }

        [TestMethod]
        public void TestResolveAll()
        {
            IEnumerable<AbstractServ> serv = provider.ResolveAll<AbstractServ>();
            serv.Count().Should().Be(2);

            serv.First().GetInt().Should().Be(1);
            serv.Last().GetInt().Should().Be(2);

            IEnumerable<IRepository> repos = provider.ResolveAll<IRepository>();
            repos.Count().Should().Be(1);
        }

        [TestMethod]
        public void TestRecursionConstructor()
        {
            IService service = provider.Resolve<IService>();
            service.repository.dzen().Should().Be(34);
        }

        [TestMethod]
        public void TestValidator()
        {
            var newDependencies = new DependenciesConfiguration();
            newDependencies.Register<IBar, Repository>();

            var newProvider = new DependencyProvider(newDependencies);

            IBar temp = newProvider.Resolve<IBar>();
            temp.Should().Be(null);
        }

        [TestMethod]
        public void TestDependenceGenericType()
        {
            IFoo<IRepository> temp = provider.Resolve<IFoo<IRepository>>();
            var repTemp = temp.GetRepository();
            repTemp.dzen().Should().Be(34);
        }

        [TestMethod]
        public void TestOpenGenerics()
        {
            var newDependencies = new DependenciesConfiguration();
            newDependencies.Register<IRepository, Repository>();
            newDependencies.Register<IRepository, TRepository>();
            newDependencies.Register(typeof(IFoo<>), typeof(Foo<>));

            var newProvider = new DependencyProvider(newDependencies);

            IFoo<IRepository> temp = newProvider.Resolve<IFoo<IRepository>>();
            var repTemp = temp.GetRepository();
            repTemp.dzen().Should().Be(34);
        }

        [TestMethod]
        public void TestUnregisteredInterface()
        {
            IBar temp = provider.Resolve<IBar>();
            temp.Should().Be(null);
        }
    }
}
 