using System.Net;

using MiNET;
using log4net;

namespace Plugins
{
    public class MiniPEPlayerFactory : PlayerFactory
    {

        private LobbyManager lm;
        private static readonly ILog Log = LogManager.GetLogger(typeof(MiniPEPlayerFactory));

        public MiniPEPlayerFactory(LobbyManager lm)
        {
            this.lm = lm;
        }

        public override Player CreatePlayer(MiNetServer server, IPEndPoint endPoint, int mtuSize)
        {
            return new MiniPEPlayer(server, endPoint, mtuSize, lm);
        }
    }
}