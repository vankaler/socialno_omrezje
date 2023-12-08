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

namespace socialno_omrezje
{
    [Serializable]
    public class MeData : ViewModelBase, ISerializable, INotifyPropertyChanged
    {
        [XmlIgnore] public RelayCommand SaveUserDataCommand { get; set; }

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

        public MeData()
        {
            SaveUserDataCommand = new RelayCommand(SaveUserData);
        }

        private void SaveUserData()
        {
            SaveUserDataToSettings();
            MessageBox.Show("User data saved!");
        }

        private void SaveUserDataToSettings()
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

            if (config.AppSettings.Settings["Naslov"] == null)
            {
                config.AppSettings.Settings.Add("Naslov", Naslov);
            }
            else
            {
                config.AppSettings.Settings["Naslov"].Value = Naslov;
            }

            if (config.AppSettings.Settings["Opis"] == null)
            {
                config.AppSettings.Settings.Add("Opis", Opis);
            }
            else
            {
                config.AppSettings.Settings["Opis"].Value = Opis;
            }

            // Save changes
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
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
