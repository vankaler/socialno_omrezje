using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace socialno_omrezje
{
    public class Friend : ViewModelBase
    {
        private string ime;
        public string Ime
        {
            get { return ime; }
            set
            {
                ime = value;
                RaisePropertyChanged();
            }
        }

        private BitmapImage slika;
        public BitmapImage Slika
        {
            get { return slika; }
            set
            {
                slika = value;
                RaisePropertyChanged();
            }
        }

        private string status;
        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                RaisePropertyChanged();
            }
        }
    }

    public class Me : ViewModelBase
    {
        private string ime;
        public string Ime
        {
            get { return ime; }
            set
            {
                ime = value;
                RaisePropertyChanged();
            }
        }

        private string naslov;
        public string Naslov
        {
            get { return naslov; }
            set
            {
                naslov = value;
                RaisePropertyChanged();
            }
        }

        private string opis;
        public string Opis
        {
            get { return opis; }
            set
            {
                opis = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand SaveUserDataCommand { get; set; }

        public Me()
        {
            SaveUserDataCommand = new RelayCommand(SaveUserData);
        }

        private void SaveUserData()
        {
            // Save user data to application settings
            SaveUserDataToSettings();
            MessageBox.Show("User data saved!");
        }

        private void SaveUserDataToSettings()
        {
            // Implement your logic to save user data to application settings here
            // For example, you can use Properties.Settings.Default
            // Properties.Settings.Default.Ime = Ime;
            // Properties.Settings.Default.Naslov = Naslov;
            // Properties.Settings.Default.Opis = Opis;
            // Properties.Settings.Default.Save();
        }
    }



    public class ViewModel : ViewModelBase
    {
        public ViewModel()
        {
            SeznamObjav = new ObservableCollection<Data>();
            PrijateljiList = new ObservableCollection<Friend>();
            OdstraniCommand = new RelayCommand(OdstraniObjavo, CanOdstrani);
            UrediCommand = new RelayCommand(UrediObjavo, CanUredi);
            GoBackCommand = new RelayCommand(GoBack);
            OpenAddPostWindowCommand = new RelayCommand(OpenAddPostWindow);
            OpenImageDialogCommand = new RelayCommand(() => OpenImageDialog());
            DodajPrijateljaCommand = new RelayCommand(DodajPrijatelja);
            OdstraniPrijateljaCommand = new RelayCommand(OdstraniPrijatelja, CanOdstraniPrijatelja);
            UrediPrijateljaCommand = new RelayCommand(UrediPrijatelja, CanUrediPrijatelja);
            MeData = new Me();
        }

        private ObservableCollection<Friend> prijateljiList;
        public ObservableCollection<Friend> PrijateljiList
        {
            get { return prijateljiList; }
            set
            {
                prijateljiList = value;
                RaisePropertyChanged();
            }
        }

        public ICommand DodajPrijateljaCommand { get; set; }
        public ICommand OdstraniPrijateljaCommand { get; set; }
        public ICommand UrediPrijateljaCommand { get; set; }

        // Add these methods to handle friend operations
        private void DodajPrijatelja()
        {
            // Implement logic to add a friend
            BitmapImage image = new BitmapImage(new Uri("C:/Users/nejcp/OneDrive/Desktop/II letnik/I_semester/uporabniški_vmesniki/vaje/socialno_omrezje/socialno_omrezje/bin/Images/facebook_image.jpg"));
            PrijateljiList.Add(new Friend
            {
                Ime = "New Friend",
                Slika = image,
                Status = GetRandomStatus()
            });
            RaisePropertyChanged(nameof(CanOdstraniPrijatelja));
            RaisePropertyChanged(nameof(CanUrediPrijatelja));
        }

        private void OdstraniPrijatelja()
        {
            // Implement logic to remove a friend
            if (PrijateljiList.Count > 0)
            {
                PrijateljiList.RemoveAt(0);
                RaisePropertyChanged(nameof(CanOdstraniPrijatelja));
                RaisePropertyChanged(nameof(CanUrediPrijatelja));
            }
        }

        public bool CanOdstraniPrijatelja
        {
            get { return PrijateljiList.Count > 0; }
        }

        private void UrediPrijatelja()
        {
            if (PrijateljiList.Count > 0)
            {
                PrijateljiList[0].Ime = "Edited Friend";
                RaisePropertyChanged(nameof(CanUrediPrijatelja));
            }
        }

        public bool CanUrediPrijatelja
        {
            get { return PrijateljiList.Count > 0; }
        }

        private string GetRandomStatus()
        {
            // Generates a random status (online or offline)
            Random random = new Random();
            return random.Next(2) == 0 ? "Online" : "Offline";
        }

        public RelayCommand OpenAddPostWindowCommand { get; set; }
        public RelayCommand OpenImageDialogCommand { get; set; }

        private ObservableCollection<Data> seznamObjav;
        public ObservableCollection<Data> SeznamObjav
        {
            get { return seznamObjav; }
            set
            {
                seznamObjav = value;
                RaisePropertyChanged();
            }
        }

        private Data izbranaObjava;
        public Data IzbranaObjava
        {
            get { return izbranaObjava; }
            set
            {
                izbranaObjava = value;
                RaisePropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private Me meData;
        public Me MeData
        {
            get { return meData; }
            set
            {
                meData = value;
                RaisePropertyChanged();
            }
        }

        public ICommand DodajCommand { get; set; }
        public ICommand OdstraniCommand { get; set; }
        public ICommand UrediCommand { get; set; }
        public ICommand GoBackCommand { get; }

        private void OdstraniObjavo()
        {
            if (IzbranaObjava != null)
            {
                SeznamObjav.Remove(IzbranaObjava);
            }
        }

        private bool CanOdstrani()
        {
            return true;
        }

        private void UrediObjavo()
        {
            if (IzbranaObjava != null)
            {
                // Open the editPostWindow with the selected post
                editPostWindow editWindow = new editPostWindow(SeznamObjav, IzbranaObjava);
                editWindow.ShowDialog();
            }
        }

        private bool CanUredi()
        {
            return true;
        }

        private void GoBack()
        {
            var currentWindow = Application.Current.MainWindow;
            currentWindow.Close();
        }

        private void OpenAddPostWindow()
        {
            addPostWindow addPostWindow = new addPostWindow(SeznamObjav);
            addPostWindow.ShowDialog();
        }

        private void OpenImageDialog()
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Select an Image",
                Filter = "Image Files (*.jpg; *.jpeg; *.png; *.gif; *.bmp)|*.jpg; *.jpeg; *.png; *.gif; *.bmp|All Files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Process the selected image
                string selectedImagePath = openFileDialog.FileName;
            }
        }
    }
}
