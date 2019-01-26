using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoDragons.GGJ.Gameplay.Events
{
    public struct GameDataCanBeSynced
    {
        public GameData GameData;
        public Player OriginPlayer;

        public GameDataCanBeSynced(GameData gameData, Player originPlayer)
        {
            GameData = gameData;
            OriginPlayer = originPlayer;
        }
    }
}
