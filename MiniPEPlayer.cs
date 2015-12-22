using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiNET;
using System.Net;
using MiNET.Entities;

namespace Plugins
{
    public class MiniPEPlayer : Player
    {
        private LobbyManager lm;

        public MiniPEPlayer(MiNetServer server, IPEndPoint endPoint, int mtuSize, LobbyManager lm) : base(server, endPoint, mtuSize)
        {
            this.lm = lm;
        }

        public override void Disconnect(string reason, bool sendDisconnect = true)
        {
            // check if the player is in a jump match
            if(lm.jumpManager.playsJump(this))
            {
                // remove player from match
                lm.jumpManager.getMatchByPlayer(this).removePlayer(this);
            }

            base.Disconnect(reason, sendDisconnect);
        }
        
    }
}
