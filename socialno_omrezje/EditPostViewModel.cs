using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace socialno_omrezje
{
    public class EditPostViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public RelayCommand UpdatePostCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand OpenImageDialogCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private PostData selectedPost;

        public ObservableCollection<PostData> WallPosts { get; set; }

        private string editedVsebina;
        public string EditedVsebina
        {
            get { return editedVsebina; }
            set
            {
                editedVsebina = value;
                RaisePropertyChanged(nameof(EditedVsebina));
            }
        }

        private string editedContent;
        public string EditedContent
        {
            get { return editedContent; }
            set
            {
                editedContent = value;
                RaisePropertyChanged(nameof(EditedContent));
            }
        }

        private ImageSource editedImage;
        public ImageSource EditedImage
        {
            get { return editedImage; }
            set
            {
                editedImage = value;
                RaisePropertyChanged(nameof(EditedImage));
            }
        }





        public EditPostViewModel(ObservableCollection<PostData> wallPosts, PostData post)
        {
            // Initialize properties with values from the provided 'post'
            SelectedPost = post;
            EditedVsebina = post.Vsebina;
            EditedContent = post.Content;

            // Assuming you have a method to convert the image path to BitmapImage
            EditedImage = ConvertImagePathToBitmapImage(post.ImagePath);

            WallPosts = wallPosts;

            // Initialize commands
            UpdatePostCommand = new RelayCommand(UpdatePost);
            CancelCommand = new RelayCommand(Cancel);
            OpenImageDialogCommand = new RelayCommand(OpenImageDialog);
        }





        public PostData SelectedPost
        {
            get { return selectedPost; }
            set
            {
                selectedPost = value;
                RaisePropertyChanged(nameof(SelectedPost));
            }
        }

        private void OpenImageDialog()
        {
            // Create OpenFileDialog
            var openFileDialog = new OpenFileDialog
            {
                Title = "Select an Image",
                Filter = "Image Files (*.jpg; *.jpeg; *.png; *.gif; *.bmp)|*.jpg; *.jpeg; *.png; *.gif; *.bmp|All Files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) // Set the initial directory if needed
            };

            // Show the dialog
            bool? result = openFileDialog.ShowDialog();

            // Process the result
            if (result == true)
            {
                // Get the selected file name and set it to the ViewModel property
                string selectedImagePath = openFileDialog.FileName;
                BitmapImage newImage = ConvertImagePathToBitmapImage(selectedImagePath);

                // Update the EditedImage property
                EditedImage = newImage;

                // Notify the UI about the change
                RaisePropertyChanged(nameof(EditedImage));
            }
        }

        private BitmapImage ConvertImagePathToBitmapImage(string imagePath)
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                try
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute);
                    bitmapImage.EndInit();
                    return bitmapImage;
                }
                catch (Exception ex)
                {
                    // Log or handle the exception, set a default image, or return null
                    Console.WriteLine($"Error loading image: {ex.Message}");
                }
            }

            // Return a default image or null if needed
            return null;
        }

        private void UpdatePost()
        {
            try
            {
                if (SelectedPost != null)
                {
                    SelectedPost.Vsebina = EditedVsebina;
                    SelectedPost.Content = EditedContent;
                    SelectedPost.Slika = EditedImage as BitmapImage;
                    SelectedPost.DatumObjave = DateTime.Now;

                    RaisePropertyChanged(nameof(SelectedPost));
                }
            }
            catch (Exception ex)
            {
                // Log or display the exception message for debugging
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void Cancel()
        {
            // If you want to discard changes and close the window
            CloseWindow();

            // Alternatively, if you want to reset the edited values to original values
            // assuming there is a method ResetToOriginalValues in your Data class
            // SelectedPost.ResetToOriginalValues();
            // RaisePropertyChanged(nameof(EditedVsebina));
            // RaisePropertyChanged(nameof(EditedContent));
            // RaisePropertyChanged(nameof(EditedTitle));
        }

        private void CloseWindow()
        {
            // Close the window
            var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
            if (currentWindow != null)
            {
                currentWindow.Close();
            }
        }
    }
}
