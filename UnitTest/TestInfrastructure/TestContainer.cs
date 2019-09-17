using System;
using Autofac;
using Moq;

namespace UnitTests.TestInfrastructure
{
    public class TestContainer : IDisposable
    {
        public IContainer Container { get; private set; }

        public TestContainer() : this(UnitTestBindings.CreateContainer(autoMock: true))
        {
        }

        protected TestContainer(IContainer container)
        {
            Container = container;
        }

        protected T Get<T>()
        {
            return Container.Resolve<T>();
        }

        protected Mock<T> GetMock<T>() where T : class
        {
            bool registerMock = false;
            if (Container.IsRegistered(typeof(T)))
            {
                try
                {
                    Mock.Get(Container.Resolve<T>());
                }
                catch
                {
                    registerMock = true;
                }
            }

            if (registerMock || !Container.IsRegistered(typeof(T)))
            {
                var mockInstance = (new Mock<T>() { CallBase = true }).Object;

                var updater = new ContainerBuilder();
                updater.RegisterInstance(mockInstance).As<T>();
#pragma warning disable CS0618 // Type or member is obsolete
                updater.Update(Container);
#pragma warning restore CS0618 // Type or member is obsolete
            }

            T instance = Container.Resolve<T>();
            return Mock.Get(instance);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Container != null)
            {
                Container.Dispose();
                Container = null;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }
    }
}