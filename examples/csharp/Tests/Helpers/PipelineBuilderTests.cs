using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class PipelineBuilderTests
{
    [Fact]
    public void Execute_ShouldApplyAllSteps()
    {
        var pipeline = new PipelineBuilder<int>();
        pipeline.AddStep(x => x + 1);
        pipeline.AddStep(x => x * 2);
        
        var result = pipeline.Execute(5);
        
        Assert.Equal(12, result);
    }

    [Fact]
    public void StepCount_ShouldReturnCorrectCount()
    {
        var pipeline = new PipelineBuilder<int>();
        pipeline.AddStep(x => x + 1);
        pipeline.AddStep(x => x * 2);
        
        Assert.Equal(2, pipeline.StepCount);
    }

    [Fact]
    public void Clear_ShouldRemoveAllSteps()
    {
        var pipeline = new PipelineBuilder<int>();
        pipeline.AddStep(x => x + 1);
        pipeline.Clear();
        
        Assert.Equal(0, pipeline.StepCount);
    }
}
