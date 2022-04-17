using Photon.Realtime;

namespace network
{
    public class GameEvent
    {
        public readonly GameEventType Type;
        public readonly Player Player;

        public GameEvent(GameEventType type, Player player)
        {
            Type = type;
            Player = player;
        }
    }

    public enum GameEventType
    {
        NONE,
        PLAYER_JOINED,
        PLAYER_LEFT,
        MIKUNI_PLACED_BY_PLAYER
    }
}