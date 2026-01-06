using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class QueryBuilderTests
{
    [Fact]
    public void Where_ShouldFilterData()
    {
        var builder = new QueryBuilder<int>();
        var data = new[] { 1, 2, 3, 4, 5 };
        
        var result = builder.Where(x => x > 2).Execute(data);
        
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public void OrderBy_ShouldSortData()
    {
        var builder = new QueryBuilder<int>();
        var data = new[] { 3, 1, 2 };
        
        var result = builder.OrderBy(x => x).Execute(data);
        
        Assert.Equal(new[] { 1, 2, 3 }, result);
    }

    [Fact]
    public void Take_ShouldLimitResults()
    {
        var builder = new QueryBuilder<int>();
        var data = new[] { 1, 2, 3, 4, 5 };
        
        var result = builder.Take(3).Execute(data);
        
        Assert.Equal(3, result.Count());
    }
}
