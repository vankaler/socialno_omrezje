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
    public class FriendData : ViewModelBase
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

