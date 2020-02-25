using System;
using System.Collections;
using System.Collections.Generic;

namespace Framework.Interfaces.Injection
{
    /// <summary>
    ///     Proxy to IOC container.
    /// </summary>
    public class Container : IContainer
    {
        private static IContainer instance;

        private readonly IContainer proxy;

        private Container(IContainer target)
        {
            proxy = target;
        }

        public static IContainer Instance
        {
            get
            {
                if (!Container.InstanceRegistered())
                {
                    throw new InvalidOperationException("Container has not been registered.");
                }

                return Container.instance;
            }
        }

        public bool ConfigurationIsValid(out string details)
        {
            return proxy.ConfigurationIsValid(out details);
        }

        public bool ConfigurationIsValid(Type type, out string details)
        {
            return proxy.ConfigurationIsValid(type, out details);
        }

        public T GetInstance<T>()
        {
            return proxy.GetInstance<T>();
        }

        public T GetInstance<T>(IEnumerable<KeyValuePair<string, object>> args)
        {
            return proxy.GetInstance<T>(args);
        }

        public T GetInstance<T>(string name)
        {
            return proxy.GetInstance<T>(name);
        }

        public object GetInstance(Type type)
        {
            return proxy.GetInstance(type);
        }

        public IEnumerable<T> GetAllInstances<T>()
        {
            return proxy.GetAllInstances<T>();
        }

        public IEnumerable GetAllInstances(Type type)
        {
            return proxy.GetAllInstances(type);
        }

        public void Release(object instance)
        {
            proxy.Release(instance);
        }

        public static void RegisterContainer(IContainer container)
        {
            if (Container.InstanceRegistered())
            {
                throw new InvalidOperationException("Container has already been registered.");
            }

            Container.instance = new Container(container);
        }

        public static void DeregisterContainer()
        {
            if (!Container.InstanceRegistered())
            {
                throw new InvalidOperationException("Container has not been registered.");
            }

            Container.Instance.Release(Container.Instance);
            Container.instance = null;
        }

        private static bool InstanceRegistered()
        {
            return Container.instance != null;
        }
    }
}