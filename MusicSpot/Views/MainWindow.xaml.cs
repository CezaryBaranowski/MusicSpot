using MahApps.Metro.Controls;
using MusicSpot.ViewModels;

namespace MusicSpot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            // dataModel = MainViewModel.GetInstance();
            this.DataContext = MainViewModel.GetInstance();
        }

        //private void MusicView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    MessageBox.Show(MainViewModel.GetInstance().SelectedTabIndex.ToString());
        //}
    }
}
