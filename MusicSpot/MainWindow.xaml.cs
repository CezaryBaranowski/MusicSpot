using MusicSpot.ViewModels;
using System.Windows;

namespace MusicSpot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        protected readonly MainViewModel dataModel;
        public MainWindow()
        {
            InitializeComponent();
            dataModel = MainViewModel.GetInstance();
            DataContext = dataModel;
        }

        private void PlayButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (dataModel.IsPlaying == false)
                dataModel.IsPlaying = true;
            else dataModel.IsPlaying = false;


        }
    }
}
