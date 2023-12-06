using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

namespace socialno_omrezje
{
    public class Data : INotifyPropertyChanged
    {
        private string vsebina;
        public string Vsebina
        {
            get { return vsebina; }
            set
            {
                if (vsebina != value)
                {
                    vsebina = value;
                    OnPropertyChanged();
                }
            }
        }
        private string content;
        public string Content 
        {
            get { return content; }
            set
            {
                if (content != value)
                {
                    content = value;
                    OnPropertyChanged();
                }
            }
        }
        private string imagePath;
        public string ImagePath
        {
            get { return imagePath; }
            set
            {
                if (imagePath != value)
                {
                    imagePath = value;
                    OnPropertyChanged();
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
                    OnPropertyChanged();
                }
            }
        }

        private List<string> prijatelji;
        public List<string> Prijatelji
        {
            get { return prijatelji; }
            set
            {
                if (prijatelji != value)
                {
                    prijatelji = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime datumObjave;
        public DateTime DatumObjave
        {
            get { return datumObjave; }
            set
            {
                if (datumObjave != value)
                {
                    datumObjave = value;
                    OnPropertyChanged();
                }
            }
        }

        private string lokacija;
        public string Lokacija
        {
            get { return lokacija; }
            set
            {
                if (lokacija != value)
                {
                    lokacija = value;
                    OnPropertyChanged();
                }
            }
        }

        private int likes;
        public int Likes
        {
            get { return likes; }
            set
            {
                if (likes != value)
                {
                    likes = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
