using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiNET;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Blocks;
using MiNET.Worlds;
using log4net;
using System.Net;
using MiNET.Entities;
using System.Threading;

namespace Plugins
{
    public class LobbyManager : Plugin
    {
        public Vector3 spawnPos = new Vector3(-91, 6, 493);
        private static readonly ILog Log = LogManager.GetLogger(typeof(LobbyManager));

        public Level lobbyLevel;

        public JumpManager jumpManager;
        
        private Timer _popupTimer;

        protected override void OnEnable()
        {
            // here is the part where I think I have to set the PlayerFactory
            Context.Server.PlayerFactory = new MiniPEPlayerFactory(this);
            // load important levels
            lobbyLevel = Context.LevelManager.GetLevel(null, "Default");
            // stop block placing and breaking and stop the world time
            foreach (var level in Context.LevelManager.Levels)
            {
                level.BlockBreak += LevelOnBlockBreak;
                level.BlockPlace += LevelOnBlockPlace;
                level.IsWorldTimeStarted = false;
            }
            // start timer for popups in lobby
            _popupTimer = new Timer(DoLobbyPopups, null, 1000, 1000);
            // place a villager in the lobby
            // Mob (a villager) to teleport to Jump ('n' run)
            Mob entity = new Mob(15, lobbyLevel)
            {
                KnownPosition = new PlayerLocation(new Vector3(-91.3, 5, 501.3)),
                NameTag = "Jump",
            };
            entity.HealthManager = new VillagerHealthManager(entity, this);
            entity.SpawnEntity();

            // create JumpManager
            jumpManager = new JumpManager(this);
        }


        // Sets players spawn position to the lobby spawn position
        // so that the player will spawn there while joining
        [PacketHandler, Receive]
        public Package HandleJoin(McpeLogin packet, Player player)
        {
            player.SpawnPosition = new PlayerLocation(spawnPos);
            player.HealthManager = new PlayerHealthManager(player, this);
            // show a little welcome message
            player.AddPopup(new Popup()
            {
                MessageType = MessageType.Tip,
                Message = "§aWelcome on MiniPE!",
                Duration = 30
            });
            return packet;
        }

        /*
        Sends popups to players in lobby (the popup should be shown on every time when the player is in lobby)
        */
        private void DoLobbyPopups(object state)
        {
            var players = lobbyLevel.GetSpawnedPlayers();
            foreach (var player in players)
            {
                player.AddPopup(new Popup()
                {
                    MessageType = MessageType.Popup,
                    Message = "§bJust testing a bit",
                    Duration = 100
                });
            }    
        }

        private void LevelOnBlockBreak(object sender, BlockBreakEventArgs e)
        {
            e.Cancel = true;
        }

        private void LevelOnBlockPlace(object sender, BlockPlaceEventArgs e)
        {
            e.Cancel = true;
        }

        /*
        Check if a player who plays the Jump and run fell into water
        and then teleport back to the beginning
        */
        [PacketHandler, Receive]
        public Package inWater(McpeMovePlayer message, MiniPEPlayer player) // <- this is not called I think (maybe because of the Server creates no MiniPEPlayer Objects)
        {
            if (jumpManager.playsJump(player))
            {
                if (player.Level.GetBlock(new BlockCoordinates(player.KnownPosition)).Id == 8)
                {
                    player.KnownPosition = new PlayerLocation(new Vector3(340, 10, 454));
                    player.SendMovePlayer(true);
                }
                if (player.Level.GetBlock(new BlockCoordinates(player.KnownPosition)).Id == 9)
                {
                    player.KnownPosition = new PlayerLocation(new Vector3(340, 10, 454));
                    player.SendMovePlayer(true);
                }
            }
            return message;
        }

        /*
        Check if a player in jump n run reached the finish
        */
        [PacketHandler, Receive]
        public Package overRedstoneBlock(McpeMovePlayer message, MiniPEPlayer player) // <- isn't working too
        {
            if(jumpManager.playsJump(player))
            {
                if(player.Level.GetBlock(new BlockCoordinates((int)player.KnownPosition.X, (int)player.KnownPosition.Y-1, (int)player.KnownPosition.Z)).Id == 152)
                {
                    jumpManager.getMatchByPlayer(player).removePlayer(player);
                }
            }
            return message;
        }
        
        /*
        Command for getting back to lobby
        */
        [Command]
        public void lobby(Player player)
        {
            player.SendMessage("Teleporting to Lobby...", MessageType.Chat);
            player.SpawnLevel(lobbyLevel, new PlayerLocation
            {
                X = (int)spawnPos.X,
                Y = (int)spawnPos.Y,
                Z = (int)spawnPos.Z,
                Yaw = 91,
                Pitch = 28,
                HeadYaw = 91,
            });
        }
        
    }
}
