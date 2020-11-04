using ClassExamples;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dependency_Injection_Container.Tests
{
    [TestClass]
    public class ContainerTests
    {
        Service service;

        [TestInitialize]
        public void TestInit()
        {
            var dependencies = new DependenciesConfiguration();
            dependencies.Register<IService, Service>();
            dependencies.Register<IRepository, Repository>();
            dependencies.Register<AbstractServ, Serv>();
            dependencies.Register<Temp, Temp>();

            var provider = new DependencyProvider(dependencies);
            service = (Service)provider.Resolve<IService>();
            var serv = provider.Resolve<AbstractServ>();
            var temp = provider.Resolve<Temp>();
        }

        [TestMethod]
        public void TestMy()
        {
            
        }
    }
}
