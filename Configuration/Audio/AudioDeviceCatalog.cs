using System.Collections.Generic;
using Windows.Configurations.Features.Audio;

namespace Windows.Configurations.Configuration.Audio
{
    public static class AudioDeviceCatalog
    {
        public static void Refresh(AudioDevicesSettings devices)
        {
            devices.Playback = Merge(devices.Playback, AudioEndpointEnumerator.ListPlayback());
            devices.Recording = Merge(devices.Recording, AudioEndpointEnumerator.ListRecording());

            string playbackDefault = AudioEndpointEnumerator.GetDefaultPlaybackId();
            string recordingDefault = AudioEndpointEnumerator.GetDefaultRecordingId();

            if (!string.IsNullOrEmpty(playbackDefault)
                && (string.IsNullOrEmpty(devices.PlaybackDefault)
                    || !devices.Playback.Exists(entry => entry.Connected && entry.Id == devices.PlaybackDefault)))
            {
                devices.PlaybackDefault = playbackDefault;
            }

            if (!string.IsNullOrEmpty(recordingDefault)
                && (string.IsNullOrEmpty(devices.RecordingDefault)
                    || !devices.Recording.Exists(entry => entry.Connected && entry.Id == devices.RecordingDefault)))
            {
                devices.RecordingDefault = recordingDefault;
            }
        }

        private static List<AudioDeviceEntry> Merge(List<AudioDeviceEntry> saved, IReadOnlyList<AudioEndpoint> live)
        {
            Dictionary<string, AudioDeviceEntry> savedById = [];

            if (saved != null)
            {
                foreach (AudioDeviceEntry entry in saved)
                {
                    if (string.IsNullOrEmpty(entry?.Id))
                        continue;

                    savedById[entry.Id] = entry;
                }
            }

            List<AudioDeviceEntry> merged = [];
            HashSet<string> liveIds = [];

            foreach (AudioEndpoint endpoint in live)
            {
                liveIds.Add(endpoint.Id);

                merged.Add(new AudioDeviceEntry
                {
                    Id = endpoint.Id,
                    Name = endpoint.Name,
                    Enabled = savedById.TryGetValue(endpoint.Id, out AudioDeviceEntry entry) && entry.Enabled,
                    IconPath = endpoint.IconPath,
                    Connected = true
                });
            }

            // Um dispositivo marcado que foi desconectado precisa continuar salvo: sem isso,
            // ao reconectar ele voltaria desmarcado.
            foreach (AudioDeviceEntry entry in savedById.Values)
            {
                if (!entry.Enabled || liveIds.Contains(entry.Id))
                    continue;

                merged.Add(new AudioDeviceEntry
                {
                    Id = entry.Id,
                    Name = entry.Name,
                    Enabled = true,
                    Connected = false
                });
            }

            return merged;
        }
    }
}
