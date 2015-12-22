using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiNET;
using System.Net;

namespace Plugins
{
    public class MiniPEPlayer : Player
    {

        // not sure what to do with the constructor

        public override void Disconnect(string reason, bool sendDisconnect = true)
        {
            // will add something here later

            base.Disconnect(reason, sendDisconnect);
        }
    }
}
