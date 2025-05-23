﻿namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAllRegisterTypes(this IServiceCollection services, params Assembly[] assemblies)
        {
            if (assemblies == null) return;

            List<TypeInfo> interfaceTypes = new List<TypeInfo>();
            List<TypeInfo> implTypes = new List<TypeInfo>();
            List<TypeInfo> serviceTypes = new List<TypeInfo>();
            foreach (var assembly in assemblies)
            {
                interfaceTypes.AddRange(assembly.DefinedTypes
                        .Where(u => u.IsInterface && u.GetCustomAttribute(typeof(DependencyInjectionAttribute)) != null));

                implTypes.AddRange(assembly.DefinedTypes
                        .Where(u => u.IsClass && u.ImplementedInterfaces.Any()));

                serviceTypes.AddRange(assembly.DefinedTypes
                        .Where(u => u.IsClass && u.GetCustomAttribute(typeof(DependencyInjectionAttribute)) != null));
            }

            foreach (var interfaceType in interfaceTypes)
            {
                var attribute = interfaceType.GetCustomAttribute(typeof(DependencyInjectionAttribute)) as DependencyInjectionAttribute;
                if (attribute == null) continue;

                var multipImplTypes = implTypes.Where(u => u.IsClass && u.GetInterfaces().Contains(interfaceType));

                foreach (var implType in multipImplTypes)
                {
                    services.Add(new ServiceDescriptor(interfaceType, implType, attribute.ServiceLifetime));
                }
            }

            foreach (var serviceType in serviceTypes)
            {
                var attribute = serviceType.GetCustomAttribute(typeof(DependencyInjectionAttribute)) as DependencyInjectionAttribute;
                if (attribute == null) continue;

                services.Add(new ServiceDescriptor(serviceType, serviceType, attribute.ServiceLifetime));
            }
        }

    }
}
