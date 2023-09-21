using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace MuteApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ToggleMicrophoneMute();
        }

        static void ToggleMicrophoneMute()
        {
            var enumerator = new MMDeviceEnumerator();
            var captureDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);

            string muteSoundPath = "C:\\Users\\PC\\source\\repos\\Petya\\DiscordMuteSound.mp3";
            string unmuteSoundPath = "C:\\Users\\PC\\source\\repos\\Petya\\DiscordUnmuteSound.mp3";

            foreach (var device in captureDevices)
            {
                bool currentState = device.AudioEndpointVolume.Mute;
                device.AudioEndpointVolume.Mute = !currentState;

                if (!currentState)
                {
                    PlayMp3(muteSoundPath);
                }
                else
                {
                    PlayMp3(unmuteSoundPath);
                }

                Console.WriteLine($"Toggled mute state for device: {device.FriendlyName} to {!currentState}");

                break;
            }
        }

        static void PlayMp3(string filename)
        {
            using (var audioFile = new AudioFileReader(filename))
            using (var outputDevice = new WaveOutEvent())
            {
                outputDevice.Init(audioFile);
                outputDevice.Play();
                while (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    System.Threading.Thread.Sleep(500);
                }
            }
        }
    }
}
