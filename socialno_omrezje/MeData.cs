using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Configuration;
using System.IO;
using System.Windows;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows.Media.Imaging;

namespace socialno_omrezje
{
    public class MeData : ViewModelBase, INotifyPropertyChanged
    {
        public RelayCommand SaveUserDataCommand { get; set; }
        public RelayCommand ToggleEditModeCommand { get; set; }

        private ViewModel viewModel;

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

        private string naslov;
        public string Naslov
        {
            get { return naslov; }
            set
            {
                if (naslov != value)
                {
                    naslov = value;
                    RaisePropertyChanged(nameof(Naslov));
                }
            }
        }

        private string opis;
        public string Opis
        {
            get { return opis; }
            set
            {
                if (opis != value)
                {
                    opis = value;
                    RaisePropertyChanged(nameof(Opis));
                }
            }
        }

        private bool isEditMode;
        public bool IsEditMode
        {
            get { return isEditMode; }
            set
            {
                if (isEditMode != value)
                {
                    isEditMode = value;
                    RaisePropertyChanged(nameof(IsEditMode));
                }
            }
        }
        private MeData tempMeData;
        public MeData TempMeData
        {
            get { return tempMeData; }
            set { Set(ref tempMeData, value); }
        }


        public MeData(MeData other)
        {
            Ime = other.Ime;
            Naslov = other.Naslov;
            Opis = other.Opis;
            Slika = other.Slika;
            CopyFrom(other);
        }

        public MeData()
        {
            SaveUserDataCommand = new RelayCommand(SaveUserData);
            ToggleEditModeCommand = new RelayCommand(ToggleEditMode);
        }

        public void CopyFrom(MeData other)
        {
            Ime = other.Ime;
            Naslov = other.Naslov;
            Opis = other.Opis;

            if (other.Slika != null)
            {
                using (var stream = new MemoryStream())
                {
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(other.Slika));
                    encoder.Save(stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    Slika = new BitmapImage();
                    Slika.BeginInit();
                    Slika.CacheOption = BitmapCacheOption.OnLoad;
                    Slika.StreamSource = stream;
                    Slika.EndInit();
                    Slika.Freeze();
                }
            }
            else
            {
                Slika = null;
            }
        }

        public void Update(MeData other)
        {
            CopyFrom(other);
        }
        private void ToggleEditMode()
        {
            IsEditMode = !IsEditMode;
        }

        private void SaveUserData()
        {
            if (viewModel != null)
            {
                viewModel.MeData.Update(viewModel.TempMeData);

                viewModel.MeData.SaveUserDataToSettings(); 

                viewModel.RaisePropertyChanged(nameof(viewModel.MeData));

                MessageBox.Show("User data saved!");
            }
        }

        private void SaveUserDataToSettings()
        {
            if (IsEditMode)
            {
                // Save user data to configuration file
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                // Add or update settings
                if (config.AppSettings.Settings["Ime"] == null)
                {
                    config.AppSettings.Settings.Add("Ime", Ime);
                }
                else
                {
                    config.AppSettings.Settings["Ime"].Value = Ime;
                }

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Ime), Ime);
            info.AddValue(nameof(Naslov), Naslov);
            info.AddValue(nameof(Opis), Opis);
        }
    }
}
