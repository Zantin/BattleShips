namespace BattleShipsLibrary
{
    public enum ClientToServer
    {
        Attack,
        GiveUp,
        ThisIsMe,
    }

    public enum ServerToClient
    {
        Win,
        Loss,
        EnemyUsername,
        Hit,
        Miss,
        YourTurn,
        BattleReady,
    }
}
