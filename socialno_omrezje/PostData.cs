using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows.Media.Imaging;
using System.Xml;

namespace socialno_omrezje
{
    [Serializable]
    public class PostData : ViewModelBase, INotifyPropertyChanged, ISerializable
    {

        //-------------------------------------------------------------------------
        // OBSERVABLE COLLECTION
        private ObservableCollection<PostData> postList;
        public ObservableCollection<PostData> PostList
        {
            get { return postList; }
            set
            {
                if (postList != value)
                {
                    postList = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<FriendData> friendList;
        public ObservableCollection<FriendData> FriendList
        {
            get { return friendList; }
            set
            {
                if (friendList != value)
                {
                    friendList = value;
                    OnPropertyChanged();
                }
            }
        }

        //-------------------------------------------------------------------------
        // POST DATA

        public PostData()
        {
            PostList = new ObservableCollection<PostData>();
            FriendList = new ObservableCollection<FriendData>();
        }

        protected PostData(SerializationInfo info, StreamingContext context)
        {
            Vsebina = info.GetString("Vsebina");
            Content = info.GetString("Content");
            ImagePath = info.GetString("ImagePath");
            var imageBytes = (byte[])info.GetValue("Slika", typeof(byte[]));
            Slika = LoadImageFromBytes(imageBytes);
            DatumObjave = info.GetDateTime("DatumObjave");
            Lokacija = info.GetString("Lokacija");
            Likes = info.GetInt32("Likes");
            PostList = (ObservableCollection<PostData>)info.GetValue("SeznamObjav", typeof(ObservableCollection<PostData>));
        }

        //-------------------------------------------------------------------------
        // POST STRUCTURE

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

        private byte[] SaveImageToBytes(BitmapImage image)
        {
            if (image != null)
            {
                var encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    return stream.ToArray();
                }
            }
            return null;
        }

        private BitmapImage LoadImageFromBytes(byte[] imageData)
        {
            if (imageData != null)
            {
                var image = new BitmapImage();
                using (var stream = new MemoryStream(imageData))
                {
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = stream;
                    image.EndInit();
                }
                return image;
            }
            return null;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Vsebina), Vsebina);
            info.AddValue(nameof(Content), Content);
            info.AddValue(nameof(ImagePath), ImagePath);
            info.AddValue(nameof(Slika), SaveImageToBytes(Slika));
            info.AddValue(nameof(FriendList), FriendList.ToList());
            info.AddValue(nameof(DatumObjave), DatumObjave);
            info.AddValue(nameof(Lokacija), Lokacija);
            info.AddValue(nameof(Likes), Likes);
            info.AddValue(nameof(PostList), PostList.ToList());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
