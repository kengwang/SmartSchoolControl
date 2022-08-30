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
    public string GetHashedString(string original, bool lowerCase = true);
}

public class EasyHasher<THasher> : IEasyHasher<THasher> where THasher : IHash, new()
{
    private THasher? _hasherCache;
    private THasher Hasher => _hasherCache ??= new THasher();

    public string GetHashedString(string original, bool lowerCase = true)
    {
        var result = new Span<byte>(new byte[Hasher.Length]);
        Hasher.UpdateFinal(Encoding.UTF8.GetBytes(original), result);
        if (lowerCase)
            return BitConverter.ToString(result.ToArray()).Replace("-",string.Empty).ToLower();
        return BitConverter.ToString(result.ToArray()).Replace("-",string.Empty);
    }
}