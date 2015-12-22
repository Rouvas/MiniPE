using MiNET;
using MiNET.Entities;
using MiNET.Net;
using log4net;
using MiNET.Utils;
using System.Net;
using System;
using System.Linq;

namespace Plugins
{
    public class VillagerHealthManager : HealthManager
    {

        Entity  villager;
        LobbyManager lm;

        public VillagerHealthManager(Entity entity, LobbyManager lm) : base(entity)
        {
            villager = entity;
            this.lm = lm;
        }

        public override void TakeHit(Entity source, int damage = 1, DamageCause cause = DamageCause.Unknown)
        {
            
            var player = source as MiniPEPlayer;
            if (player != null)
            {
                if(villager.NameTag.Equals("Jump"))
                {
                    lm.jumpManager.playerWantsToJoin(player);
                }
            }
            
        }

        public override void OnTick()
        {
            Health = 200;

            base.OnTick();
        }
    }
}