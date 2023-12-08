using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
                // Create a new SerializationInfo object
                var serializationInfo = new SerializationInfo(typeof(ViewModel), new FormatterConverter());

                // Add the PrijateljiList collection to the serialization info
                serializationInfo.AddValue("PrijateljiList", PrijateljiList);

                // Add the SeznamObjav collection to the serialization info
                serializationInfo.AddValue("SeznamObjav", SeznamObjav);

                // Add the MeData property to the serialization info
                serializationInfo.AddValue("MeData", MeData);

                // Create an XmlWriter to write to the XML file
                using (var streamWriter = new StreamWriter(filePath))
                {
                    var xmlWriter = new XmlTextWriter(streamWriter);
                    xmlWriter.Formatting = Formatting.Indented;

                    // Start an XML document
                    xmlWriter.WriteStartDocument();

                    // Write the PrijateljiList collection
                    xmlWriter.WriteStartElement("PrijateljiList");
                    foreach (var prijatelj in PrijateljiList)
                    {
                        prijatelj.SaveDataToXml(xmlWriter); // Recursively serialize each prijatelj
                    }
                    xmlWriter.WriteEndElement(); // Close PrijateljiList element

                    // Write the SeznamObjav collection
                    xmlWriter.WriteStartElement("SeznamObjav");
                    foreach (var objava in SeznamObjav)
                    {
                        objava.SaveDataToXml(xmlWriter); // Recursively serialize each objava
                    }
                    xmlWriter.WriteEndElement(); // Close SeznamObjav element

                    // Write the MeData property
                    xmlWriter.WriteStartElement("MeData");
                    foreach (var property in MeData.GetType().GetProperties())
                    {
                        var value = property.GetValue(MeData);
                        if (value != null)
                        {
                            xmlWriter.WriteStartElement(property.Name);
                            xmlWriter.WriteString(value.ToString());
                            xmlWriter.WriteEndElement();
                        }
                    }
                    xmlWriter.WriteEndElement(); // Close MeData element

                    // End the XML document
                    xmlWriter.WriteEndDocument();
                    xmlWriter.Flush();
                    xmlWriter.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data to XML: {ex.Message}", "Error");
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

    }
} 
