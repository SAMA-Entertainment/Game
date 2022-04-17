using System.Collections.Generic;
using Photon.Realtime;

namespace network
{
    public class Team
    {
        public readonly TeamSide Side;
        public readonly uint Capacity;
        
        public uint TotalMikuniCaptured { private set; get; }
        public List<Player> Players;
        public List<GameEvent> TeamHistory;

        public Team(TeamSide side, uint capacity)
        {
            Side = side;
            Capacity = capacity;
        }

        public void AddPlayer(Player player)
        {
            Players.Add(player);
            TeamHistory.Add(new GameEvent(GameEventType.PLAYER_JOINED, player));
        }

        public void RemovePlayer(Player player)
        {
            Players.Remove(player);
            TeamHistory.Add(new GameEvent(GameEventType.PLAYER_LEFT, player));
        }

        public bool RegisterScore(Player player, uint mikuniCaptured)
        {
            if (!Players.Contains(player)) return false;
            TotalMikuniCaptured += mikuniCaptured;
            TeamHistory.Add(new GameEvent(GameEventType.MIKUNI_PLACED_BY_PLAYER, player));
            return true;
        }
    }
}