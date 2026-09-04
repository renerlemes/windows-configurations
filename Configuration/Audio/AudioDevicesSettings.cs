using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Windows.Configurations.Configuration.Audio
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
        public string PlaybackDefault { get; set; }

        public string PlaybackShortcut { get; set; }

        public List<AudioDeviceEntry> Playback { get; set; } = [];

        public string RecordingDefault { get; set; }

        public string RecordingShortcut { get; set; }

        public List<AudioDeviceEntry> Recording { get; set; } = [];
    }
}
