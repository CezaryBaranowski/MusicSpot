using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MusicSpot.ViewModels
{
    public sealed class MusicViewModel : ViewModelBase
    {

        private static readonly MusicViewModel musicViewModel = new MusicViewModel();

        private MusicViewModel()
        {
            SettingsViewModel = SettingsViewModel.GetInstance();
            IsPlaying = false;
            IsEditingFilesEnabled = false;
            MusicSelectedDirectory = "All";
            RefreshMusicDirectories();
            LoadMusicFiles();
        }

        public static MusicViewModel GetInstance()
        {
            return musicViewModel;
        }

        public async void LoadMusicFiles()
        {
            IList<string> directories = new List<string>();
            directories = GetMusicDirectories();
        }

        #region Properties

        private SettingsViewModel SettingsViewModel;

        private bool _isPlaying;
        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                _isPlaying = value;
                OnPropertyChanged(nameof(IsPlaying));
            }
        }
        private bool _isEditingFilesEnables;
        public bool IsEditingFilesEnabled
        {
            get => _isEditingFilesEnables;
            set
            {
                _isEditingFilesEnables = value;
                OnPropertyChanged(nameof(IsEditingFilesEnabled));
            }
        }

        private ObservableCollection<string> _musicDirectories;
        public ObservableCollection<string> MusicDirectories
        {
            get => _musicDirectories;
            set
            {
                _musicDirectories = value;
                OnPropertyChanged();
            }
        }

        private string _musicSelectedDirectory;
        public string MusicSelectedDirectory
        {
            get => _musicSelectedDirectory;
            set
            {
                _musicSelectedDirectory = value;
                OnPropertyChanged();
            }
        }

        #endregion

        private IList<string> GetMusicDirectories()
        {
            if (MusicSelectedDirectory.Equals("All")) return SettingsViewModel.MusicDirectories;
            return new List<string> { MusicSelectedDirectory };
        }

        public void RefreshMusicDirectories()
        {
            _musicDirectories = new ObservableCollection<string>(SettingsViewModel.GetInstance().MusicDirectories);
            _musicDirectories.Insert(0, "All");
            OnPropertyChanged(nameof(MusicDirectories));
        }
    }
}
