namespace BattleShipsLibrary
{
    public class NetworkPlayer
    {
        public ClientConnection clientConnection;
        public Player player { get; private set; }

        public NetworkPlayer(ClientConnection clientConnection)
        {
            this.clientConnection = clientConnection;
            this.clientConnection.BindNetworkPlayer(this);
        }

        public void SetPlayer(Player player)
        {
            this.player = player;
        }
    }
}