using Mekatrol.Automatum.Middleware.Exceptions;
using Mekatrol.Automatum.Models;
using Mekatrol.Automatum.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Mekatrol.Automatum.UnitTests;

[TestClass]
public class FlowServiceTest : ServicesUnitTest
{
    [TestMethod]
    public async Task TestNewFlowBadId()
    {
        var flow = new Flow
        {
            Label = "label1",
            Blocks = [],
            Connections = []
        };

        var flowService = _serviceProvider.GetRequiredService<IFlowService>();

        var cancellationTokenSource = new CancellationTokenSource();
        var ex = await Assert.ThrowsExceptionAsync<BadRequestException>(async () => await flowService.CreateFlow(flow, cancellationTokenSource.Token));

        Assert.AreEqual(ex.Errors[0].ErrorMessage, $"A new flow must have the ID set to '{Guid.Empty}'");
    }

    [TestMethod]
    public async Task TestNewFlowGoodId()
    {
        var flow = new Flow
        {
            Id = Guid.Empty,
            Label = "label1",
            Blocks = [],
            Connections = []
        };

        var flowService = _serviceProvider.GetRequiredService<IFlowService>();

        var cancellationTokenSource = new CancellationTokenSource();
        await flowService.CreateFlow(flow, cancellationTokenSource.Token);

        var flowFileName = Path.GetFullPath(Path.Combine("./", "flows", $"{flow.Id}.json"));

        Assert.IsTrue(File.Exists(flowFileName));

        File.Delete(flowFileName);
    }
}