using MahApps.Metro.Controls;
using MusicSpot.ViewModels;

namespace MusicSpot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        protected MainViewModel dataModel;
        public MainWindow()
        {
            InitializeComponent();
            // dataModel = MainViewModel.GetInstance();
            this.DataContext = MainViewModel.GetInstance();

        }

    }
}
