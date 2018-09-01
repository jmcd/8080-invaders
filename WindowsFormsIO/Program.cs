using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var romDirectoryPath = Path.GetDirectoryName(Application.ExecutablePath);
            var romLoader = new ROMLoader(romDirectoryPath);
            byte[] ramContent = null;
            try
            {
                ramContent = romLoader.Load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (ramContent != null)
            {
                var soundDirectoryPath = InstallSounds();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                var form = new Form(ramContent, soundDirectoryPath) {BackColor = Color.Cyan};
                Application.Run(form);
            }
        }

        static string InstallSounds()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var assemblyName = assembly.GetName().Name;
            var outputDirectoryPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                assemblyName);

            Directory.CreateDirectory(outputDirectoryPath);

            var resourceNamePrefix = $"{assemblyName}.Sounds.";

            var manifestResourceNames = assembly.GetManifestResourceNames();
            foreach (var s in manifestResourceNames.Where(s => s.StartsWith(resourceNamePrefix) && s.EndsWith(".wav")))
            {
                using (var inputStream = assembly.GetManifestResourceStream(s))
                {
                    var buffer = new byte[inputStream.Length];
                    inputStream.Read(buffer, 0, buffer.Length);

                    var outputFilename = s.Substring(resourceNamePrefix.Length);
                    var outputFilePath = Path.Combine(outputDirectoryPath, outputFilename);

                    File.WriteAllBytes(outputFilePath, buffer);
                }
            }

            return outputDirectoryPath;
        }
    }
}