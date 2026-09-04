using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Windows.Configurations.Features.Audio
{
    public sealed class MuteOnLockMonitor : IDisposable
    {
        private readonly List<string> _mutedDevices = [];
        private readonly AudioDeviceWatcher _watcher = new();
        private readonly object _gate = new();
        private bool _enabled;
        private bool _away;

        public MuteOnLockMonitor()
        {
            _watcher.Changed += OnAudioDevicesChanged;
        }

        public void SetEnabled(bool enabled)
        {
            if (enabled == _enabled)
                return;

            _enabled = enabled;

            if (enabled)
            {
                SystemEvents.SessionSwitch += OnSessionSwitch;
                return;
            }

            SystemEvents.SessionSwitch -= OnSessionSwitch;
            Restore();
        }

        private void OnSessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            if (!_enabled)
                return;

            if (IsLeaving(e.Reason))
            {
                Silence();
                return;
            }

            if (IsReturning(e.Reason))
                Restore();
        }

        /// <summary>
        /// Trocar de usuário e desconectar uma sessão remota deixam a máquina sozinha do mesmo
        /// jeito que o bloqueio, então também precisam silenciar.
        /// </summary>
        private static bool IsLeaving(SessionSwitchReason reason)
        {
            return reason is SessionSwitchReason.SessionLock
                or SessionSwitchReason.ConsoleDisconnect
                or SessionSwitchReason.RemoteDisconnect;
        }

        private static bool IsReturning(SessionSwitchReason reason)
        {
            return reason is SessionSwitchReason.SessionUnlock
                or SessionSwitchReason.ConsoleConnect
                or SessionSwitchReason.RemoteConnect;
        }

        private void Silence()
        {
            lock (_gate)
                _away = true;

            // Enquanto a sessão está fechada, um dispositivo que for conectado precisa nascer
            // mudo também: sem o observador, o som voltaria por ele.
            _watcher.Start();

            MuteAll();
        }

        private void Restore()
        {
            lock (_gate)
                _away = false;

            _watcher.Stop();

            lock (_gate)
            {
                // Restaura apenas o que este app silenciou: um dispositivo que já estava mudo
                // antes do bloqueio continua como o usuário deixou.
                foreach (string deviceId in _mutedDevices)
                    SystemVolume.SetMute(deviceId, false);

                _mutedDevices.Clear();
            }
        }

        private void OnAudioDevicesChanged()
        {
            ThreadPool.QueueUserWorkItem(_ => MuteAll());
        }

        private void MuteAll()
        {
            lock (_gate)
            {
                if (!_away)
                    return;

                // Silenciar só o dispositivo padrão não basta: um app tocando em outra saída, ou
                // uma troca de padrão feita pelo Windows durante o bloqueio, faria o som voltar.
                foreach (AudioEndpoint endpoint in AudioEndpointEnumerator.ListPlayback())
                {
                    if (_mutedDevices.Contains(endpoint.Id) || SystemVolume.GetMute(endpoint.Id))
                        continue;

                    SystemVolume.SetMute(endpoint.Id, true);
                    _mutedDevices.Add(endpoint.Id);
                }
            }
        }

        public void Dispose()
        {
            SetEnabled(false);

            _watcher.Changed -= OnAudioDevicesChanged;
            _watcher.Dispose();
        }
    }
}
