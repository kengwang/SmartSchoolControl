using System.Security.Cryptography;
using System.Text;
using CryptoBase.Abstractions.Digests;
using CryptoBase.Digests;
using SchoolComputerControl.Server.Interfaces;

namespace SchoolComputerControl.Server.Endpoints.ConfigurationEndpoints;

public class HashServiceEndpoint : IEndpoint
{
    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton(typeof(IEasyHasher<>), typeof(EasyHasher<>));
    }

    public void ConfigureApp(WebApplication app)
    {
        // Nothing
    }
}

public interface IEasyHasher<THasher> where THasher : IHash, new()
{
    public string GetHashedString(string original);
}

public class EasyHasher<THasher> : IEasyHasher<THasher> where THasher : IHash, new()
{
    private THasher? _hasherCache;
    private THasher Hasher => _hasherCache ??= new THasher();
    
    public string GetHashedString(string original)
    {
        var result = Span<byte>.Empty;
        Hasher.UpdateFinal(Encoding.UTF8.GetBytes(original), result);
        return Encoding.UTF8.GetString(result);
    }
}
