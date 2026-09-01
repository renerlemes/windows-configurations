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
                    || !devices.Playback.Exists(entry => entry.Id == devices.PlaybackDefault)))
            {
                devices.PlaybackDefault = playbackDefault;
            }

            if (!string.IsNullOrEmpty(recordingDefault)
                && (string.IsNullOrEmpty(devices.RecordingDefault)
                    || !devices.Recording.Exists(entry => entry.Id == devices.RecordingDefault)))
            {
                devices.RecordingDefault = recordingDefault;
            }
        }

        private static List<AudioDeviceEntry> Merge(List<AudioDeviceEntry> saved, IReadOnlyList<AudioEndpoint> live)
        {
            Dictionary<string, bool> enabledById = [];

            if (saved != null)
            {
                foreach (AudioDeviceEntry entry in saved)
                {
                    if (string.IsNullOrEmpty(entry?.Id) || enabledById.ContainsKey(entry.Id))
                        continue;

                    enabledById[entry.Id] = entry.Enabled;
                }
            }

            List<AudioDeviceEntry> merged = [];

            foreach (AudioEndpoint endpoint in live)
            {
                merged.Add(new AudioDeviceEntry
                {
                    Id = endpoint.Id,
                    Name = endpoint.Name,
                    Enabled = enabledById.TryGetValue(endpoint.Id, out bool enabled) && enabled,
                    IconPath = endpoint.IconPath
                });
            }

            return merged;
        }
    }
}
