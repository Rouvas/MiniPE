using System.Net;

using MiNET;

namespace Plugins
{
    public class PlayerFactory
    {
        public virtual Player CreatePlayer(MiNetServer server, IPEndPoint endPoint, int mtuSize)
        {
            return new MiniPEPlayer(server, endPoint, mtuSize);
        }
    }
}