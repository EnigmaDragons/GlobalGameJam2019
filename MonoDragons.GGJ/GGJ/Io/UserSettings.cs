using System;
using MonoDragons.Core.IO;

namespace MonoDragons.GGJ
{
    public class UserSettings
    {
        private const string Name = "UserSettings";
        private readonly AppDataJsonIo _io;
        private readonly UserSettingsData _current;
        
        public UserSettings()
        {
            _io = new AppDataJsonIo(AppID.Value);
            _current = _io.LoadOrDefault(Name, () => new UserSettingsData());
        }

        public void Update(Action<UserSettingsData> change)
        {
            change(_current);
            _io.Save(Name, _current);
        }

        public string LastConnectionEndpoint => _current.LastConnectionEndpoint;
        public int SoundVolume => _current.SoundVolume;
        public int MusicVolume => _current.MusicVolume;
    }
}
