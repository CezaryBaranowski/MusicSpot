using MusicSpot.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace MusicSpot.Views
{
    /// <summary>
    /// Interaction logic for Music.xaml
    /// </summary>
    public partial class MusicView : UserControl
    {
        public MusicViewModel dataModel;

        public MusicView()
        {
            InitializeComponent();
            dataModel = MusicViewModel.GetInstance();
            this.DataContext = dataModel;
        }

        private void PlayButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (dataModel.IsPlaying == false)
                dataModel.IsPlaying = true;
            else dataModel.IsPlaying = false;
            //this.DataContext = dataModel;
        }

        private void ChangeMusicDirectory(object sender, SelectionChangedEventArgs e)
        {
            MusicViewModel.GetInstance().LoadMusicFilesAsync();
        }

        private void Mute(object sender, RoutedEventArgs e)
        {
            if (dataModel.IsMuted == false)
                dataModel.IsMuted = true;
            else dataModel.IsMuted = false;
        }
    }
}
