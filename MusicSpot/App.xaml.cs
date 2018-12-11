using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace MusicSpot
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IList<string> StartupSongsPaths = new List<string>();

        protected override void OnStartup(StartupEventArgs e)
        {
            if (e.Args.Length > 0)
            {
                for (int i = 0; i < e.Args.Length; i++)
                {

                    FileInfo file = new FileInfo(e.Args[i]);
                    if (file.Exists)
                    {
                        StartupSongsPaths.Add(e.Args[i]);
                    }
                }

            }
        }
    }
}
