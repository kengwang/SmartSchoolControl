using Microsoft.Extensions.DependencyInjection;

namespace SchoolComputerControl.Client.Interfaces;

public interface IService
{
    public void ConfigureService(IServiceCollection services);
}