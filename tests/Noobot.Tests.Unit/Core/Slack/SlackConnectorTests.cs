using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Noobot.Core;
using Noobot.Core.Configuration;
using Noobot.Core.Extensions;
using Xunit;

namespace Noobot.Tests.Unit.Core.Slack
{
    public class SlackConnectorTests
    {
        [Fact]
        public async Task should_connect_as_expected()
        {
            // given
            var configReader = JsonConfigReader.DefaultLocation();
            var services = new ServiceCollection()
                .AddLogging()
                .AddNoobotCore(configReader);
            var serviceProvider = services.BuildServiceProvider();
            var connector = new NoobotCore(configReader, new Mock<ILogger<NoobotCore>>().Object, serviceProvider);

            // when
            await connector.Connect();

            // then
        }
    }
}