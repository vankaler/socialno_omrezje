using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace socialno_omrezje
{
    [Serializable]
    public class ViewModel : ViewModelBase, ISerializable
    {

        [XmlIgnore] public RelayCommand DodajPrijateljaCommand { get; set; }
        [XmlIgnore] public RelayCommand OdstraniPrijateljaCommand { get; set; }
        [XmlIgnore] public RelayCommand UrediPrijateljaCommand { get; set; }
        [XmlIgnore] public RelayCommand OpenAddPostWindowCommand { get; set; }
        [XmlIgnore] public RelayCommand OpenImageDialogCommand { get; set; }
        [XmlIgnore] public RelayCommand OdstraniCommand { get; set; }
        [XmlIgnore] public RelayCommand UrediCommand { get; set; }


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

        private PostData izbranaObjava;
        public PostData IzbranaObjava
        {
            get { return izbranaObjava; }
            set { Set(ref izbranaObjava, value); }
        }

        //--------------------------------------------------------------------
        // VIEW-MODEL
        public ViewModel()
        { // Initialize the non-serializable properties here
            PrijateljiList = new ObservableCollection<FriendData>();
            SeznamObjav = new ObservableCollection<PostData>();
            MeData = new MeData();

            DodajPrijateljaCommand = new RelayCommand(DodajPrijatelja);
            OdstraniPrijateljaCommand = new RelayCommand(OdstraniPrijatelja, CanOdstraniPrijatelja);
            UrediPrijateljaCommand = new RelayCommand(UrediPrijatelja, CanUrediPrijatelja);
            OpenAddPostWindowCommand = new RelayCommand(OpenAddPostWindow);
            OpenImageDialogCommand = new RelayCommand(OpenImageDialog);
            UrediCommand = new RelayCommand(UrediObjavo, CanUredi);
        }

            //--------------------------------------------------------------------
            // FUNCTIONS
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
                Slika = new BitmapImage(new Uri("C:/Users/nejcp/OneDrive/Desktop/II letnik/I_semester/uporabniški_vmesniki/vaje/socialno_omrezje/socialno_omrezje/bin/Images/facebook_image.jpg")),
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
        public static ViewModel LoadDataFromXml(string filePath)
        {
            try
            {
                using (var fs = new FileStream(filePath, FileMode.Open))
                {
                    var serializer = new XmlSerializer(typeof(ViewModel));
                    var viewModel = (ViewModel)serializer.Deserialize(fs);

                    // Make sure PrijateljiList is not null
                    if (viewModel.PrijateljiList == null)
                        viewModel.PrijateljiList = new ObservableCollection<FriendData>();

                    return viewModel;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data from XML: {ex.Message}", "Error");
                return null;
            }
        }


        public void SaveDataToXml(string filePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ViewModel));
                using (TextWriter writer = new StreamWriter(filePath))
                {
                    
                    serializer.Serialize(writer, this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data to XML: {ex.Message}", "Error");
            }
        }
    }
}
