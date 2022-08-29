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

    public static void AddPagesFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        foreach (var type in assembly.GetTypes().Where(t => typeof(Page).IsAssignableFrom(t) && !t.IsInterface).ToList())
        {
            services.AddTransient(type);
            services.AddTransient(typeof(Page), type);
        }
    }
    
    public static void AddWindowsFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        foreach (var type in assembly.GetTypes().Where(t => typeof(Window).IsAssignableFrom(t) && !t.IsInterface).ToList())
        {
            services.AddTransient(type);
            services.AddTransient(typeof(Window), type);
        }
    }
}