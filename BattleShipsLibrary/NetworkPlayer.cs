namespace BattleShipsLibrary
{
    public class NetworkPlayer
    {
        public ClientConnection clientConnection;
        public Player player { get { return clientConnection.player; } }

        public NetworkPlayer(ClientConnection clientConnection)
        {
            this.clientConnection = clientConnection;
        }

        public void Send(params object[] objects)
        {
            clientConnection.Send(objects);
        }
    }
}