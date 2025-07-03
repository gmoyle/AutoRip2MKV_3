using System;
using System.Collections.Generic;

namespace AutoRip2MKV
{
    /// <summary>
    /// Simple dependency injection container for managing service dependencies
    /// </summary>
    public class ServiceContainer
    {
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        private readonly Dictionary<Type, Func<object>> _factories = new Dictionary<Type, Func<object>>();
        private static ServiceContainer _instance;

        private ServiceContainer() { }

        public static ServiceContainer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ServiceContainer();
                    _instance.RegisterDefaults();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Register a singleton instance
        /// </summary>
        public void RegisterSingleton<T>(T instance) where T : class
        {
            _services[typeof(T)] = instance;
        }

        /// <summary>
        /// Register a factory function for creating instances
        /// </summary>
        public void RegisterFactory<T>(Func<T> factory) where T : class
        {
            _factories[typeof(T)] = () => factory();
        }

        /// <summary>
        /// Resolve a service instance
        /// </summary>
        public T Resolve<T>() where T : class
        {
            var type = typeof(T);
            
            // Check for singleton instance first
            if (_services.ContainsKey(type))
            {
                return (T)_services[type];
            }
            
            // Check for factory
            if (_factories.ContainsKey(type))
            {
                return (T)_factories[type]();
            }
            
            throw new InvalidOperationException($"Service of type {type.Name} is not registered");
        }

        /// <summary>
        /// Check if a service is registered
        /// </summary>
        public bool IsRegistered<T>()
        {
            var type = typeof(T);
            return _services.ContainsKey(type) || _factories.ContainsKey(type);
        }

        /// <summary>
        /// Register default implementations
        /// </summary>
        private void RegisterDefaults()
        {
            RegisterSingleton<ILogger>(new NLogLogger());
            RegisterFactory<IProcessManager>(() => new ProcessManager(Resolve<ILogger>()));
            RegisterFactory<IFileOperations>(() => new FileOperations(Resolve<ILogger>()));
            RegisterFactory<ICredentialManager>(() => new WindowsCredentialManager(Resolve<ILogger>()));
            RegisterFactory<IConfigurationValidator>(() => new ConfigurationValidator(Resolve<ILogger>(), Resolve<IFileOperations>()));
            RegisterFactory<IConfigurationManager>(() => new ConfigurationManager(Resolve<ILogger>(), Resolve<IConfigurationValidator>(), Resolve<IFileOperations>()));
        }

        /// <summary>
        /// Clear all registrations (useful for testing)
        /// </summary>
        public void Clear()
        {
            _services.Clear();
            _factories.Clear();
        }

        /// <summary>
        /// Reset to default configuration
        /// </summary>
        public static void Reset()
        {
            _instance = null;
        }
    }
}
