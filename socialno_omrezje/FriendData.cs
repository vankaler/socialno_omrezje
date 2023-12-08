using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace socialno_omrezje
{
    [Serializable]
    public class FriendData : ViewModelBase, ISerializable
    {
        public RelayCommand DodajPrijateljaCommand { get; set; }
        public RelayCommand OdstraniPrijateljaCommand { get; set; }
        public RelayCommand UrediPrijateljaCommand { get; set; }

        private string ime;
        public string Ime
        {
            get { return ime; }
            set
            {
                if (ime != value)
                {
                    ime = value;
                    RaisePropertyChanged(nameof(Ime));
                }
            }
        }

        private BitmapImage slika;
        public BitmapImage Slika
        {
            get { return slika; }
            set
            {
                if (slika != value)
                {
                    slika = value;
                    RaisePropertyChanged(nameof(Slika));
                }
            }
        }

        private string status;
        public string Status
        {
            get { return status; }
            set
            {
                if (status != value)
                {
                    status = value;
                    RaisePropertyChanged(nameof(Status));
                }
            }
        }


        

        private ObservableCollection<FriendData> prijateljiList;
        public ObservableCollection<FriendData> PrijateljiList
        {
            get { return prijateljiList; }
            set
            {
                if (prijateljiList != value)
                {
                    prijateljiList = value;
                    RaisePropertyChanged(nameof(PrijateljiList));
                }
            }
        }

        public FriendData()
        {
            PrijateljiList = new ObservableCollection<FriendData>();
            DodajPrijateljaCommand = new RelayCommand(DodajPrijatelja);
            OdstraniPrijateljaCommand = new RelayCommand(OdstraniPrijatelja, CanOdstraniPrijatelja);
            UrediPrijateljaCommand = new RelayCommand(UrediPrijatelja, CanUrediPrijatelja);
        }

        private void DodajPrijatelja()
        {
            // Implement logic to add a friend
            BitmapImage image = new BitmapImage(new Uri("C:/Users/nejcp/OneDrive/Desktop/II letnik/I_semester/uporabniški_vmesniki/vaje/socialno_omrezje/socialno_omrezje/bin/Images/facebook_image.jpg"));
            PrijateljiList.Add(new FriendData
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

        public bool CanOdstraniPrijatelja()
        {
            return true;
        }

        private void UrediPrijatelja()
        {
            if (PrijateljiList.Count > 0)
            {
                PrijateljiList[0].Ime = "Edited Friend";
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

        public void SaveDataToXml(System.Xml.XmlTextWriter xmlWriter)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "XML Files (*.xml)|*.xml"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;

                try
                {
                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        var serializer = new XmlSerializer(typeof(ObservableCollection<FriendData>));
                        serializer.Serialize(fs, PrijateljiList);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving data to XML: {ex.Message}", "Error");
                }
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Ime), Ime);
            info.AddValue(nameof(Slika), Slika);
            info.AddValue(nameof(Status), Status);
            info.AddValue(nameof(PrijateljiList), PrijateljiList.ToList());
        }
    }
}

