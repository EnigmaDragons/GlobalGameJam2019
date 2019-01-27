using MonoDragons.Core.EventSystem;
using MonoDragons.Core.IO;
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
            io = new AppDataJsonIo("GGJ2019");
        }

        private void Save(DataStabilized e)
        {
            try
            {
                io.Save("Save", e);
            }
            catch (Exception x)
            {
                Logger.WriteLine("Failed to save!");
                Logger.Write(x.ToString());
            }
        }
    }
}
