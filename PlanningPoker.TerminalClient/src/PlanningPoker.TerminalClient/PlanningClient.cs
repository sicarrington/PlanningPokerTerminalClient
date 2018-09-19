using PlanningPoker.Client.Connections;
using PlanningPoker.Client.Model;
using PlanningPoker.TerminalClient.Input;
using System.Threading;
using System.Threading.Tasks;

namespace PlanningPoker.TerminalClient
{
    public class PlanningClient
    {
        private IPlanningConnectionFactory _planningConnectionFactory;
        private PokerTerminal _pokerTerminal;

        private IPlanningPokerConnection _connection;
        private CancellationTokenSource _cancellationTokenSource;


        public PlanningClient(IPlanningConnectionFactory planningConnectionFactory, PokerTerminal pokerTerminal)
        {
            _planningConnectionFactory = planningConnectionFactory;
            _pokerTerminal = pokerTerminal;
        }
        private void EnsureConnection()
        {
            if(_connection == null)
            {
                _connection = _planningConnectionFactory.NewConnection();

                _connection.OnJoinSessionSucceeded(JoinSessionSucceeded);
                _connection.OnJoinSessionFailed(JoinSessionFailed);

                _connection.OnSessionInformationUpdated(SessionInformationUpdated);
                _connection.OnDisconnected(DisconnectedFromSession);

                _connection.OnSessionEnded(SessionEnded);
            }
        }
        public async Task Start(CancellationTokenSource cancellationTokenSource)
        {
            _cancellationTokenSource = cancellationTokenSource;
            var option = await _pokerTerminal.GetStartOption();
            if(option == StartMenuOption.JoinSession)
            {
                await JoinSession();
            }
        }

        private async Task JoinSession()
        {
            var sessiongJoinInfo = await _pokerTerminal.GetUserJoinInformation();

            EnsureConnection();

            await _connection.Start(_cancellationTokenSource.Token);
            await _connection.JoinSession(sessiongJoinInfo.sessionId, sessiongJoinInfo.userName);
        }

        private void DisconnectedFromSession()
        {
            _pokerTerminal.Output("\n******************\nSession was disconnected\n******************\n");
            if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
            }
        }
        private void JoinSessionSucceeded()
        {

        }
        private void JoinSessionFailed()
        {
            _pokerTerminal.Output("Joining the session failed");
        }
        private void SessionInformationUpdated(PokerSession sessionInformation)
        {
            _pokerTerminal.RenderStatusInformation(sessionInformation);
        }
        private async void SessionEnded()
        {
            await _pokerTerminal.HandleSessionEnded();
            await Start(_cancellationTokenSource);
        }
    }
}
