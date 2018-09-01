using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowsFormsApp
{
    public class MCIWavSoundDriver
    {
        private static class MCI
        {
            [DllImport("winmm.dll")]
            private static extern Int32 mciSendString(string command, StringBuilder buffer, int bufferSize, IntPtr hwndCallback);

            [DllImport("winmm.dll")]
            private static extern Int32 mciGetErrorString(Int32 errorCode, StringBuilder errorText, Int32 errorTextSize);

            public static void SendString(string command, StringBuilder buffer, int bufferSize, IntPtr hwndCallback)
            {
                var i = mciSendString(command, buffer, bufferSize, hwndCallback);
                if (i == 0)
                {
                    return;
                }
                var errorText = new StringBuilder(128);
                mciGetErrorString(i, errorText, 128);
                throw new Exception(errorText.ToString());
            }
        }

        private readonly string directoryPath;
        public MCIWavSoundDriver(string directoryPath) => this.directoryPath = directoryPath;

        private void Initialize()
        {
            var directoryInfo = new DirectoryInfo(directoryPath);

            const string ext = ".wav";
            const string searchPattern = "*" + ext;

            foreach (var fileInfo in directoryInfo.GetFiles(searchPattern))
            {
                var filePath = fileInfo.FullName;
                var filename = fileInfo.Name;
                var alias = filename.Substring(0, filename.Length - ext.Length);
                MCI.SendString($"open {filePath} type waveaudio alias {alias}", null, 0, IntPtr.Zero);
            }
            wasInitialized = true;
        }

        public void Play(int sound, bool shouldLoop)
        {
            EnsureInitialized();
            var targetAlias = TargetAlias(sound, shouldLoop);

            MCI.SendString($"seek {targetAlias} to start", null, 0, IntPtr.Zero);

            if (shouldLoop)
            {
                MCI.SendString($"play {targetAlias}", null, 0, IntPtr.Zero);
            }
            else
            {
                MCI.SendString($"play {targetAlias}", null, 0, IntPtr.Zero);
            }
        }

        private static string TargetAlias(int sound, bool loop)
        {
            return loop ? $"{sound}loop" : $"{sound}";
        }

        public void Stop(int sound, bool wasLooping)
        {
            EnsureInitialized();
            var targetAlias = TargetAlias(sound, wasLooping);
            MCI.SendString($"stop {targetAlias}", null, 0, IntPtr.Zero);
        }

        private readonly object lockable = new object();
        private bool wasInitialized;

        public void EnsureInitialized()
        {
            lock (lockable)
            {
                if (wasInitialized) { return; }
                Initialize();
            }
        }
    }
}