using GalaSoft.MvvmLight;
using Microsoft.Win32;
using socialno_omrezje;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Windows;
using System;
using System.Linq;
using GalaSoft.MvvmLight.Command;

public class AddPostViewModel : ViewModelBase
{
    private BitmapImage selectedImage;
    private string title;
    private string description;

    public BitmapImage SelectedImage
    {
        get { return selectedImage; }
        set
        {
            if (selectedImage != value)
            {
                selectedImage = value;
                RaisePropertyChanged(nameof(SelectedImage));
            }
        }
    }

    public string Title
    {
        get { return title; }
        set
        {
            if (title != value)
            {
                title = value;
                RaisePropertyChanged(nameof(Title));
            }
        }
    }

    public string Description
    {
        get { return description; }
        set
        {
            if (description != value)
            {
                description = value;
                RaisePropertyChanged(nameof(Description));
            }
        }
    }

    public ObservableCollection<PostData> WallPosts { get; set; }

    private ObservableCollection<FriendData> prijateljiList;
    public ObservableCollection<FriendData> PrijateljiList
    {
        get { return prijateljiList; }
        set { Set(ref prijateljiList, value); }
    }

    public RelayCommand OpenImageDialogCommand { get; set; }
    public RelayCommand AddPostCommand { get; set; }
    public RelayCommand GoBackCommand { get; set; }

    public AddPostViewModel(ObservableCollection<PostData> wallPosts, ObservableCollection<FriendData> prijateljiList)
    {
        WallPosts = wallPosts;

        OpenImageDialogCommand = new RelayCommand(OpenImageDialog);
        AddPostCommand = new RelayCommand(AddPost);
        GoBackCommand = new RelayCommand(GoBack);
        this.prijateljiList = prijateljiList;
    }

    private void OpenImageDialog()
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Image Files |*.jpg;*.jpeg;*.png|All Files |*.*"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            SelectedImage = new BitmapImage(new Uri(openFileDialog.FileName));
        }
    }

    private void AddPost()
    {

        if (Description != null && Title != null && SelectedImage != null)
        {
            WallPosts.Add(new PostData
            {
                Vsebina = Description,
                Content = Title,
                Slika = SelectedImage,
                DatumObjave = DateTime.Now,
                Likes = new Random().Next(0, 101),
            });

            // Close the addPostWindow
            Close();
        }
    }

    private void GoBack()
    {
        var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
        if (currentWindow != null)
        {
            currentWindow.Close();
        }
    }

    private void Close()
    {
        var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
        if (currentWindow != null)
        {
            currentWindow.Close();
        }
    }
}
