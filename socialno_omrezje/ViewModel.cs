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
    public class ViewModel : ViewModelBase
    {
        public ViewModel()
        {
            SeznamObjav = new ObservableCollection<Data>();
            DodajCommand = new RelayCommand(DodajObjavo);
            OdstraniCommand = new RelayCommand(OdstraniObjavo, CanOdstrani);
            UrediCommand = new RelayCommand(UrediObjavo, CanUredi);
            GoBackCommand = new RelayCommand(GoBack);
            OpenAddPostWindowCommand = new RelayCommand(OpenAddPostWindow);
            OpenImageDialogCommand = new RelayCommand(() => OpenImageDialog());
        }

        private ObservableCollection<string> prijateljiList;
        public ObservableCollection<string> PrijateljiList
        {
            get { return prijateljiList; }
            set
            {
                prijateljiList = value;
                RaisePropertyChanged();
            }
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

        public ICommand DodajCommand { get; set; }
        public ICommand OdstraniCommand { get; set; }
        public ICommand UrediCommand { get; set; }
        public RelayCommand GoBackCommand { get; }

        private void DodajObjavo()
        {
            BitmapImage image = new BitmapImage(new Uri("C:/Users/nejcp/OneDrive/Desktop/II letnik/I_semester/uporabniški_vmesniki/vaje/socialno_omrezje/socialno_omrezje/bin/Images/cat_vegetables.jpg"));
            SeznamObjav.Add(new Data
            {
                Vsebina = "What's up",
                Slika = image,
                DatumObjave = DateTime.Now,
                Lokacija = "Nova Lokacija",
                Likes = 14,
                Content = "He does not like vegetables."
            });
        }

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
            addPostWindow addPostWindow = new addPostWindow(SeznamObjav); // Pass the ObservableCollection<Data>
            addPostWindow.ShowDialog();
        }

        private void OpenImageDialog()
        {
            // Implement your image dialog logic here
            // For example, you can use OpenFileDialog to allow users to select an image
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
                // Perform further actions if needed
            }
        }
    }
}
