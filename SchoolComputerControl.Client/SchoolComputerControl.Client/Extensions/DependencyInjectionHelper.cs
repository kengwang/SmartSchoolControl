using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using SchoolComputerControl.Client.Interfaces;

namespace SchoolComputerControl.Client.Extensions;

public static class DependencyInjectionHelper
{
    public static void AddViewModelFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        foreach (var type in assembly.GetTypes().Where(t => typeof(IViewModel).IsAssignableFrom(t) && !t.IsInterface)
                     .ToList())
        {
            services.AddTransient(type);
            services.AddTransient(typeof(IViewModel), type);
        }
    }

    public static void AddServicesFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        foreach (var service in assembly.GetTypes().Where(t => typeof(IService).IsAssignableFrom(t) && !t.IsInterface)
                     .ToList().Select(type => Activator.CreateInstance(type) as IService)
                     .Where(service => service is not null))
        {
            service!.ConfigureService(services);
            services.AddSingleton(service!);
        }
    }

    public static void AddPagesFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        foreach (var type in assembly.GetTypes().Where(t => typeof(Page).IsAssignableFrom(t) && !t.IsInterface)
                     .ToList())
        {
            services.AddTransient(type);
            services.AddTransient(typeof(Page), type);
        }
    }

    public static void AddWindowsFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        foreach (var type in assembly.GetTypes().Where(t => typeof(Window).IsAssignableFrom(t) && !t.IsInterface)
                     .ToList())
        {
            services.AddTransient(type);
            services.AddTransient(typeof(Window), type);
        }
    }


    public static void AddNotificationReceiverFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var scanner = from type in assembly.GetTypes()
            where !type.IsAbstract && !type.IsGenericTypeDefinition
            let interfaces = type.GetInterfaces()
            let genericInterfaces = interfaces.Where(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INotificationReceiver<>))
            let matchingInterface = genericInterfaces.FirstOrDefault()
            where matchingInterface != null
            select (matchingInterface, type);
        foreach (var (matchingInterface, type) in scanner)
        {
            services.AddTransient(matchingInterface, type);
        }
    }
}