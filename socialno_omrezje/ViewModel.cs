using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Serialization;

namespace socialno_omrezje
{
    [Serializable]
    public class ViewModel : ViewModelBase, ISerializable, INotifyPropertyChanged
    {

        [XmlIgnore] public RelayCommand DodajPrijateljaCommand { get; set; }
        [XmlIgnore] public RelayCommand OdstraniPrijateljaCommand { get; set; }
        [XmlIgnore] public RelayCommand UrediPrijateljaCommand { get; set; }
        [XmlIgnore] public RelayCommand OpenAddPostWindowCommand { get; set; }
        [XmlIgnore] public RelayCommand OpenImageDialogCommand { get; set; }
        [XmlIgnore] public RelayCommand OdstraniCommand { get; set; }
        [XmlIgnore] public RelayCommand UrediCommand { get; set; }
        public ICommand ChangeProfilePictureCommand { get; private set; }
        public ICommand SaveUserDataCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        //--------------------------------------------------------------------
        // OBSERVABLE-COLLECTIONS
        public ObservableCollection<PostData> WallPosts { get; set; }
        private ObservableCollection<FriendData> prijateljiList;
        public ObservableCollection<FriendData> PrijateljiList
        {
            get { return prijateljiList; }
            set
            {
                Set(ref prijateljiList, value);
                RaisePropertyChanged(nameof(CanOdstraniPrijatelja));
                RaisePropertyChanged(nameof(CanUrediPrijatelja));
            }
        }
        private FriendData izbraniPrijatelj;
        public FriendData IzbraniPrijatelj
        {
            get { return izbraniPrijatelj; }
            set { Set(ref izbraniPrijatelj, value); }
        }

        private ObservableCollection<PostData> seznamObjav;
        public ObservableCollection<PostData> SeznamObjav
        {
            get { return seznamObjav; }
            set { Set(ref seznamObjav, value); }
        }

        private MeData meData;
        public MeData MeData
        {
            get { return meData; }
            set { Set(ref meData, value); }
        }

        private MeData tempMeData;
        public MeData TempMeData
        {
            get { return tempMeData; }
            set { Set(ref tempMeData, value); }
        }


        private PostData izbranaObjava;
        public PostData IzbranaObjava
        {
            get { return izbranaObjava; }
            set { Set(ref izbranaObjava, value); }
        }

        //--------------------------------------------------------------------
        // VIEW-MODEL
        public ViewModel()
        {
            // Initialize the non-serializable properties here
            PrijateljiList = new ObservableCollection<FriendData>();
            SeznamObjav = new ObservableCollection<PostData>();

            // Create a new instance for MeData and TempMeData
            MeData = new MeData();
            TempMeData = new MeData(MeData);
            SaveUserDataCommand = new RelayCommand(SaveUserData);

            ChangeProfilePictureCommand = new RelayCommand(ChangeProfilePicture);
            DodajPrijateljaCommand = new RelayCommand(DodajPrijatelja);
            OdstraniPrijateljaCommand = new RelayCommand(OdstraniPrijatelja, CanOdstraniPrijatelja);
            UrediPrijateljaCommand = new RelayCommand(UrediPrijatelja, CanUrediPrijatelja);
            OpenAddPostWindowCommand = new RelayCommand(OpenAddPostWindow);
            OpenImageDialogCommand = new RelayCommand(OpenImageDialog);
            UrediCommand = new RelayCommand(UrediObjavo, CanUredi);
        }


        //--------------------------------------------------------------------
        // FUNCTIONS

        private void SaveUserData()
        {
            TempMeData.Update(MeData);
            SaveUserDataToSettings();
            MessageBox.Show("User data updated!");
        }

        private void SaveUserDataToSettings()
        {
        if (MeData != null)
        {
            // TODO
        }

        }

        
        private void UrediObjavo()
        {
            if (IzbranaObjava != null)
            {
                editPostWindow editWindow = new editPostWindow(SeznamObjav, IzbranaObjava);
                editWindow.ShowDialog();
            }
        }

        private bool CanUredi()
        {
            return true;
        }


        private void DodajPrijatelja()
        {
            PrijateljiList.Add(new FriendData
            {
                Ime = "New Friend",
                Slika = new BitmapImage(new Uri("/assets/facebook_image.jpg", UriKind.Relative)),
                Status = GetRandomStatus()
            });
        }



        private void OdstraniPrijatelja()
        {
            if (IzbraniPrijatelj != null)
            {
                PrijateljiList.Remove(IzbraniPrijatelj);
            }
        }

        public bool CanOdstraniPrijatelja()
        {
            return true;
        }

        private void UrediPrijatelja()
        {
            if (IzbraniPrijatelj != null)
            {
                IzbraniPrijatelj.Ime = "Edited Friend";
                RaisePropertyChanged(nameof(CanUrediPrijatelja));
            }
        }

        public bool CanUrediPrijatelja()
        {
            return true;
        }

        private string GetRandomStatus()
        {
            Random random = new Random();
            return random.Next(2) == 0 ? "Online" : "Offline";
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
                string selectedImagePath = openFileDialog.FileName;
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(SeznamObjav), SeznamObjav);
            info.AddValue(nameof(PrijateljiList), PrijateljiList);
            info.AddValue(nameof(MeData), MeData);
        }
        public static void SaveData<T>(T data, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, data);
            }
        }

        public static T LoadData<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return default(T);
            }

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (StreamReader reader = new StreamReader(filePath))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        private void ChangeProfilePicture()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files |*.jpg;*.jpeg;*.png|All Files |*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                MeData.Slika = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 
