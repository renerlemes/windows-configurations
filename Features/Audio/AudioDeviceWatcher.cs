using System;
using System.Runtime.InteropServices;

namespace Windows.Configurations.Features.Audio
{
    /// <summary>
    /// Avisa quando o Windows conecta, desconecta ou troca o padrão de um dispositivo de áudio.
    /// O evento chega em uma thread do COM e não pode chamar o enumerador de volta ali mesmo:
    /// quem escuta precisa jogar o trabalho para outra thread.
    /// </summary>
    internal sealed class AudioDeviceWatcher : IMMNotificationClient, IDisposable
    {
        private IMMDeviceEnumerator _enumerator;

        public event Action Changed;

        public void Start()
        {
            if (_enumerator != null)
                return;

            try
            {
                IMMDeviceEnumerator enumerator = (IMMDeviceEnumerator)new MMDeviceEnumeratorComObject();

                enumerator.RegisterEndpointNotificationCallback(this);

                _enumerator = enumerator;
            }
            catch (COMException)
            {
                // Sem as notificações o app continua funcionando, só deixa de reagir na hora.
            }
        }

        public void Stop()
        {
            if (_enumerator is null)
                return;

            try
            {
                _enumerator.UnregisterEndpointNotificationCallback(this);
            }
            catch (COMException)
            {
            }

            Marshal.ReleaseComObject(_enumerator);

            _enumerator = null;
        }

        public void Dispose() => Stop();

        void IMMNotificationClient.OnDeviceStateChanged(string pwstrDeviceId, int dwNewState) => Changed?.Invoke();

        void IMMNotificationClient.OnDeviceAdded(string pwstrDeviceId) => Changed?.Invoke();

        void IMMNotificationClient.OnDeviceRemoved(string pwstrDeviceId) => Changed?.Invoke();

        void IMMNotificationClient.OnDefaultDeviceChanged(int flow, int role, string pwstrDefaultDeviceId) => Changed?.Invoke();

        void IMMNotificationClient.OnPropertyValueChanged(string pwstrDeviceId, PROPERTYKEY key)
        {
            // Ignorado de propósito: dispara em rajada por mudança de volume e não altera a listagem.
        }
    }
}
