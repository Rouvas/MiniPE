using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiNET;

namespace Plugins
{
    public class JumpManager
    {
        public List<JumpMatch> Matches;
        public static int MAX_MATCHES = 10;

        public LobbyManager manager;

        public JumpManager(LobbyManager m)
        {
            manager = m;
            // create match list
            Matches = new List<JumpMatch>();
        }

        public void playerWantsToJoin(Player player)
        {
            if (!playsJump(player))
            {
                if (isAMatchFree() || getMatchCount() < MAX_MATCHES)
                {
                    // Spieler can join a match
                    if (isAMatchFree())
                    {
                        // there is a match the player can join
                        getFirstFreeMatch().addPlayer(player);
                    }
                    else
                    {
                        // create a new match for the player
                        JumpMatch m = new JumpMatch(this);
                        m.addPlayer(player);
                        Matches.Add(m);
                    }
                }
                else
                {
                    // there is no match to play at the moment
                    player.SendMessage("§b[MiniPE] There is no match you can join at the moment.", MessageType.Chat);
                }
            }
        }

        public int getMatchCount()
        {
            return Matches.Count;
        }

        public Boolean isAMatchFree()
        {
            foreach (JumpMatch Match in Matches)
            {
                if (Match.getPlayersCount() < Match.MAX_PLAYERS)
                {
                    return true;
                }
            }
            return false;
        }

        public JumpMatch getFirstFreeMatch()
        {
            foreach(JumpMatch Match in Matches) {
                if(Match.getPlayersCount() < Match.MAX_PLAYERS)
                {
                    return Match;
                }
            }
            return null;
        }

        /*
        Checks if a player plays jump n run
        */
        public Boolean playsJump(Player player)
        {
            foreach(JumpMatch match in Matches)
            {
                if(match.isInMatch(player))
                {
                    return true;
                }
            }
            return false;
        }

        /*
        returns the match from a player
        */
        public JumpMatch getMatchByPlayer(Player player)
        {
            foreach(JumpMatch match in Matches)
            {
                if(match.isInMatch(player))
                {
                    return match;
                }
            }
            return null;
        }
    }
}
