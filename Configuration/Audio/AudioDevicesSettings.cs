using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SoundSwitch.Configuration.Audio
{
    public class AudioDeviceEntry
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool Enabled { get; set; }

        /// <summary>
        /// Resolvido a cada enumeração, junto do dispositivo: não é persistido.
        /// </summary>
        [JsonIgnore]
        public string IconPath { get; set; }

        /// <summary>
        /// Indica se o dispositivo está presente e ativo agora. Um fone desconectado continua
        /// salvo, para não perder a marcação, mas deixa de ser exibido.
        /// </summary>
        [JsonIgnore]
        public bool Connected { get; set; }
    }

    public class AudioDevicesSettings
    {
        /// <summary>
        /// Última escolha do usuário. Mantido mesmo se o dispositivo desconectar, para
        /// restaurá-lo ao reconectar; o fallback automático não altera este valor.
        /// </summary>
        public string PlaybackDefault { get; set; }

        public string PlaybackShortcut { get; set; }

        public List<AudioDeviceEntry> Playback { get; set; } = [];

        /// <summary>
        /// Última escolha do usuário. Mantido mesmo se o dispositivo desconectar, para
        /// restaurá-lo ao reconectar; o fallback automático não altera este valor.
        /// </summary>
        public string RecordingDefault { get; set; }

        public string RecordingShortcut { get; set; }

        public List<AudioDeviceEntry> Recording { get; set; } = [];

        public List<AudioDeviceEntry> Devices(bool playback) => playback ? Playback : Recording;

        public string PreferredId(bool playback) => playback ? PlaybackDefault : RecordingDefault;

        public void SetPreferredId(bool playback, string id)
        {
            if (playback)
                PlaybackDefault = id;
            else
                RecordingDefault = id;
        }

        public string Shortcut(bool playback) => playback ? PlaybackShortcut : RecordingShortcut;

        public void SetShortcut(bool playback, string shortcut)
        {
            if (playback)
                PlaybackShortcut = shortcut;
            else
                RecordingShortcut = shortcut;
        }

        public List<AudioDeviceEntry> EnabledConnected(bool playback)
        {
            return Devices(playback).FindAll(device => device.Enabled && device.Connected);
        }

        public AudioDeviceEntry FindById(bool playback, string id)
        {
            return Devices(playback).Find(entry =>
                string.Equals(entry.Id, id, StringComparison.OrdinalIgnoreCase));
        }
    }
}
