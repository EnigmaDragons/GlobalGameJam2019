using MonoDragons.Core.EventSystem;
using MonoDragons.Core.IO;
using MonoDragons.GGJ.Gameplay;
using MonoDragons.GGJ.Gameplay.Events;
using System;

namespace MonoDragons.GGJ
{
    public class DataSaver
    {
        private AppDataJsonIo io;

        public DataSaver()
        {
            Event.Subscribe<DataStabilized>(Save, this);
            Event.Subscribe<PlayerDefeated>(Delete, this);
            io = new AppDataJsonIo("Bed Dead Redemption");
        }

        private void Delete(object e)
        {
            try
            {
                io.Delete("Save");
            }
            catch (Exception x)
            {
                Logger.WriteLine("Failed to delete save!");
                Logger.Write(x.ToString());
            }
        }

        private void Save(DataStabilized e)
        {
            try
            {
                io.Save("Save", e.GameData);
            }
            catch (Exception x)
            {
                Logger.WriteLine("Failed to save!");
                Logger.Write(x.ToString());
            }
        }
    }
}
