using Microsoft.Extensions.DependencyInjection;
using System;

namespace Domain.SeedWork
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class DependencyInjectionAttribute : Attribute
    {
        public ServiceLifetime ServiceLifetime { get; }

        public DependencyInjectionAttribute(ServiceLifetime serviceLifetime)
        {
            ServiceLifetime = serviceLifetime;
        }
    }

}
