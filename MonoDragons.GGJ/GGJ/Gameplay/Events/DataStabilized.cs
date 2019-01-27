using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoDragons.GGJ.Gameplay.Events
{
    class DataStabilized
    {
        public GameData GameData { get; set; }

        public DataStabilized(GameData gameData)
        {
            GameData = gameData;
        }
    }
}
