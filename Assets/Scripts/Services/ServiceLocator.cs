using System.Collections.Generic;

namespace Services
{
    public class ServiceLocator
    {
        private static readonly Dictionary<System.Type, object> _services = new Dictionary<System.Type, object>();

        public static void Register<T>(T service)
        {
            _services[typeof(T)] = service;
        }

        public static T Get<T>()
        {
            return (T)_services[typeof(T)];
        }
    }
}