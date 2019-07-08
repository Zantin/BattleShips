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
        EnemyAttack,
        Hit,
        Miss,
        YourTurn,
        BattleReady,
    }
}
