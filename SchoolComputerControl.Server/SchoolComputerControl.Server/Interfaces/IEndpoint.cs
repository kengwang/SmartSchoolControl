namespace SchoolComputerControl.Server.Interfaces;

public interface IEndpoint
{
    public void ConfigureBuilder(WebApplicationBuilder builder);
    public void ConfigureApp(WebApplication app);
}