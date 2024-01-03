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
    [Serializable]
    public class MeData : ViewModelBase, ISerializable, INotifyPropertyChanged
    {
        [XmlIgnore] public RelayCommand SaveUserDataCommand { get; set; }
        [XmlIgnore] public RelayCommand ToggleEditModeCommand { get; set; }

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

            // Ensure the BitmapImage is cloned to avoid reference issues
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
            // Assuming you have an instance of ViewModel named viewModel
            if (viewModel != null)
            {
                // Update the properties of MeData from TempMeData
                viewModel.MeData.Update(viewModel.TempMeData);

                // Save user data to settings
                viewModel.MeData.SaveUserDataToSettings(); // Call the method from MeData

                // Notify that the properties have changed
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

                // ... (similar updates for other properties)

                // Save changes
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }


        public static MeData LoadDataFromXml()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "XML Files (*.xml)|*.xml"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                try
                {
                    using (var fs = new FileStream(filePath, FileMode.Open))
                    {
                        var serializer = new XmlSerializer(typeof(MeData));
                        return (MeData)serializer.Deserialize(fs);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading data from XML: {ex.Message}", "Error");
                    return null;
                }
            }

            return null;
        }

        // Method to save ViewModel data to XML
        public void SaveDataToXml()
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
                        var serializer = new XmlSerializer(typeof(MeData));
                        serializer.Serialize(fs, this);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving data to XML: {ex.Message}", "Error");
                }
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
