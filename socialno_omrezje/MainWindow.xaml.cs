// MainWindow.xaml.cs
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;

namespace socialno_omrezje
{
    public partial class MainWindow : Window
    {
        private ViewModel viewModel;
        private UserProfileEditorControl userProfileEditorControl;

        public MainWindow()
        {
            InitializeComponent();

            viewModel = new ViewModel();
            DataContext = viewModel;
            userProfileEditorControl = new UserProfileEditorControl(viewModel);
        }

        private void exitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void postDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(viewModel.IzbranaObjava.Content, "Content");
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files |*.jpg;*.jpeg;*.png|All Files |*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                viewModel.IzbranaObjava.Slika = new System.Windows.Media.Imaging.BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        private void Uvozi_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML Files (*.xml)|*.xml";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                // Load data from the XML file into the existing ViewModel instance
                viewModel = ViewModel.LoadData<ViewModel>(filePath);

                // Update the DataContext with the existing ViewModel
                DataContext = viewModel;
            }
        }

        private void Izvozi_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML Files (*.xml)|*.xml";

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;

                // Serialize the current ViewModel instance to an XML file
                ViewModel.SaveData(viewModel, filePath);
            }
        }

        private void EditProfile_Click(object sender, RoutedEventArgs e)
        {
            userProfileEditorControl.Visibility = Visibility.Visible;
        }
    }
}


