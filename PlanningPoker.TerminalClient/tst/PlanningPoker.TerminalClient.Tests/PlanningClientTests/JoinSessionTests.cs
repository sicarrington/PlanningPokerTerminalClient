using Moq;
using PlanningPoker.Client.Connections;
using PlanningPoker.TerminalClient.Input;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PlanningPoker.TerminalClient.Tests.PlanningClientTests
{
    public class JoinSessionTests
    {
        private Mock<IPlanningConnectionFactory> _planningConnectionFactory;
        private Mock<PokerTerminal> _pokerTerminal;
        private Mock<IPlanningPokerConnection> _pokerConnection;

        private PlanningClient _planningClient;

        public JoinSessionTests()
        {
            _pokerConnection = new Mock<IPlanningPokerConnection>();
            _pokerConnection.Setup(x => x.Start(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _pokerConnection.Setup(x => x.JoinSession(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);
            _planningConnectionFactory = new Mock<IPlanningConnectionFactory>();
            _planningConnectionFactory.Setup(x => x.NewConnection()).Returns(_pokerConnection.Object);
            _pokerTerminal = new Mock<PokerTerminal>();
            _pokerTerminal.Setup(x => x.GetStartOption()).Returns(Task.FromResult(StartMenuOption.JoinSession));

            _planningClient = new PlanningClient(_planningConnectionFactory.Object, _pokerTerminal.Object); 
        }
        [Fact]
        public async void GivenUserElectsToJoinSession_WhenConnectionIsNotEstablished_ThenNewConnectionIsCreated()
        {
            await _planningClient.Start(new CancellationTokenSource());

            _planningConnectionFactory.Verify(x => x.NewConnection(), Times.Once);
        }
        [Fact]
        public async void GivenUserElectsToJoinSession_WhenJoiningSession_ThenConnectionIsEstablished()
        {
            await _planningClient.Start(new CancellationTokenSource());

            _pokerConnection.Verify(x => x.Start(It.IsAny<CancellationToken>()), Times.Once);
        }
        [Fact]
        public async void GivenUserElectsToJoinSession_WhenUserIsPromptedForDetails_ThenDetailsAreCorrectlyMappedToJoinSessionCall()
        {
            var userName = "Fred";
            var sessionId = "12345";

            _pokerTerminal.Setup(x => x.GetUserJoinInformation()).Returns(Task.FromResult((sessionId, userName)));

            await _planningClient.Start(new CancellationTokenSource());

            _pokerConnection.Verify(x => x.JoinSession(It.Is<string>(y => y == sessionId), It.Is<string>(y => y == userName)), Times.Once);
        }
    }
}
