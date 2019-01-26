using MonoDragons.Core.EventSystem;
using MonoDragons.Core.IO;
using MonoDragons.GGJ.Gameplay.Events;
using MonoDragons.GGJ.OtherEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoDragons.GGJ.UnnamedFolder
{
    public class StateSynchronizer
    {
        private GameData _gameData;
        private Player _local;
        private AppDataJsonIo _io;

        public StateSynchronizer(GameData data, Player local)
        {
            _gameData = data;
            _local = local;
            _io = new AppDataJsonIo("GGJ2019");
            Event.Subscribe<GameDataCanBeSynced>(Sync, this);
        }

        private void Sync(GameDataCanBeSynced data)
        {
            if(data.OriginPlayer != _local)
            {
                if (_local == Player.Cowboy)
                    _gameData.HouseState = data.GameData.HouseState;
                else
                    _gameData.CowboyState = data.GameData.CowboyState;
            }
            try
            {
                _io.Save("save", _gameData);
            }
            catch
            {
                Event.Publish(new FailedToSave());
            }
        }
    }
}
