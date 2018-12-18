namespace MusicSpot.ViewModels
{
    class DiscoveryViewModel
    {

        private static readonly DiscoveryViewModel discoveryViewModel = new DiscoveryViewModel();

        static DiscoveryViewModel()
        {
        }

        private DiscoveryViewModel()
        {
            InitDiscoveryViewModel();
        }

        public static DiscoveryViewModel GetInstance()
        {
            return discoveryViewModel;
        }

        private void InitDiscoveryViewModel()
        {

        }
    }
}
