using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiNET;
using MiNET.Worlds;
using MiNET.Utils;

namespace Plugins
{
    public class JumpMatch
    {
        private Level level;

        public int MAX_PLAYERS = 10;

        public List<MiniPEPlayer> Players;

        public JumpManager manager;

        public JumpMatch(JumpManager m)
        {
            manager = m;
            // create player list
            Players = new List<MiniPEPlayer>();
            // load the level for the jump n run
            level = new Level("jump" + manager.getMatchCount(), new AnvilWorldProvider(@"the path"));
            level.Initialize();
        }

        public int getPlayersCount()
        {
            return Players.Count;
        }

        public void addPlayer(MiniPEPlayer player)
        {
            // add player to list
            Players.Add(player);
            // send a message to the player
            player.SendMessage("§b[MiniPE] Joining...", MessageType.Chat);
            // teleport the player to the jump n run
            player.SpawnLevel(level, new PlayerLocation
            {
                X = 340,
                Y = 10,
                Z = 454,
                Yaw = 91,
                Pitch = 28,
                HeadYaw = 91,
            });
        }

        public void removePlayer(MiniPEPlayer player)
        {
            // remove player from list
            Players.Remove(player);
            // teleport player back to lobby
            player.SpawnLevel(manager.manager.lobbyLevel, new PlayerLocation
            {
                X = (int)manager.manager.spawnPos.X,
                Y = (int)manager.manager.spawnPos.Y,
                Z = (int)manager.manager.spawnPos.Z,
                Yaw = 91,
                Pitch = 28,
                HeadYaw = 91,
            });
        }

        public Boolean isInMatch(MiniPEPlayer player)
        {
            foreach (Player p in Players)
            {
                if(player.Username.Equals(p.Username))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
