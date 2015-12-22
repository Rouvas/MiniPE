using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiNET;
using MiNET.Entities;

namespace Plugins
{
    public class PlayerHealthManager : HealthManager
    {
        Player player;
        LobbyManager lm;

        public PlayerHealthManager(Entity entity, LobbyManager lm) : base(entity)
        {
            player = entity as Player;
            this.lm = lm;
        }

        public override void OnTick()
        {
            Health = 20;
            base.OnTick();
        }

    }
}
