using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class CryptoHelperTests
{
    [Fact]
    public void GenerateRandomString_ShouldGenerateString()
    {
        var str = CryptoHelper.GenerateRandomString(10);
        
        Assert.Equal(10, str.Length);
    }

    [Fact]
    public void Hash_ShouldGenerateHash()
    {
        var hash1 = CryptoHelper.Hash("test");
        var hash2 = CryptoHelper.Hash("test");
        
        Assert.Equal(hash1, hash2);
        Assert.NotEmpty(hash1);
    }

    [Fact]
    public void GenerateGuid_ShouldGenerateValidGuid()
    {
        var guid = CryptoHelper.GenerateGuid();
        
        Assert.NotEqual(Guid.Empty, guid);
    }

    [Fact]
    public void ToBase64_ShouldEncodeAndDecode()
    {
        var data = "test data";
        var encoded = CryptoHelper.ToBase64(data);
        var decoded = CryptoHelper.FromBase64(encoded);
        
        Assert.Equal(data, decoded);
    }
}
